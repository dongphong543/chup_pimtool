import {
  AbstractControl,
  AsyncValidatorFn,
  ValidationErrors,
} from "@angular/forms";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
// import { PIMService } from "../components/pim.service";

export function existPjNumValidator(service): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    const pjNum = control.value;
    if (pjNum.length > 4) return of(null);
    // console.log(pjNum);

    // service.getProjectByPjNum(parseInt(pjNum)).subscribe(r => console.log(1));
    return service
      .getProjectByPjNum(parseInt(pjNum))
      .pipe(catchError((error) => {
        if (error.status == "404") {console.log("caught"); return of(undefined);}
        }), map((res) => (res ? { existPjNumError: true } : null)), )
      

    // return of({existPjNumError: true})
  };
}

// // <div    *ngIf="heroForm.errors?.['identityRevealed'] && (heroForm.touched || heroForm.dirty)"
// //         class="cross-validation-error-message alert alert-danger">
// //             Name cannot match alter ego.
// // </div>

// // https://angular.io/guide/form-validation#cross-field-validation
