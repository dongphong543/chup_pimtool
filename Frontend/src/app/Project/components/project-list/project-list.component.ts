import { ChangeDetectionStrategy, Component } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { PageEvent } from "@angular/material/paginator";
import { ActivatedRoute } from "@angular/router";
import { PaginationInstance } from "ngx-pagination";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators"
import { PIMService } from "../../services/pim.service";


@Component({
  selector: "pim-project-list",
  styleUrls: ["./project-list.component.scss"],
  templateUrl: "./project-list.component.html",
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectListComponent {
  maxPageNumber: number;

  public paginationConfig: PaginationInstance = {
    id: 'project-list-pagination',
    itemsPerPage: 5,
    currentPage: 1,
    totalItems: 16
  };

  sortingCol: string;
  sortingDirection: number;

  projects$: Observable<any>;
  searchForm: FormGroup;

  searchText: string = "";
  searchStatus: string = "";

  checkedPjNum: number[] = [];
  

  constructor(private service: PIMService) {}
  
  getProjectsList() {
    return  this.service.getProjects(
            this.searchForm.get("searchText").value,
            this.searchForm.get("searchStatus").value,
            this.sortingCol,
            this.sortingDirection,
            this.paginationConfig.currentPage - 1,
            this.paginationConfig.itemsPerPage
    )
  }

  ngOnInit(): void {
    this.sortingCol = 'P';
    this.sortingDirection = 1;

    // Get ready for Search Form
    this.searchForm = new FormGroup({
      searchText: new FormControl(""),
      searchStatus: new FormControl(""),
    });

    this.searchForm.patchValue({
      searchText: localStorage.getItem("searchTextCriteria"),
      searchStatus: localStorage.getItem("searchStatusCriteria"),
    });

    // Get ready for Pagination
    this.maxPageNumber = 5;
    this.paginationConfig.itemsPerPage = 8;
    this.paginationConfig.currentPage = 1;
    this.service.getProjectCount(
      this.searchForm.get("searchText").value,
      this.searchForm.get("searchStatus").value
    ).subscribe(
      response => this.paginationConfig.totalItems = response
    )

    // Get projects list
    this.projects$ = this.getProjectsList();
  }

  onPageChange(number: number) {
    this.paginationConfig.currentPage = number;
    this.projects$ = this.getProjectsList();

    this.service.getProjectCount(
      this.searchForm.get("searchText").value,
      this.searchForm.get("searchStatus").value
    ).subscribe(
      response => {this.paginationConfig.totalItems = response;}
    );
    
  }

  onPressSortingHeader(s: string) {                        // 1 is ASC, 2 is DESC
    if (['P', 'N', 'S', 'C', 'D'].includes(s)) {
      if (this.sortingCol === s) {
        this.sortingDirection = (this.sortingDirection == 1 ? 2 : 1);
      }
      else {
        this.sortingCol = s;
        this.sortingDirection = 1;
      }
    }

    this.projects$ = this.getProjectsList();
  }

  getToPage1() {
    this.paginationConfig.currentPage = 1;
  }

  changedCheckbox(pjNum: number, checked: boolean) {
    if (checked == true) {
      this.checkedPjNum.push(pjNum);
    }

    else {
      let idx = this.checkedPjNum.findIndex((i) => i == pjNum);
      if (idx != -1) {
        this.checkedPjNum.splice(idx, 1);
      }
    }
  }

  deleteProject(pjNum: string) {

    if (confirm("Are you sure you want to delete this project?")) {
      this.service.projectNumbersExist([parseInt(pjNum)]).subscribe(
            response => {
              if (response == false) {
                alert("This project has been deleted already. Press OK to reload.");
                window.location.reload();
                return;
              }
              else {
                this.service.deleteProject([parseInt(pjNum)]).subscribe(
                  response => window.location.reload()
                );
              }
            }
          );
    }
    
  }

  deleteCheckedProject() {

    if (confirm("Are you sure you want to delete THESE projects?")) {
      this.service.projectNumbersExist(this.checkedPjNum).subscribe(
            response => {
              if (response == false) {
                alert("One of these projects has been deleted already. Press OK to reload.");
                window.location.reload();
                return;
              }
              else {
                this.service.checkDeletableByPjNums(this.checkedPjNum).subscribe(
                  response => {
                    console.log(response);
                    if (response == false) {
                      alert("Only NEW projects can be deleted.");
                      return;
                    }
                    else {
                      this.service.deleteProject(this.checkedPjNum).subscribe(
                        response => window.location.reload()
                      );
                    }
                  }
                )
              }
            }
          );
    }
  }

  searchProject() {
    let sText = this.searchForm.controls.searchText.value == null? "" : this.searchForm.controls.searchText.value,
        sStatus = this.searchForm.controls.searchStatus.value == null? "" : this.searchForm.controls.searchStatus.value;
    
    localStorage.setItem("searchTextCriteria", sText);
    localStorage.setItem("searchStatusCriteria", sStatus);

    this.getToPage1();

    this.projects$ = this.service.getProjects(
      sText,
      sStatus,
      this.sortingCol,
      this.sortingDirection,
      this.paginationConfig.currentPage - 1,
      this.paginationConfig.itemsPerPage);
    this.service.getProjectCount(sText, sStatus).subscribe(
      response => {this.paginationConfig.totalItems = response;}
    );

    this.projects$.subscribe((response) => {
      this.checkedPjNum = [];
    });
  }

  resetProject() {
    localStorage.setItem("searchTextCriteria", "");
    localStorage.setItem("searchStatusCriteria", "");

    this.searchForm.patchValue({
      searchText: "",
      searchStatus: "",
    });

    this.searchProject();
  }
}
