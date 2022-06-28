import { NgModule } from "@angular/core";

import { PIMBaseModule } from "@base";
import { ProjectListComponent } from "./components";
import { ProjectRoutingModule } from "./project-routing.module";
import { NPSComponent } from "./components/nps/nps.component";

import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { Ng2SearchPipeModule } from "ng2-search-filter";
import { SidebarComponent } from "./components/sidebar/sidebar.component";
import { UnexpectedErrorComponent } from "./components/unexpected-error/unexpected-error.component";

import { StatusPipe } from "./pipes/project-status.pipe"

@NgModule({
  declarations: [
    ProjectListComponent,
    NPSComponent,
    SidebarComponent,
    UnexpectedErrorComponent,
    StatusPipe
  ],
  providers: [],
  imports: [
    ProjectRoutingModule,
    PIMBaseModule,
    FormsModule,
    ReactiveFormsModule,
    Ng2SearchPipeModule,
  ],
})
export class ProjectModule {}
