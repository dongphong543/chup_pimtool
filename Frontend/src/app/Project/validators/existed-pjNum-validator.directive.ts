import {
  AbstractControl,
  AsyncValidatorFn,
  ValidationErrors,
} from "@angular/forms";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";

export function existPjNumValidator(service): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    const pjNum = control.value;

    return service
      .checkProjectByPjNums([parseInt(pjNum)])
      .pipe(map((res) => (res == true ? { existPjNumError: true } : null)));
  };
}
