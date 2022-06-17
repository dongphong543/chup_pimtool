import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { ProjectListComponent } from "./components";

import { NPSComponent } from "../Project/components";
import { UnexpectedErrorComponent } from "../Project/components/unexpected-error/unexpected-error.component";

const routes: Routes = [
  { path: "list", component: ProjectListComponent },
  { path: "new", component: NPSComponent },
  { path: "edit/:projectId", component: NPSComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProjectRoutingModule {}
