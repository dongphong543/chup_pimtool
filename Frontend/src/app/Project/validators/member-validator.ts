import {
  AbstractControl,
  AsyncValidatorFn,
  ValidationErrors,
} from "@angular/forms";
import { Observable } from "rxjs";
import { first, map } from "rxjs/operators";

export function memberValidator(service): AsyncValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    const mems = control.value;
    return service
      .checkNonExistMemberByVisa(mems.toString())
      .pipe(
        map((res: any[]) =>
          res.length > 0 ? { memberNonExistError: true } : null
        )
      )
      .pipe(first());
  };
}
