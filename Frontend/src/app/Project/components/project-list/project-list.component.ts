import { Component, ChangeDetectionStrategy } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
// import * as Errors from "../../../Error/error-declaration";
import { PIMService } from "../pim.service";
import { Observable, combineLatest, BehaviorSubject } from "rxjs";
import { filter, map, scan } from "rxjs/operators";

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
  sortDirection$ = this.sortedColumn$.pipe(
    scan<string, { col: string; dir: string }>(
      (sort, val) => {
        return sort.col === val
          ? { col: val, dir: sort.dir === "desc" ? "asc" : "desc" }
          : { col: val, dir: "desc" };
      },
      { dir: "desc", col: "" }
    )
  );

  searchText: string = "";
  searchStatus: string = "";

  checkboxCount: number = 0;

  constructor(private service: PIMService) {
    // this.projects$ = this.httpClient.get<any>("http://localhost:44386/api/Project");
    // console.log("HERE");
  }

  ngOnInit(): void {
    // this.getProjects();
    this.projects$ = combineLatest(
      this.service.getProjects(),
      this.sortDirection$
    ).pipe(
      map(([list, sort]) =>
        !sort.col ? list : this.service.sortByColumn(list, sort.col, sort.dir)
      )
    );
    this.sortOn("projectNumber");
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

    // console.log(this.checkboxCount);
  }

  getProjects() {
    // this.httpClient
    //   .get<any>("http://localhost:44386/api/Project")
    //   .subscribe((response) => {
    //     // console.log(response);
    //     this.projects = response;
    //     this.projects.forEach((i) => {
    //       // if (i.startDate)
    //       //   i.startDate = i.startDate
    //       //     .slice(0, 10)
    //       //     .split("-")
    //       //     .reverse()
    //       //     .join(".");
    //       // if (i.endDate)
    //       //   i.endDate = i.endDate.slice(0, 10).split("-").reverse().join(".");
    //       i.checkbox = false;
    //     });
    //     this.projects.sort((a, b) => {
    //       return a.projectNumber - b.projectNumber;
    //     });
    //     this.reservedProjects = JSON.parse(JSON.stringify(this.projects));
    //     console.log(this.projects);
    //   });
  }

  deleteProject(pjNum: string) {

    // for (let i = 0; i < this.projectsArr.length; ++i) {
    //   if (pjNum == this.projectsArr[i].projectNumber.toString())
    //     return this.projectsArr[i].id;
    // }

    // return null;

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
          this.service.deleteProject(id).subscribe();
          console.log("Deleted");
        } else {
          console.log("Not deleted");
        }

        window.location.reload();
    });




    // let id = this.getProjectIdFromPjNum(pjNum);
    // if (id == null) {
    //   // throw new Errors.ProjectNotFoundError(pjNum);
    //   alert("Deleted already");
    //   return;
    // }

    // if (confirm("Are you sure you want to delete this project?")) {
    //   this.service.deleteProject(id).subscribe();
    //   console.log("Deleted");
    // } else {
    //   console.log("Not deleted");
    // }

    // // this.getProjects();
    // window.location.reload();
  }

  deleteCheckedProject() {
    // this.projects$.subscribe((response) => {
    //   // let tempPjArr = this.projectsArr;

      if (confirm("Are you sure you want to delete checked project(s)?")) {
        {
          for (let i = 0; i < this.projectsArr.length; ++i) {
            if (this.projectsArr[i].checkbox && this.projectsArr[i].status != "NEW") {
              // throw new Errors.DeleteNotNewProjectError(i.projectNumber);
              alert("Only the New projects can be deleted. Please select again.");
              window.location.reload();
              return;
            }
          }
          this.projectsArr.forEach((i) => {
            if (i.checkbox) {
              this.service.deleteProject(i.id).subscribe();
            }
          });
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
    this.searchText = (<HTMLInputElement>(
      document.getElementById("textSelect")
    )).value;
    this.searchStatus = (<HTMLInputElement>(
      document.getElementById("statusSelect")
    )).value;

    // this.projects$ = this.projects$.pipe(filter(pjs => pjs.projectNumber == 1200))
    // this.projects$.subscribe(r => console.log(r))
    // console.log(this.searchText, this.searchStatus);
    // if (this.searchText.length > 0) {
    //   this.searchProjectByText(this.searchText);
    // }
    // if (this.searchStatus.length > 0) {
    //   this.searchProjectByStatus(this.searchStatus);
    // }
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
    window.location.reload();
  }
}
