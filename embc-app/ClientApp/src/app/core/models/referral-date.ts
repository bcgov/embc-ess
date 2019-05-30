export interface ReferralDate {
  from: Date;
  to?: Date; // optional if 'days' is set
  days?: number; // optional if 'to' is set
}

export interface ReferralDateForm {
  fromDate?: string;
  toDate?: string;
  fromTime?: string;
  toTime?: string;
  days?: number;
}
