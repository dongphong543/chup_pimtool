import { Location } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { combineLatest, Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { BroadcastService } from "../../../Error/broadcast.service";
import { dateValidator } from "../../validators/date-validator";
import { existPjNumValidator } from "../../validators/existed-pjNum-validator";
import { memberValidator } from "../../validators/member-validator";
import { emptyOrSpacesValidator } from "../../validators/empty-or-spaces-validator";
import { PIMService } from "../../services/pim.service";

class Project {
  constructor(
    public id: number,
    public groupId: number,
    public projectNumber: number,
    public name: string,
    public customer: string,
    public status: string,
    public startDate: Date,
    public endDate: Date,
    public version: number
  ) {}
}

@Component({
  selector: "app-nps",
  templateUrl: "./nps.component.html",
  styleUrls: ["./nps.component.scss"],
})
export class NPSComponent implements OnInit {
  nowInEditMode: boolean;
  isMemberNotFound: boolean;
  memberNotFound$: Observable<any>;
  isSubmitButtonPressed: boolean = false;

  pjId: number = null;
  pjVersion: number = 0;

  newProjectForm: FormGroup;
  groups$: Observable<any>;
  employees$: Observable<any>;
  nonExistEmployees$: Observable<any>;
  projectLeaderName$: Observable<any>;

  pjNumError: boolean;

  statuses = [
    { name: "New", code: "NEW" },
    { name: "Planned", code: "PLA" },
    { name: "In progress", code: "INP" },
    { name: "Finished", code: "FIN" },
  ];

  pj: Project;

  constructor(
    private service: PIMService,
    public broadcastService: BroadcastService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    let urlSegment: UrlSegment[] = this.activatedRoute.snapshot.url;
    this.nowInEditMode = urlSegment[0].path == "edit";
    this.isMemberNotFound = false;

    this.groups$ = this.service.getGroups();
    this.employees$ = this.service.getEmployees();

    this.newProjectForm = new FormGroup(
      {
        pjNum: new FormControl(
          { value: "", disabled: this.nowInEditMode },
          [
            Validators.required,
            Validators.min(1),
            Validators.max(9999),
            Validators.pattern("^[0-9]*$"),
          ],
          [existPjNumValidator(this.service)]
          // []
        ),
        name: new FormControl("", [
          Validators.required,
          Validators.maxLength(50),
          emptyOrSpacesValidator
        ]),
        customer: new FormControl(null, [
          Validators.required,
          Validators.maxLength(50),
          emptyOrSpacesValidator
        ]),
        group: new FormControl("", Validators.required),
        member: new FormControl("", [], [memberValidator(this.service)]),
        status: new FormControl("NEW", Validators.required),
        startDate: new FormControl(null, Validators.required),
        endDate: new FormControl(null),
      },
      {
        validators: [dateValidator],
        asyncValidators: [],
        updateOn: "change"
      }
    );

    if (this.nowInEditMode) {
      this.service
        .getProjectByPjNum(parseInt(urlSegment[1].path))
        .subscribe((response) => {
          this.newProjectForm.patchValue({
            pjNum: response.projectNumber,
            name: response.name,
            customer: response.customer,
            group: response.groupId,
            member: response.employees.map((e) => e.visa).toString(),
            status: response.status,
            startDate: response.startDate?.split("T")[0],
            endDate: response.endDate?.split("T")[0],
          });

          this.pjId = response.id;
          this.pjVersion = response.version;
        });
    }
    else {
      if (localStorage.getItem("projectForm") != null) {
        of(localStorage.getItem("projectForm")).subscribe(
          response => this.newProjectForm.patchValue(JSON.parse(response))
        );
      }  
    }

    this.projectLeaderName$ = combineLatest([
      this.groups$,
      this.employees$,
    ]).pipe(
      map(([arr1, arr2]) => {
        return arr1.map((gr) => ({
          id: gr.id,
          groupLeaderId: gr.groupLeaderId,
          fullName: arr2
            .filter((em) => em.id == gr.groupLeaderId)
            .map((em) => em.firstName + " " + em.lastName)
            .toString(),
        }));
      })
    );
  }

  onSubmit() {
    this.isSubmitButtonPressed = true;
    this.nonExistEmployees$ = this.service.checkNonExistMemberByVisa(
      this.newProjectForm.controls.member.value
    );

    if (this.newProjectForm.invalid) return;

    this.pj = new Project(
      this.pjId,
      this.newProjectForm.controls.group.value,
      this.newProjectForm.controls.pjNum.value,
      this.newProjectForm.controls.name.value,
      this.newProjectForm.controls.customer.value,
      this.newProjectForm.controls.status.value,
      this.newProjectForm.controls.startDate.value,
      this.newProjectForm.controls.endDate.value?.length == 0
        ? null
        : this.newProjectForm.controls.endDate.value,
      this.pjVersion
    );
    
    localStorage.removeItem("projectForm");
    
    if (this.nowInEditMode == false) {    // New project mode
      delete this.pj.id;
      delete this.pj.version;
      this.service.projectNumbersExist([this.pj.projectNumber]).subscribe(
        response => {
          if (response == false) {
            this.addProject();
          }
          else {
            this.newProjectForm.get('pjNum').setErrors({existPjNumError: true});
            console.log(this.newProjectForm.get('pjNum').errors)
          }
        }
      );
    }

    else this.editProject();              // Edit project mode
  }

  onCancel() {
    if (this.nowInEditMode == false) {
      localStorage.setItem("projectForm", JSON.stringify(this.newProjectForm.value));
    }

    this.router.navigate(["/"]);
  }

  addProject() {
    if (this.newProjectForm.invalid) return;
    else {
      this.service
      .postProject(this.pj, this.newProjectForm.get("member").value)
      .subscribe((response) => this.router.navigate(["/"]));
    }
  }

  editProject() {
    if (this.newProjectForm.invalid) return;
    else {
      this.service
      .putProject(this.pj, this.newProjectForm.get("member").value)
      .subscribe((response) => this.router.navigate(["/"]));
    } 
  }

  setSubmittedFalse() {
    this.isSubmitButtonPressed = false;
  }
}
