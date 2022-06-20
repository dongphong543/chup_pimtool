import {
  AbstractControl,
  AsyncValidatorFn,
  ValidationErrors,
} from "@angular/forms";
import { Observable, of } from "rxjs";
import { map } from "rxjs/operators";

export function existPjNumValidator(service): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    const pjNum = control.value;
    if (pjNum.length > 4) return of(null);

    return service
      .checkProjectByPjNum(parseInt(pjNum))
      .pipe(map((res) => (res == true ? { existPjNumError: true } : null)))
  };
}