export interface ReferralDate {
  uuid: string; // must be unique so that we can display uniquely
  from?: any;
  to?: any; // moment string
  days?: number; // moment string
}
