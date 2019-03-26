import { FormArray, FormGroup } from '@angular/forms';

export function clearFormArray(formArray: FormArray): void {
  while (formArray && formArray.length !== 0) {
    formArray.removeAt(0);
  }
}

export function invalidControl(parent: FormGroup, controlName: string, errorCodes = ['required']): boolean {
  const c = parent.get(controlName);
  return (c && errorCodes.some(error => c.hasError(error)));
}
