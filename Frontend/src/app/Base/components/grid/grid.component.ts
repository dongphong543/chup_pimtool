import { Component, ChangeDetectionStrategy } from "@angular/core";


@Component({
  selector: "pim-grid",
  templateUrl: "./grid.component.html",
  styleUrls: ["./grid.component.scss"],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GridComponent {
  constructor() {}
}
