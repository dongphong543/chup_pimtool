import { Component, ChangeDetectionStrategy } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { PIMService } from "src/app/Project/components/pim.service";

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
    public version: number
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
    this.getProjects();
  }

  getProjects() {
    this.service.getProjects().subscribe((response) => {
      // console.log(response);
      this.projects = response;
      this.projects.forEach((i) => {
        // if (i.startDate) i.startDate = i.startDate.slice(0, 10).split('-').reverse().join('.');
        // if (i.endDate) i.endDate = i.endDate.slice(0, 10).split('-').reverse().join('.');
      });
      this.projects.sort((a, b) => {
        return a.projectNumber - b.projectNumber;
      });
      // console.log(this.projects);
    });
  }

  searchProjectByText(i: number) {
    //  console.log("TEST" + i);
    // GridComponent.s
  }
}
