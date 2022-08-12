import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class BroadcastService {
  public err: BehaviorSubject<any>;

  constructor() {
    this.err = new BehaviorSubject<any>("");
  }
}
