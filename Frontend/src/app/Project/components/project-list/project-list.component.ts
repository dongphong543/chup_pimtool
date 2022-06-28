import { ChangeDetectionStrategy, Component } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";
import { PIMService } from "../../services/pim.service";


@Component({
  selector: "pim-project-list",
  styleUrls: ["./project-list.component.scss"],
  templateUrl: "./project-list.component.html",
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectListComponent {
  projects$: Observable<any>;
  searchForm: FormGroup;
  sortDirection$: Observable<any>;

  searchText: string = "";
  searchStatus: string = "";

  checkedPjNum: number[] = [];

  constructor(private service: PIMService, private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {

    this.searchForm = new FormGroup({
      searchText: new FormControl(""),
      searchStatus: new FormControl(""),
    });

    this.searchForm.patchValue({
      searchText: localStorage.getItem("searchTextCriteria"),
      searchStatus: localStorage.getItem("searchStatusCriteria"),
    });

    this.projects$ = this.service.getProjects(
      this.searchForm.get("searchText").value,
      this.searchForm.get("searchStatus").value
    );
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

    this.projects$ = this.service.getProjects(sText, sStatus);

    this.projects$.subscribe((response) => {
      this.checkedPjNum = [];
    });

    
  }

  resetProject() {
    localStorage.removeItem("searchTextCriteria");
    localStorage.removeItem("searchStatusCriteria");

    this.searchForm.patchValue({
      searchText: "",
      searchStatus: "",
    });

    this.searchProject();
  }
}
