import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { NPSComponent } from "./Project/components";
import { UnexpectedErrorComponent } from "./Project/components/unexpected-error/unexpected-error.component";

const routes: Routes = [
  {
    path: "pim",
    loadChildren: () =>
      import("./Project/project.module").then((m) => m.ProjectModule),
  },

  
  { path: "error", component: UnexpectedErrorComponent },
  { path: "**", redirectTo: "/pim/list", pathMatch: "full" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
