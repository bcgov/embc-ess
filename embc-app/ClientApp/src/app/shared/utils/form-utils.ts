import { FormArray, FormGroup } from '@angular/forms';

export function clearFormArray(formArray: FormArray): void {
  while (formArray && formArray.length !== 0) {
    formArray.removeAt(0);
  }
}

export function invalidField(field: string, parent: FormGroup, submitted = false): boolean {
  const c = parent.get(field);
  return c && c.invalid && submitted;
}

export function hasErrors(field: string, parent: FormGroup, errorCodes = ['required']): boolean {
  const c = parent.get(field);
  return (c && errorCodes.some(error => c.hasError(error)));
}
