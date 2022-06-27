import { ChangeDetectionStrategy, Component } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable } from "rxjs";
import { PIMService } from "../../services/pim.service";

export class Project {
  constructor(
    public id: number,
    public groupId: number,
    public projectNumber: number,
    public name: string,
    public customer: string,
    public status: string,
    public startDate: string,
    public endDate: string,
    public version: number,
    public checkbox: boolean
  ) {}
}

@Component({
  selector: "pim-project-list",
  styleUrls: ["./project-list.component.scss"],
  templateUrl: "./project-list.component.html",
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectListComponent {
  projectsArr: Project[];
  projects$: Observable<any>;
  sortedColumn$ = new BehaviorSubject<string>("");
  searchForm: FormGroup;
  sortDirection$: Observable<any>;

  searchText: string = "";
  searchStatus: string = "";

  checkedPjNum: number[] = [];

  constructor(private service: PIMService, private router: Router) {}

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

    // this.projects$.subscribe((response) => (this.projectsArr = response));
  }

  sortOn(column: string) {
    this.sortedColumn$.next(column);
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
    // console.log(this.checkedPjNum);
  }

  deleteProject(pjNum: string) {

    if (confirm("Are you sure you want to delete this project?")) {
      this.service.checkProjectByPjNums([parseInt(pjNum)]).subscribe(
            response => {
              // console.log(parseInt(pjNum))
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
      this.service.checkProjectByPjNums(this.checkedPjNum).subscribe(
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
    localStorage.setItem(
      "searchTextCriteria",
      this.searchForm.controls.searchText.value
    );
    localStorage.setItem(
      "searchStatusCriteria",
      this.searchForm.controls.searchStatus.value
    );

    this.projects$ = this.service.getProjects(
      this.searchForm.get("searchText").value,
      this.searchForm.get("searchStatus").value
    );
    this.projects$.subscribe((response) => {
      // this.projectsArr = response;
      this.checkedPjNum = [];
    });
  }

  resetProject() {
    localStorage.setItem("searchTextCriteria", "");
    localStorage.setItem("searchStatusCriteria", "");
    window.location.reload();
  }
}
