import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class BroadcastService {
  public msg: BehaviorSubject<string>;
  public err: BehaviorSubject<any>;

  constructor() {
    //initialize it to false
    this.msg = new BehaviorSubject<string>("");
    this.err = new BehaviorSubject<any>("");
  }
}
