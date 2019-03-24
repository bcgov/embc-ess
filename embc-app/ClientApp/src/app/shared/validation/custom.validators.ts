
import { AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import * as moment from 'moment';

function isEmptyInputValue(value: any): boolean {
  // we don't check for string here so it also works with arrays
  return value == null || value.length === 0;
}

const PHONE_REGEXP = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
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

  static date(format = "MM/DD/YYYY"): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      const val = moment(c.value, format, true);
      if (!val.isValid()) {
        return { date: true };
      }
      return null;
    };
  }

  static dateIsBefore(targetDate = new Date(), format = "MM/DD/YYYY"): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      const val = moment(c.value, format, true);
      if (val.isAfter(targetDate)) {
        return { dateIsBefore: true };
      }
      return null;
    };
  }

  static isAnAdult(ageLimit = 18, format = "MM/DD/YYYY"): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
      const val = moment(c.value, format, true);
      const now = moment();
      if (now.diff(val, 'years') < ageLimit) {
        return { dateIsBefore: true };
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
    return PHONE_REGEXP.test(c.value) ? null : { phone: true };
  }

  static postalCodeCanada(c: AbstractControl): ValidationErrors | null {
    if (isEmptyInputValue(c.value)) {
      return null;  // don't validate empty values to allow optional controls
    }
    return POSTALCODE_REGEXP.test(c.value) ? null : { postalCodeCanada: true };
  }

}
