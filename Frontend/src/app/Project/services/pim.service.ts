import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";


// class ProjectAddDTO {
//   project: any;
//   memString: string;

//   constructor(p: any, s: string) {
//     this.project = p;
//     this.memString= s;
//   }
// }


@Injectable({
  providedIn: "root",
})

export class PIMService {
  constructor(public httpClient: HttpClient) {}
  localUrl: string = "https://localhost:44334/api";

  getProjects(searchText: string, searchCriteria: string): Observable<any> {
    var query = new URLSearchParams();
    query.append("searchText", searchText);
    query.append("searchCriteria", searchCriteria);
    return this.httpClient.get(this.localUrl + "/Project?" + query.toString());
    // return this.httpClient.get<any>(this.localUrl + "/Project");
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
    return this.httpClient.post<any>(this.localUrl + "/Employee/nonexist", {VisaStr: mems});
  }

  checkExistMemberByVisa(mems: string): Observable<any> {
    return this.httpClient.post<any>(this.localUrl + "/Employee/exist", {VisaStr: mems});
  }

  postProject(pj: any, memString: string) {
    return this.httpClient.post(this.localUrl + "/Project", {project: pj, memString: memString});
  }

  putProject(pj: any, memString: string) {
    // var query = new URLSearchParams();
    // query.append("memString", memString);
    // return this.httpClient.put(this.localUrl + "/Project?" + query.toString() , pj);

    return this.httpClient.put(this.localUrl + "/Project", {project: pj, memString: memString});

    // .subscribe(
    //   (response) => {
    //     console.log("UPDATE")
    //     console.log(response);
    //   },
    //   (error) => {
    //     console.log(error);
    //   }
    // );
  }

  // postProjectEmployee(pjNum: number, emVisa: string[]) {
  //   return this.httpClient.post(this.localUrl + "/ProjectEmployee", {ProjectPjNum: pjNum, EmployeeVisa: emVisa})
  // }

  deleteProject(ids: number[]) {
    // return this.httpClient.delete(this.localUrl + "/Project/", ids)

    return this.httpClient.request('DELETE', this.localUrl + "/Project/", {body: ids});
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

  // postProjectEmployee(pjId: number, emId: number) {
  //   this.httpClient
  //     .post(this.localUrl + "/ProjectEmployee", {
  //       projectId: pjId,
  //       employeeId: emId,
  //     })
  //     .subscribe(
  //       (response) => {
  //         console.log(response);
  //       },
  //       (error) => {
  //         console.log(error);
  //       }
  //     );
  // }
}
