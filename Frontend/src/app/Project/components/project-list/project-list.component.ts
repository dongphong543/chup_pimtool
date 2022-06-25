import { Component, ChangeDetectionStrategy } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
// import * as Errors from "../../../Error/error-declaration";
import { PIMService } from "../../services/pim.service";
import { Observable, combineLatest, BehaviorSubject } from "rxjs";
import { filter, map, scan } from "rxjs/operators";
import { Router } from "@angular/router";
import { FormControl, FormGroup } from "@angular/forms";

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
  // reservedProjects: Project[];
  projectsArr: Project[];
  projects$: Observable<any>;
  sortedColumn$ = new BehaviorSubject<string>("");
  searchForm: FormGroup;
  sortDirection$: Observable<any>;

  searchText: string = "";
  searchStatus: string = "";

  checkboxCount: number = 0;

  constructor(private service: PIMService, private router: Router) { }

  ngOnInit(): void {
    this.searchForm = new FormGroup(
      {
        searchText: new FormControl(""),
        searchStatus: new FormControl("")
      }
    );

    this.searchForm.patchValue({
      searchText: localStorage.getItem("searchTextCriteria"),
      searchStatus: localStorage.getItem("searchStatusCriteria")
    });

    this.projects$ = this.service.getProjects(this.searchForm.get('searchText').value, this.searchForm.get('searchStatus').value);
    // this.sortDirection$ = this.sortedColumn$.pipe(
    //   scan<string, { col: string; dir: string }>(
    //     (sort, val) => {
    //       return sort.col === val
    //         ? { col: val, dir: sort.dir === "desc" ? "asc" : "desc" }
    //         : { col: val, dir: "desc" };
    //     },
    //     { dir: "desc", col: "" }
    //   )
    // );
    
    // this.projects$ = combineLatest(
    //   this.service.getProjects(),
    //   this.sortDirection$
    // ).pipe(
    //   map(([list, sort]) =>
    //     !sort.col ? list : this.service.sortByColumn(list, sort.col, sort.dir)
    //   )
    // );
    // this.sortOn("projectNumber");
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
          // throw new Errors.ProjectNotFoundError(pjNum);
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
      },
    );
  }

  deleteCheckedProject() {
      if (confirm("Are you sure you want to delete checked project(s)?")) {
        {
          for (let i = 0; i < this.projectsArr.length; ++i) {
            if (this.projectsArr[i].checkbox && this.projectsArr[i].status != "NEW") {
              alert("Only the New projects can be deleted. Please select again.");
              window.location.reload();
              return;
            }
          }
          let delArr: number[] = [];
          this.projectsArr.forEach((i) => {
            if (i.checkbox) {
              // this.service.deleteProject(i.id).subscribe();
              delArr.push(i.id);
            }
          });
          this.service.deleteProject(delArr).subscribe();

        }
        // window.location.reload();
      }
      else {
        console.log("Not deleted");
      }
    }
    // );
  // }

  searchProject() {
    localStorage.setItem("searchTextCriteria", this.searchForm.controls.searchText.value);
    localStorage.setItem("searchStatusCriteria", this.searchForm.controls.searchStatus.value);

    // this.projects$ = this.projects$.pipe(filter(pjs => pjs.projectNumber == 1200))
    // this.projects$.subscribe(r => console.log(r))
    // console.log(this.searchText, this.searchStatus);
    // if (this.searchText.length > 0) {
    //   this.searchProjectByText(this.searchText);
    // }
    // if (this.searchStatus.length > 0) {
    //   this.searchProjectByStatus(this.searchStatus);
    // }
    this.projects$ = this.service.getProjects(this.searchForm.get('searchText').value, this.searchForm.get('searchStatus').value);

  }

  // searchProjectByStatus(i: string) {
  //   this.projectsArr = this.projectsArr.filter((pj) => pj.status.toString() == i);
  //   return;
  // }

  // searchProjectByText(i: string) {
  //   // this.projectsArr = this.projectsArr.filter((pj) =>
  //   //   this.searchTextCriteria(pj, i)
  //   // );
  //   // return;

  //   this.projects$ = this.projects$.pipe(filter(pj => pj.projectNumber == 123))
  //                                                     // pj.projectNumber.toString().includes(i) ||
  //                                                     // pj.name.toString().includes(i) ||
  //                                                     // pj.customer.toString().includes(i)));
  //   // this.projects$.subscribe();
  //   console.log(i)
  // }

  // searchTextCriteria(pj: Project, i: string) {
  //   return (
  //     pj.projectNumber.toString().includes(i) ||
  //     pj.name.toString().includes(i) ||
  //     pj.customer.toString().includes(i)
  //   );
  // }

  getProjectIdFromPjNum(pjNum: string) {
    for (let i = 0; i < this.projectsArr.length; ++i) {
      if (pjNum == this.projectsArr[i].projectNumber.toString())
        return this.projectsArr[i].id;
    }

    return null;
  }

  resetProject() {
    // // this.projects = JSON.parse(JSON.stringify(this.reservedProjects));
    // if (this.searchText == null) this.searchText = "";
    // if (this.searchStatus == null) this.searchStatus = "";
    // // return;
    // this.projects$ = this.service.getProjects();
    // this.sortOn('projectNumber');
    localStorage.setItem("searchTextCriteria", "");
    localStorage.setItem("searchStatusCriteria", "");
    window.location.reload();
  }
}
