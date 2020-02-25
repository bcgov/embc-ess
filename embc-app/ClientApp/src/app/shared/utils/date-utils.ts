import * as moment from 'moment';

export function ensureMoment(dateTime: any): moment.Moment {
  if (!moment.isMoment(dateTime)) {
    return moment(dateTime);
  }
  return dateTime;
}

/**
 * Returns the number of days between two dates.
 * Will return 1 (not 0) if you use the same date as `from` and `to`.
 *
 * @param from The start date
 * @param to The end date
 */
export function numberOfDays(from: Date, to: Date): number {
  const a = moment(from);
  const b = moment(to);
  return b.diff(a, 'days') + 1;
}

/** 
 * Normalizes a date by removing timezone offset. 
 * Fixes issues with JS adding unwanted time zone info to dates.
 * Returns a Date object with normalized time zone information.
 * @param date The date to normalize
 */
export function normalizeDate(date: Date): Date {
  if (date != null) {
    // Not sure why, but sometimes the date properties on objects are strings at run time.
    date = new Date(date);
    date.setMinutes(date.getMinutes() + date.getTimezoneOffset());
  }

  return date;
}

/**
 * Checks if the provided string is a valid date.
 * Returns a boolean indicating if the string is a valid date or not.
 * @param date The date string to validate
 */
export function dateStringIsValid(date: string): boolean {
  return moment(date).isValid();
}