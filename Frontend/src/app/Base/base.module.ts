import { CommonModule } from "@angular/common";
import { NgModule, ModuleWithProviders } from "@angular/core";
import { RouterModule } from "@angular/router";
import { TranslateModule } from "@ngx-translate/core";


// All exported items hear need to declare in public_api.ts
const DECLARED_EXPORTS = [];

const ENTRY_COMPONENTS = [];

const RELAYED_EXPORTS = [CommonModule, TranslateModule];

@NgModule({
  declarations: [...DECLARED_EXPORTS],
  providers: [],
  imports: [RouterModule, ...RELAYED_EXPORTS],
  exports: [...RELAYED_EXPORTS, ...DECLARED_EXPORTS],
})
export class PIMBaseModule {
  static forRoot(): ModuleWithProviders<PIMBaseModule> {
    return {
      ngModule: PIMBaseModule,
    };
  }
}
