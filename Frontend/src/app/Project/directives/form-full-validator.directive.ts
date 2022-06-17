import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const formFullValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const pjNum = control.get("pjNum");
  const name = control.get("name");
  const customer = control.get("customer");
  const group = control.get("group");
  const status = control.get("status");
  const startDate = control.get("startDate");

  // console.log(pjNum.valid, name.valid, customer.valid);

  return pjNum &&
    name &&
    customer &&
    group &&
    status &&
    startDate &&
    // pjNum.valid &&
    pjNum.value?.length > 0 &&
    name.valid &&
    customer.valid &&
    group.valid &&
    status.valid &&
    startDate.valid
    ? null
    : { formFullError: true };
};

// <div    *ngIf="heroForm.errors?.['identityRevealed'] && (heroForm.touched || heroForm.dirty)"
//         class="cross-validation-error-message alert alert-danger">
//             Name cannot match alter ego.
// </div>

// https://angular.io/guide/form-validation#cross-field-validation
