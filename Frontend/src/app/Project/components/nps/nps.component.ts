import { Component, OnInit } from "@angular/core";
import { Location } from "@angular/common";
import { Project } from "../../../Base/components/grid/grid.component";
import {
  FormBuilder,
  FormGroup,
  FormControl,
  Validators,
  FormControlName,
  FormArray,
  ValidatorFn,
  AbstractControl,
} from "@angular/forms";
import { PIMService } from "../../services/pim.service";
import { Observable, combineLatest, BehaviorSubject, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { BroadcastService } from "../../../Error/broadcast.service";
import { dateValidator } from "../../directives/date-validator.directive";
import { existPjNumValidator } from "../../directives/existed-pjNum-validator.directive";
import { formFullValidator } from "../../directives/form-full-validator.directive";
import { memberValidator } from "../../directives/member-validator.directive";
import { Router } from "@angular/router";

class Employee {
  constructor(
    public id: number,
    public visa: string,
    public firstName: string,
    public lastName: string,
    public birthDate: Date,
    public version: number
  ) {}
}

class Group {
  constructor(
    public id: number,
    public groupLeaderId: number,
    public version: number
  ) {}
}

class Form {
  constructor(
    public pjNum: number,
    public name: string,
    public customer: string,
    public groupId: number,
    public members: string,
    public status: string,
    public startDate: Date,
    public endDate: Date
  ) {}
}

@Component({
  selector: "app-nps",
  templateUrl: "./nps.component.html",
  styleUrls: ["./nps.component.scss"],
})
export class NPSComponent implements OnInit {
  nowInEPS: boolean;
  isMemberNotFound: boolean;
  memberNotFound$: Observable<any>;
  isSubmitted: boolean = false;

  pjId: number = null;
  pjVersion: number = 0;

  npsForm: FormGroup;
  groups$: Observable<any>;
  employees$: Observable<any>;
  nonExistEmployees$: Observable<any>;
  projectLeaderName$: Observable<any>;

  pjNumError: boolean;

  // ngStyleString: string = $"{
  //   'border-color':
  //     npsForm.get('pjNum').errors?.existPjNumError && isSubmitted
  //       ? 'red'
  //       : ''
  // }"

  statuses = [
    { name: "New", code: "NEW" },
    { name: "Planned", code: "PLA" },
    { name: "In Progress", code: "INP" },
    { name: "Finished", code: "FIN" },
  ];

  pj: Project;

  constructor(
    private service: PIMService,
    private location: Location,
    public broadcastService: BroadcastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    let path: string = this.location.path();
    this.nowInEPS = path.slice(5, 9) == "edit";
    // console.log(path.slice(5, 9))
    this.isMemberNotFound = false;

    // console.log(path.slice(5, path.length + 1))

    this.groups$ = this.service.getGroups();
    this.employees$ = this.service.getEmployees();

    this.npsForm = new FormGroup(
      {
        pjNum: new FormControl(
          { value: "", disabled: this.nowInEPS },
          [Validators.required, Validators.min(1), Validators.max(9999)],
          [existPjNumValidator(this.service)]
        ),
        name: new FormControl("", [Validators.required, Validators.maxLength(50)]),
        customer: new FormControl(null, [Validators.required, Validators.maxLength(50)]),
        group: new FormControl("", Validators.required),
        member: new FormControl("", [], [memberValidator(this.service)]),
        status: new FormControl("NEW", Validators.required),
        startDate: new FormControl(null, Validators.required),
        endDate: new FormControl(null),
      },
      {
        validators: [dateValidator],
        asyncValidators: [],
        updateOn: "change",
      }
    );

    // this.nonExistEmployees$ = this.service.checkNonExistMemberByVisa(this.npsForm.controls.member.value);

      if (this.nowInEPS) {
        this.service.getProjectByPjNum(parseInt(path.slice(10, path.length + 1))).subscribe(
          response => 
            { this.npsForm.patchValue({
              pjNum: response.projectNumber,
              name: response.name,
              customer: response.customer,
              group: response.groupId,
              member: response.employees.map(e => e.visa).toString(),
              status: response.status,
              startDate: response.startDate?.split('T')[0],
              endDate: response.endDate?.split('T')[0]           
              })

              this.pjId = response.id;
              this.pjVersion = response.version;

            }
        );
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
    this.isSubmitted = true;
    // if (this.formFull() == false) return;
    // console.log(this.npsForm.errors.dateError);
    // if (this.npsForm.get("pjNum").invalid || this.npsForm.errors?.dateError) return;
    
    // console.log(this.npsForm);
    // console.log(this.npsForm.get('member').errors?.memberNonExistError)
    this.nonExistEmployees$ = this.service.checkNonExistMemberByVisa(this.npsForm.controls.member.value);
    // this.nonExistEmployees$.subscribe(r => {
    //   console.log(r);
    // })


    if (this.npsForm.invalid) return;
    // console.log(this.npsForm);
    

    

    this.pj = new Project(
      this.pjId,
      this.npsForm.controls.group.value,
      this.npsForm.controls.pjNum.value,
      this.npsForm.controls.name.value,
      this.npsForm.controls.customer.value,
      this.npsForm.controls.status.value,
      this.npsForm.controls.startDate.value,
      this.npsForm.controls.endDate.value?.length == 0? null : this.npsForm.controls.endDate.value,
      this.pjVersion,
    );

    if (this.nowInEPS == false) delete this.pj.id;

    console.log(this.pj);

    if (this.nowInEPS == false) this.addProject();
    else this.editProject();

  }

  addProject() {
    if (this.npsForm.invalid) return;
    this.service.postProject(this.pj, this.npsForm.get('member').value).subscribe(
      response => this.router.navigate(["/"])
    );  
  }

  editProject() {
    if (this.npsForm.invalid) return;
    this.service.putProject(this.pj, this.npsForm.get('member').value).subscribe(
      response => this.router.navigate(["/"])
    );
  }

  setSubmittedFalse() {
    this.isSubmitted = false;
  }
}
