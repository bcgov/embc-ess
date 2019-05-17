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
  @Input() editMode = true; // false means read only
  @Input() id = 'generic'; // default id if not provided
  @Output() referralDateChange = new EventEmitter<ReferralDate>();

  readonly dateMask = [/\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/]; // yyyy-mm-dd
  readonly timeMask = [/\d/, /\d/, ':', /\d/, /\d/, ' ', /[a|A|p|P]/, /[m|M]/]; // hh:mm xx

  days = range(1, 15); // [1..14]
  defaultDays = 1; // the default duration

  validDate = true; // begin with the assumption of validity
  validTime = true;

  wrdForm: ReferralDateForm; // the form elements (local data)

  constructor() { }

  ngOnInit() {
    // generate the referral date
    this.wrdForm = this.convertReferralDateToReferralDateForm(this.handleMissingInputs(this.referralDate));
    this.updateDisplay();
  }

  // this function determines what to calculate from missing information
  private handleMissingInputs(r: ReferralDate): ReferralDate {
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
    } else if (r.from && !r.days && !r.to) {
      // set Days to default and calculate the To date
      const from = moment(r.from);
      r.days = this.defaultDays;
      r.to = from.add(r.days, 'days').toDate();
    } else {
      // set From date as today, set Days to default, and calculate the To date
      console.log('Valid-To: using defaults');
      r.from = moment().toDate();
      r.days = this.defaultDays;
      r.to = moment().add(1, 'days').toDate();
    }
    return r;
  }

  private convertReferralDateToReferralDateForm(referralDate: ReferralDate): ReferralDateForm {
    return {
      fromDate: moment(referralDate.from).format('YYYY-MM-DD'),
      fromTime: moment(referralDate.from).format('hh:mm a'),
      days: referralDate.days,
      toDate: null, // this will be calculated
      toTime: null, // this will be calculated
    } as ReferralDateForm;
  }

  updateDisplay() {
    this.validDate = this.wrdForm.fromDate && moment(this.wrdForm.fromDate, 'YYYY-MM-DD', true).isValid();
    this.validTime = this.wrdForm.fromTime && moment(this.wrdForm.fromTime, 'hh:mm a', true).isValid();

    const oldForm = this.wrdForm;

    if (this.validDate && this.validTime) {
      // if the dates are valid then re-calculate
      this.wrdForm = this.calculate(oldForm);
    }

    // emit referral date (whether valid or not)
    this.emitReferralDate();
  }

  private calculate(w: ReferralDateForm): ReferralDateForm {
    const fromDateTime = w.fromDate + ' ' + w.fromTime;
    const from = moment(fromDateTime, 'YYYY-MM-DD hh:mm a', true);

    // calculate the To date
    const to = from.add(w.days, 'days');

    // set To date and time
    w.toDate = to.format('YYYY-MM-DD');
    w.toTime = to.format('hh:mm a');

    return w;
  }

  private emitReferralDate() {
    if (this.validDate && this.validTime) {
      this.referralDateChange.emit(this.convertReferralDateFormToReferralDate(this.wrdForm));
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
