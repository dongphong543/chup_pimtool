import { CommonModule, Location } from "@angular/common";
import {
  HttpClient,
  HttpClientModule,
  HttpInterceptor,
  HTTP_INTERCEPTORS,
} from "@angular/common/http";
// import { HttpErrorInterceptor } from './http-error.interceptor';
import { HttpErrorInterceptorService } from "./Error/error-interceptor";

import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import {
  TranslateLoader,
  TranslateModule,
  TranslateService,
} from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";

import { AppRoutingModule } from "./app-routing.module";
import { PIMBaseModule } from "./Base/base.module";
import { ShellComponent } from "./Shell/components";
import { ShellModule } from "./Shell/shell.module";
import { ApiConfiguration } from "./swagger/api-configuration";
import { EnvironmentApiConfiguration } from "./api-config";
import { FormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

export function HttpLoaderFactory(http: HttpClient, loc: Location) {
  return new TranslateHttpLoader(
    http,
    loc.prepareExternalUrl("/assets/i18n/"),
    ".json"
  );
}

@NgModule({
  declarations: [],
  imports: [
    BrowserModule,
    AppRoutingModule,
    PIMBaseModule.forRoot(),
    ShellModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient, Location],
      },
    }),
    HttpClientModule,
    FormsModule,
    CommonModule,
    BrowserAnimationsModule
  ],
  providers: [
    {
      provide: ApiConfiguration,
      useClass: EnvironmentApiConfiguration as any,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptorService,
      multi: true,
    },
  ],
  bootstrap: [ShellComponent],
})
export class AppModule {
  constructor(translate: TranslateService) {
    translate.setDefaultLang("en");
    translate.addLangs(["en", "fr"]);
  }
}
