import {
    AbstractControl,
    AsyncValidatorFn,
    ValidationErrors,
  } from "@angular/forms";
  import { Observable, of } from "rxjs";
  import { map } from "rxjs/operators";
  
  export function memberValidator(service): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      const mems = control.value;

    //   if (genMemberList(mems).length > 0) return of(null);
  
      return service
        .checkNonExistMemberByVisa(mems)
        .pipe(map((res) => (res == true ? { existPjNumError: true } : null)))
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