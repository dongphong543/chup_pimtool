import {
    AbstractControl,
    AsyncValidatorFn,
    ValidationErrors,
  } from "@angular/forms";
  import { Observable, of } from "rxjs";
  import { first, map } from "rxjs/operators";
  
  export function memberValidator(service): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      const mems = control.value;
    //   console.log(typeof(mems))

    //   if (genMemberList(mems).length > 0) return of(null);
  
    //   service
    //     .checkNonExistMemberByVisa(mems)
    //     .subscribe(res => console.log(res))

    // return of({ memberNonExistError: true });

      return service
        .checkNonExistMemberByVisa(mems.toString())
        .pipe(map((res: any[]) => (res.length > 0 ? { memberNonExistError: true } : null)))
        .pipe(first())
    };
  }

//   function genMemberList(mems: string) {
//     if (mems != null && mems.length > 0) {
//       var members: string[] = this.npsForm.controls.member.value.split(",");
//       for (let i = 0; i < members.length; ++i) {
//         members[i] = members[i].trim();
//       }
//     } 
    
//     else {
//       members = [];
//     }

//     return members;
//   }