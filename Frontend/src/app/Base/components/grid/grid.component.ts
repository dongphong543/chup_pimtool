import { Component, ChangeDetectionStrategy } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { PIMService } from "src/app/Project/services/pim.service";

export class Project {
  constructor(
    public id: number,
    public groupId: number,
    public projectNumber: number,
    public name: string,
    public customer: string,
    public status: string,
    public startDate: Date,
    public endDate: Date,
    public version: number,
  ) {}
}

@Component({
  selector: "pim-grid",
  templateUrl: "./grid.component.html",
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GridComponent {
  // projects: Observable<Project[]>;
  projects: Project[];

  constructor(private service: PIMService) {}

  ngOnInit(): void {
    // this.getProjects();
  }

  // getProjects() {
  //   this.service.getProjects().subscribe((response) => {
  //     // console.log(response);
  //     this.projects = response;
  //     this.projects.forEach((i) => {
  //     });
  //     this.projects.sort((a, b) => {
  //       return a.projectNumber - b.projectNumber;
  //     });
  //   });
  // }

  searchProjectByText(i: number) {
    //  console.log("TEST" + i);
    // GridComponent.s
  }
}
