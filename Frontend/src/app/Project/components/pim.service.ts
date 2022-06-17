import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable({
  providedIn: "root",
})
export class PIMService {
  constructor(public httpClient: HttpClient) {}
  localUrl: string = "https://localhost:44334/api";

  getProjects(): Observable<any> {
    return this.httpClient.get<any>(this.localUrl + "/Project");
  }

  getProjectByPjNum(pjNum: number): Observable<any> {
    return this.httpClient.get<any>(this.localUrl + "/Project/pjNum/" + pjNum);
  }

  getEmployees(): Observable<any> {
    return this.httpClient.get<any>(this.localUrl + "/Employee");
  }

  getGroups(): Observable<any> {
    return this.httpClient.get<any>(this.localUrl + "/Group");
  }

  postProject(pj: any) {
    this.httpClient.post(this.localUrl + "/Project", pj).subscribe(
      (response) => {
        console.log(response);
      },
      // (error) => {
      //   console.log(error);
      // }
    );
  }

  putProject(pj: any) {
    return this.httpClient.put(this.localUrl + "/Project/" + pj.id, pj).subscribe(
      (response) => {
        console.log("UPDATE")
        console.log(response);
      },
      (error) => {
        console.log(error);
      }
    );
  }

  deleteProject(id: number) {
    return this.httpClient.delete(this.localUrl + "/Project/" + id.toString());
  }

  sortByColumn(
    list: any[] | undefined,
    column: string,
    direction = "desc"
  ): any[] {
    let sortedArray = (list || []).sort((a, b) => {
      if (a[column] > b[column]) {
        return direction === "desc" ? 1 : -1;
      }
      if (a[column] < b[column]) {
        return direction === "desc" ? -1 : 1;
      }
      return 0;
    });
    return sortedArray;
  }

  postProjectEmployee(pjId: number, emId: number) {
    this.httpClient
      .post(this.localUrl + "/ProjectEmployee", {
        projectId: pjId,
        employeeId: emId,
      })
      .subscribe(
        (response) => {
          console.log(response);
        },
        (error) => {
          console.log(error);
        }
      );
  }
}
