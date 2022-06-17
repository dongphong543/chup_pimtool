import { Component, OnInit } from "@angular/core";
import { BroadcastService } from "src/app/Error/broadcast.service";

@Component({
  selector: "app-unexpected-error",
  templateUrl: "./unexpected-error.component.html",
  styleUrls: ["./unexpected-error.component.scss"],
})
export class UnexpectedErrorComponent implements OnInit {
  constructor(public broadcastService: BroadcastService) {}

  ngOnInit(): void {
    // this.broadcastService.msg.subscribe(r => alert(r))
    // alert(this.broadcastService.msg);
  }
}
