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
