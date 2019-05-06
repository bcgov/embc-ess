export interface ReferralDate {
  from: Date;
  to?: Date; // generated
  days?: number; // comes with a default in the components
}
export interface ReferralDateForm {
  fromDate?: YearMonthDay;
  toDate?: YearMonthDay;
  fromTime?: HourMinute;
  toTime?: HourMinute;
  days?: number;
}

export interface YearMonthDay {
  year: number;
  month: number;
  day: number;
}
export interface HourMinute {
  hour: number;
  minute: number;
}
