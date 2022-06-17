import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const dateValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const startDate = control.get("startDate");
  const endDate = control.get("endDate");

  if (endDate.value == null) return null;
  return startDate &&
    endDate &&
    new Date(startDate.value) > new Date(endDate.value)
    ? { dateError: true }
    : null;
};

// <div    *ngIf="heroForm.errors?.['identityRevealed'] && (heroForm.touched || heroForm.dirty)"
//         class="cross-validation-error-message alert alert-danger">
//             Name cannot match alter ego.
// </div>

// https://angular.io/guide/form-validation#cross-field-validation
