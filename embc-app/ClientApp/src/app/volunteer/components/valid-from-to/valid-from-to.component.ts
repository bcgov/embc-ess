import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { ReferralDate } from 'src/app/core/models/referral-date';
import * as moment from 'moment';
import range from 'lodash/range';

interface ReferralDateForm {
  fromDate?: string;
  toDate?: string;
  fromTime?: string;
  toTime?: string;
  days?: number;
}

@Component({
  selector: 'app-valid-from-to',
  templateUrl: './valid-from-to.component.html',
  styleUrls: ['./valid-from-to.component.scss']
})
export class ValidFromToComponent implements OnInit {
  @Input() referralDate: ReferralDate;
  @Input() id = 'generic'; // default id if not provided
  @Output() referralDateChange = new EventEmitter<ReferralDate>();

  readonly dateMask = [/\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/]; // yyyy-mm-dd
  readonly timeMask = [/\d/, /\d/, ':', /\d/, /\d/, ' ', /[a|A|p|P]/, /[m|M]/]; // hh:mm xx

  days = range(1, 15); // [1..14]
  defaultDays = 1; // the default duration

  validFromDate = true; // begin with the assumption of validity
  validFromTime = true;
  validToTime = true;

  wrdForm: ReferralDateForm; // the form elements (local data)

  constructor() { }

  ngOnInit() {
    // populate form with initial referral date
    const rd = this.handleMissingInputs(this.referralDate);
    this.wrdForm = this.convertReferralDateToReferralDateForm(rd);
  }

  // this function calculates missing information
  private handleMissingInputs(r: ReferralDate): ReferralDate {
    // if all inputs exist, return right away
    if (r.from && r.to && r.days) {
      return r;
    }

    if (r.from && r.days && !r.to) {
      // calculate the To date
      const from = moment(r.from);
      r.to = from.add(r.days, 'days').toDate();

    } else if (r.from && !r.days && r.to) {
      // calculate the Days between dates
      const from = moment(r.from);
      const to = moment(r.to);
      r.days = from.diff(to, 'days');

    } else if (!r.from && r.days && r.to) {
      // calculate the From date
      const to = moment(r.to);
      r.from = to.subtract(r.days, 'days').toDate();

    } else if (r.from && !r.days && !r.to) {
      // set Days to default and calculate the To date
      const from = moment(r.from);
      r.days = this.defaultDays;
      r.to = from.add(r.days, 'days').toDate();

    } else {
      // set From date as today, set Days to default, and calculate the To date
      console.log('Valid-From-To: using defaults!');
      r.from = moment().toDate();
      r.days = this.defaultDays;
      r.to = moment().add(r.days, 'days').toDate();
    }

    return r;
  }

  private convertReferralDateToReferralDateForm(referralDate: ReferralDate): ReferralDateForm {
    return {
      fromDate: moment(referralDate.from).format('YYYY-MM-DD'),
      fromTime: moment(referralDate.from).format('hh:mm a'),
      days: referralDate.days,
      toDate: moment(referralDate.to).format('YYYY-MM-DD'),
      toTime: moment(referralDate.to).format('hh:mm a')
    } as ReferralDateForm;
  }

  update(retainToTime: boolean = false) {
    this.validFromDate = this.wrdForm.fromDate && moment(this.wrdForm.fromDate, 'YYYY-MM-DD', true).isValid();
    this.validFromTime = this.wrdForm.fromTime && moment(this.wrdForm.fromTime, 'hh:mm a', true).isValid();
    this.validToTime = this.wrdForm.toTime && moment(this.wrdForm.toTime, 'hh:mm a', true).isValid();

    // if the dates are valid then re-calculate
    if (this.validFromDate && this.validFromTime && (this.validToTime || !retainToTime)) {
      const fromDateTime = this.wrdForm.fromDate + ' ' + this.wrdForm.fromTime;
      const from = moment(fromDateTime, 'YYYY-MM-DD hh:mm a', true);

      // calculate the To date
      const to = from.add(this.wrdForm.days, 'days');

      // set To date
      this.wrdForm.toDate = to.format('YYYY-MM-DD');

      // conditionally set To time
      if (!retainToTime) {
        this.wrdForm.toTime = to.format('hh:mm a');
        this.validToTime = true; // in case it was invalid
      }
    }

    // emit referral date (whether valid or not)
    this.emitReferralDate();
  }

  private emitReferralDate() {
    if (this.validFromDate && this.validFromTime && this.validToTime) {
      const rd = this.convertReferralDateFormToReferralDate(this.wrdForm);
      this.referralDateChange.emit(rd);
    } else {
      this.referralDateChange.emit(null);
    }
  }

  private convertReferralDateFormToReferralDate(referralDateForm: ReferralDateForm): ReferralDate {
    const fromDateTime = referralDateForm.fromDate + ' ' + referralDateForm.fromTime;
    const toDateTime = referralDateForm.toDate + ' ' + referralDateForm.toTime;

    return {
      from: moment(fromDateTime, 'YYYY-MM-DD hh:mm a', true).toDate(),
      to: moment(toDateTime, 'YYYY-MM-DD hh:mm a', true).toDate(),
      days: referralDateForm.days
    } as ReferralDate;
  }

}
