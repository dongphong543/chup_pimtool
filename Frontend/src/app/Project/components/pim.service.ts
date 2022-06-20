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

  checkProjectByPjNum(pjNum: number): Observable<any> {
    return this.httpClient.get<any>(this.localUrl + "/Project/exist/" + pjNum);
  }

  checkNonExistMemberByVisa(mems: string): Observable<any> {
    return this.httpClient.post<any>(this.localUrl + "Employees/nonexist", mems);
  }

  postProject(pj: any) {
    return this.httpClient.post(this.localUrl + "/Project", pj);
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
    return this.httpClient.delete(this.localUrl + "/Project/" + id.toString())
    // .pipe(
    //   catchError(e => {
    //     if (e) console.log(e.status);
    //     return of(null);
    //   })
    // );
  }

  sortByColumn(
    list: any[] | undefined,
    column: string,
    direction = "desc"
  ): any[] {
    // let sortedArray = list;
    // console.log(list)
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
