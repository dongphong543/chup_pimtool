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

  checkboxCount: number = 0;

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

    this.projects$.subscribe((response) => (this.projectsArr = response));
  }

  sortOn(column: string) {
    this.sortedColumn$.next(column);
  }

  changedCheckbox(pjNum: number) {
    this.checkboxCount = 0;

    let index = this.projectsArr.findIndex((pj) => {
      return pj.projectNumber == pjNum;
    });
    this.projectsArr[index].checkbox = !this.projectsArr[index].checkbox;

    this.projectsArr.forEach((i) => {
      if (i.checkbox) {
        this.checkboxCount += 1;
      }
    });
  }

  deleteProject(pjNum: string) {
    this.projects$.subscribe((response) => {
      this.projectsArr = response;

      let id = this.getProjectIdFromPjNum(pjNum);
      if (id == null) {
        alert("This project has been deleted already. Press OK to reload.");
        window.location.reload();
        return;
      }

      if (confirm("Are you sure you want to delete this project?")) {
        this.service.deleteProject([id]).subscribe();
        console.log("Deleted");
      } else {
        console.log("Not deleted");
      }

      window.location.reload();
    });
  }

  deleteCheckedProject() {
    if (confirm("Are you sure you want to delete checked project(s)?")) {
      {
        for (let i = 0; i < this.projectsArr.length; ++i) {
          if (
            this.projectsArr[i].checkbox &&
            this.projectsArr[i].status != "NEW"
          ) {
            alert("Only the New projects can be deleted. Please select again.");
            window.location.reload();
            return;
          }
        }
        let delArr: number[] = [];
        this.projectsArr.forEach((i) => {
          if (i.checkbox) {
            delArr.push(i.id);
          }
        });
        this.service.deleteProject(delArr).subscribe();
        window.location.reload();
      }
    } else {
      console.log("Not deleted");
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
      this.projectsArr = response;
      this.checkboxCount = 0;
    });
  }

  getProjectIdFromPjNum(pjNum: string) {
    for (let i = 0; i < this.projectsArr.length; ++i) {
      if (pjNum == this.projectsArr[i].projectNumber.toString())
        return this.projectsArr[i].id;
    }

    return null;
  }

  resetProject() {
    localStorage.setItem("searchTextCriteria", "");
    localStorage.setItem("searchStatusCriteria", "");
    window.location.reload();
  }
}
