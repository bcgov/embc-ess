
import { AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import * as moment from 'moment';

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
      if (c.value && (isNaN(c.value) || c.value < min || c.value > max)) {
        return { range: true };
      }
      return null;
    };
  }

  // Validates numbers
  static number(c: AbstractControl): ValidationErrors | null {
    const NUMBER = /^-?[\d.]+(?:e-?\d+)?$/;
    if (c.value && !NUMBER.test(c.value)) {
      return { number: true };
    }
    return null;
  }

  // Validates US/Canada phone numbers
  static phone(c: AbstractControl): ValidationErrors | null {
    const PHONE = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    if (c.value && !PHONE.test(c.value)) {
      return { phone: true };
    }
    return null;
  }

  // Validates Canadian postal codes
  static postalCodeCanada(c: AbstractControl): ValidationErrors | null {
    const POSTALCODE = /^[A-Za-z][0-9][A-Za-z][ ]?[0-9][A-Za-z][0-9]$/;
    if (c.value && !POSTALCODE.test(c.value)) {
      return { postalCodeCanada: true };
    }
    return null;
  }
}
