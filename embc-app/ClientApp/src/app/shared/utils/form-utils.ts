import { FormArray, FormGroup } from '@angular/forms';

export function clearFormArray(formArray: FormArray): void {
  while (formArray && formArray.length !== 0) {
    formArray.removeAt(0);
  }
}

export function invalidField(parent: FormGroup, field: string, submitted = false): boolean {
  return parent.get(field).invalid && submitted;
}

export function hasErrors(parent: FormGroup, field: string, errorCodes = ['required']): boolean {
  const c = parent.get(field);
  return (c && errorCodes.some(error => c.hasError(error)));
}
