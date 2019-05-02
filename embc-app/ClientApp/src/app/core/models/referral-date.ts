export interface ReferralDate {
  uuid: string; // must be unique so that we can display uniquely
  from?: Date;
  to?: Date;
  days?: number;
}
export interface ReferralDateForm {
  uuid: string; // must be unique so that we can display uniquely
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
