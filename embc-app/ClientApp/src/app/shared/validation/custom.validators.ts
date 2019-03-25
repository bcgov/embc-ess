
import { AbstractControl, ValidatorFn, ValidationErrors, FormGroup } from '@angular/forms';
import * as moment from 'moment';

function isEmptyInputValue(value: any): boolean {
  // we don't check for string here so it also works with arrays
  return value == null || value.length === 0;
}

const PHONE_REGEXP = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/; // spaces (and other separators) allowed in the phone number
const NO_SPACES_PHONE_REGEXP = /^([0-9]{3})([0-9]{3})([0-9]{4})$/; // Max 10 numbers, no spaces allowed
const POSTALCODE_REGEXP = /^[A-Za-z][0-9][A-Za-z][ ]?[0-9][A-Za-z][0-9]$/;

/**
 * @description
 * Provides a set of custom validators that can be used by form controls.
 *
 * A validator is a function that processes a `FormControl` or collection of
 * controls and returns an error map or null. A null map means that validation has passed.
 *
 */
export class CustomValidators {

  static date(format = 'YYYY-MM-DD'): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      const val = moment(c.value, format, true);
      if (!val.isValid()) {
        return { date: true };
      }
      return null;
    };
  }

  static dateInThePast(format = 'YYYY-MM-DD'): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      const val = moment(c.value, format, true);
      const today = moment();
      if (val.isAfter(today, 'day')) {
        return { dateInThePast: true };
      }
      return null;
    };
  }

  static isAnAdult(ageLimit = 18, format = 'YYYY-MM-DD'): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      const val = moment(c.value, format, true);
      const today = moment();
      if (today.diff(val, 'years') < ageLimit) {
        return { isAnAdult: true };
      }
      return null;
    };
  }

  static range(min: number, max: number): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (isEmptyInputValue(c.value) || isEmptyInputValue(min) || isEmptyInputValue(max)) {
        return null;  // don't validate empty values to allow optional controls
      }
      const value = parseFloat(c.value);
      return (isNaN(value) || value < min || value > max) ? { range: true } : null;
    };
  }

  static number(c: AbstractControl): ValidationErrors | null {
    if (isEmptyInputValue(c.value)) {
      return null;  // don't validate empty values to allow optional controls
    }
    const value = parseFloat(c.value);
    return isNaN(value) ? { number: true } : null;
  }

  static phone(c: AbstractControl): ValidationErrors | null {
    if (isEmptyInputValue(c.value)) {
      return null;  // don't validate empty values to allow optional controls
    }
    return NO_SPACES_PHONE_REGEXP.test(c.value) ? null : { phone: true };
  }

  static postalCodeCanada(c: AbstractControl): ValidationErrors | null {
    if (isEmptyInputValue(c.value)) {
      return null;  // don't validate empty values to allow optional controls
    }
    return POSTALCODE_REGEXP.test(c.value) ? null : { postalCodeCanada: true };
  }

  /**
   * Custom validator that checks that two fields match.
   * For example, this could be used to validate that the confirm password and password fields match.
   *
   * @param controlName The name of "source" control; i.e. "password"
   * @param matchingControlName The name of the control to match against `controlName`; i.e. "confirmPassword"
   */
  static mustMatch(controlName: string, matchingControlName: string): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      if (!(c instanceof FormGroup)) {
        return; // bail out, a FormGroup instance is required
      }
      const formGroup = c as FormGroup;
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (matchingControl.errors && !matchingControl.errors.mustMatch) {
        return; // return if another validator has already found an error on the matchingControl
      }

      // set error on matchingControl if validation fails
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mustMatch: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  }

}
