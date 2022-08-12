import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const emptyOrSpacesValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {

  let str: string = control.value == null? "" : control.value;
  let okay: boolean = false;

  for (let i = 0; i < str.length; ++i) {
    if (str[i] != ' ') {
      okay = true;
      break;
    }
  }

  return okay == false
          ? { allSpacesError: true }
          : null;
  
};
