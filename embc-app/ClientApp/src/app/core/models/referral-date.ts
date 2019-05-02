export interface ReferralDate {
  uuid: string; // must be unique so that we can display uniquely
  from?: Date;
  to?: Date;
  days?: number;
}
export interface ReferralDateForm {
  uuid: string; // must be unique so that we can display uniquely
  from?: YearMonthDay;
  to?: YearMonthDay; // moment string
  days?: number; // moment string
}

export interface YearMonthDay {
  year: number;
  month: number;
  day: number;
}
