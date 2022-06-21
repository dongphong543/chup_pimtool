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
import { PIMService } from "../pim.service";
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
          [Validators.required, Validators.max(9999)],
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
              // member: ...
              status: response.status,
              startDate: response.startDate?.split('T')[0],
              endDate: response.endDate?.split('T')[0]           
              })

              this.pjId = response.id;
              this.pjVersion = response.version;

            },
          error => 
          {
            // window.location.assign("/");
            this.router.navigate(["/"]);
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

  // formFull() {
  //   if (this.npsForm.get("pjNum").value == null && this.npsForm.get("pjNum").invalid) return false;
  //   if (this.npsForm.get("name").invalid) return false;

  //   if (this.npsForm.get("customer").invalid) return false;
  //   if (this.npsForm.get("group").invalid) return false;
  //   if (this.npsForm.get("status").invalid) return false;
  //   if (this.npsForm.get("startDate").invalid) return false;

  //   return true;
  // }

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
      this.npsForm.controls.endDate.value,
      this.pjVersion
    );

    if (this.nowInEPS == false) delete this.pj.id;

    console.log(this.pj);

    if (this.nowInEPS == false) this.addProject();
    else this.editProject();

  }

  addProject() {
    if (this.npsForm.invalid) return;
    this.service.postProject(this.pj).subscribe();
    this.service.checkExistMemberByVisa(this.npsForm.controls.member.value).subscribe(
      response => {
        console.log(response)
        // this.service.postProjectEmployee(this.pj.projectNumber, response);
      }
    );

    

    // this.isSubmitted = true;
    // if (this.formFull() == false) return;
    // if (this.npsForm.get("pjNum").invalid && this.nowInEPS == false) return;

    // this.pj = new Project(
    //   null,
    //   this.npsForm.controls.group.value,
    //   this.npsForm.controls.pjNum.value,
    //   this.npsForm.controls.name.value,
    //   this.npsForm.controls.customer.value,
    //   this.npsForm.controls.status.value,
    //   this.npsForm.controls.startDate.value,
    //   this.npsForm.controls.endDate.value,
    //   0
    // );

    // delete this.pj.id;

    // console.log(this.pj);

    // check if project number exist => Not needed, since there's ERROR 500 => update: validator now

    // if (
    //   this.npsForm.controls.member.value != null &&
    //   this.npsForm.controls.member.value.length > 0
    // ) {
    //   var members: string[] = this.npsForm.controls.member.value.split(",");
    //   for (let i = 0; i < members.length; ++i) {
    //     members[i] = members[i].trim();
    //   }
    // } else {
    //   members = [];
    // }

    // var memberId: number[] = [],
    //   memberNotFoundVisa: string[] = [];

    // this.employees$.subscribe(
    //   (response) => {
    //     members.forEach((vis) => {
    //       let found: boolean = false;
    //       let pos = response.map((em) => em.visa).findIndex((em) => em == vis);

    //       if (pos != -1) {
    //         found = true;
    //         memberId.push(response[pos].id);
    //       }

    //       if (!found) {
    //         memberNotFoundVisa.push(vis);
    //       }

    //       console.log(memberId);
    //       if (this.npsForm.valid == true) {
    //         console.log(memberNotFoundVisa);
    //         this.memberNotFound$ = of(memberNotFoundVisa);
    //       }
    //     });

    //     if (memberNotFoundVisa.length == 0) {
    //       this.service.postProject(this.pj).subscribe();
    //       // window.location.assign("/");
    //     }
    //     else this.isMemberNotFound = true;
    //   }
    //   // error => console.log(error + "HEHE")
    // );
    // setTimeout(() => {}, 1500);

    // this.broadcastService.msg.asObservable().subscribe(values => {
    //   console.log(values + "TOI ROI"); // will return false if http error
    // });

    // console.log(this.pjNumError)

    // memberId.forEach(i => console.log(this.pj.id, i))
    // memberId.forEach(i => this.service.postProjectEmployee(this.pj.id, i));
  }

  editProject() {
    if (this.npsForm.invalid) return;
    this.service.putProject(this.pj).subscribe();
    
  }

  // set404MemberFalse() {
  //   // this.isMemberNotFound = false;
  //   setTimeout(() => {}, 500);
  //   return this.isMemberNotFound;
  // }



  // trying to replace by api 
  // genMemberList() {
  //   if (
  //     this.npsForm.controls.member.value != null &&
  //     this.npsForm.controls.member.value.length > 0
  //   ) {
  //     var members: string[] = this.npsForm.controls.member.value.split(",");
  //     for (let i = 0; i < members.length; ++i) {
  //       members[i] = members[i].trim();
  //     }
  //   } 
    
  //   else {
  //     members = [];
  //   }

  //   return members;
  // }

    // var memberId: number[] = [],
    //   memberNotFoundVisa: string[] = [];

    // this.employees$.subscribe(
    //   (response) => {
    //     members.forEach((vis) => {
    //       let found: boolean = false;
    //       let pos = response.map((em) => em.visa).findIndex((em) => em == vis);

    //       if (pos != -1) {
    //         found = true;
    //         memberId.push(response[pos].id);
    //       }

    //       if (!found) {
    //         memberNotFoundVisa.push(vis);
    //       }

    //       console.log(memberId);
    //       if (this.npsForm.valid == true) {
    //         console.log(memberNotFoundVisa);
    //         this.memberNotFound$ = of(memberNotFoundVisa);
    //       }
    //     });

    //     if (memberNotFoundVisa.length == 0) this.service.postProject(this.pj);
    //     else this.isMemberNotFound = true;
    //   }
    //   // error => console.log(error + "HEHE")
    // );
    // }

  // changePjNum() {
  //   this.broadcastService.msg.next("");
  // }

  setSubmittedFalse() {
    this.isSubmitted = false;
  }
}
