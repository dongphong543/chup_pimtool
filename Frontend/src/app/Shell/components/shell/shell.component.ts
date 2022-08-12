import { Component, ChangeDetectionStrategy } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";

@Component({
  selector: "pim-shell",
  templateUrl: "./shell.component.html",
  styleUrls: ["./shell.component.scss"],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ShellComponent {
  isEn: boolean = true;

  constructor(private translate: TranslateService) {}

  setLanguage(lang: string) {
    this.translate.use(lang);
    this.isEn = lang == "en";
  }
}
