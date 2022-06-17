import { Component, ChangeDetectionStrategy } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";

@Component({
  selector: "pim-shell",
  templateUrl: "./shell.component.html",
  styleUrls: ["./shell.component.scss"],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ShellComponent {
  constructor(private translate: TranslateService) {
    // trans.addLangs(["en", "fr"]);
    // trans.setDefaultLang("en");
  }

  setLanguage(lang: string) {
    this.translate.use(lang);
  }
}
