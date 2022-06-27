import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

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
  deleteProject(ids: number[]) {
    return this.httpClient.request("DELETE", this.localUrl + "/Project/", {
      body: ids,
    });
  }
}
