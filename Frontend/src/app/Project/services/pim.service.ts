import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class PIMService {
  constructor(public httpClient: HttpClient) {}
  localUrl: string = "https://localhost:44334/api";

  getProjects(searchText: string, searchStatus: string, sortingCol: string, sortingDirection: number, pageIndex: number, pageSize: number): Observable<any> {
    var query = new URLSearchParams();
    query.append("searchText", searchText);
    query.append("searchStatus", searchStatus);
    query.append("sortingCol", sortingCol);
    query.append("sortingDirection", sortingDirection.toString());
    query.append("pageIndex", pageIndex.toString());
    query.append("pageSize", pageSize.toString());
    return this.httpClient.get(this.localUrl + "/Project?" + query.toString());
  }

  getProjectCount(searchText: string, searchStatus: string): Observable<any> {
    var query = new URLSearchParams();
    query.append("searchText", searchText);
    query.append("searchStatus", searchStatus);
    return this.httpClient.get(this.localUrl + "/Project/count?" + query.toString());
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

  projectNumbersExist(pjNums: number[]): Observable<any> {
    return this.httpClient.request("POST", this.localUrl + "/Project/exist/", {
      body: pjNums,
    });
  }

  checkDeletableByPjNums(pjNums: number[]): Observable<any> {
    return this.httpClient.request("POST", this.localUrl + "/Project/deletable/", {
      body: pjNums,
    });
  }

  checkNonExistMemberByVisa(mems: string): Observable<any> {
    return this.httpClient.post<any>(this.localUrl + "/Employee/nonexist", {
      VisaStr: mems,
    });
  }

  checkExistMemberByVisa(mems: string): Observable<any> {
    return this.httpClient.post<any>(this.localUrl + "/Employee/exist", {
      VisaStr: mems,
    });
  }

  postProject(pj: any, memString: string) {
    return this.httpClient.post(this.localUrl + "/Project", {
      project: pj,
      memString: memString,
    });
  }

  putProject(pj: any, memString: string) {
    return this.httpClient.put(this.localUrl + "/Project", {
      project: pj,
      memString: memString,
    });
  }

  deleteProject(pjNums: number[]) {
    console.log(pjNums)
    return this.httpClient.request("DELETE", this.localUrl + "/Project/", {
      body: pjNums,
    });
  }
}
