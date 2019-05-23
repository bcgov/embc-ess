import { AbstractControl } from '@angular/forms';

export function ValidateComments(control: AbstractControl) {

  let valid = null;
  if (control.value) {

    const value = control.value.toString()
      .split(/\n/) // break string into array at carriage returns
      .map(line => line.trim()); // remove whitespace. If the line is empty it is falsy so it gets filtered

    const cleanValue = value
      .filter(line => line)
      .map((line) => line.length > 250 ? line.slice(0, 250) : line) // check each element to be sure we let lines less than 251
      .slice(0, 4) // only allow 4 lines.
      .join('\n'); // combine the array into a string

    // if there are more than four lines this is invalid
    if (value.length > 4) {
      console.log('Comment has too many lines');
      valid = cleanValue;
    }
    // if there are more than 250 characters on a line this is invalid
    value.forEach(element => {
      if (element.length > 250) {
        console.log('Comment is too long');
        valid = cleanValue;
      }
    });
  }
  return valid;
}
