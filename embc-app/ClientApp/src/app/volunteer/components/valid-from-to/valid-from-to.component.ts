import { Component, EventEmitter, Input, Output, OnInit, OnChanges } from '@angular/core';
import range from 'lodash/range';
import { ReferralDate, ReferralDateForm, YearMonthDay, HourMinute } from 'src/app/core/models/referral-date';
import * as moment from 'moment';

@Component({
  selector: 'app-valid-from-to',
  templateUrl: './valid-from-to.component.html',
  styleUrls: ['./valid-from-to.component.scss']
})
export class ValidFromToComponent implements OnInit, OnChanges {
  @Input() referralDate: ReferralDate;
  // tslint:disable-next-line: no-inferrable-types
  @Input() editMode?: boolean = true; // false is read only fields
  @Input() id?: string = 'generic';
  @Output() referralDateChange = new EventEmitter<ReferralDate>();

  days = range(1, 15); // [1..14]
  defaultDays = 1; // the default amount for the component to use as a duration
  displayDate: string;
  displayTime: string;
  readOnlyFromDate: string;
  readOnlyFromTime: string;

  // invalid date or time
  validDate = true; // begin with the assumption of validity
  validTime = true;

  wrdForm: ReferralDateForm; // The form elements (different data)

  constructor() { }

  ngOnInit() {
    // generate the referral date
    this.wrdForm = this.convertReferralDateToReferralDateForm(this.handleMissingInputs(this.referralDate));

    this.updateDisplay();
  }
  ngOnChanges(changes) {
    // this reprocesses the input if the parent changes it
    this.wrdForm = this.convertReferralDateToReferralDateForm(this.handleMissingInputs(this.referralDate));
  }
  emitReferralDate(): void {
    // check validity of input
    if (this.validDate && this.validTime) {
      // After all changes are made
      this.referralDateChange.emit(this.convertReferralDateFormToReferralDate(this.wrdForm));
    } else {
      // TODO: handle invalid inputs
      this.referralDateChange.emit({ from: null });
    }
  }
  handleMissingInputs(r: ReferralDate): ReferralDate {
    // this function determines what to calculate from missing information
    // if there is a from and to calcuate days
    if (false) {

    } else if (r.from && !r.days && r.to) {
      // calculate the days between dates
      const d1 = moment(r.from);
      const d2 = moment(r.to);
      r.days = d1.diff(d2, 'days');
      return r;
    } else if (!r.from && r.days && !r.to) {
      // we caculate from today's date using the duration included ()
      r.from = new Date();
      return r;
    }
    // } else if (r.from && r.days && r.to) {
    //   // we have all of the info so we simply return it (default)
    // } else if (r.from && r.days && !r.to) {
    //   // calculate will work with this much info (default)
    // } else if (r.from && !r.days && !r.to) {
    //   // we caclulate based on the the start date and default date value (default)
    // } else if (!r.from && r.days && r.to) {
    //   // something has gone wrong for date is mandatory. use default dates. (default)
    // } else if (!r.from && !r.days && r.to) {
    //   // this is an error we should calculate from defaults (default)
    // } else if (!r.from && !r.days && !r.to) {
    //   // use defaults
    // }
    return r;
  }
  calculate(w: ReferralDateForm): ReferralDateForm {
    // calculate to date
    // set the hours and minutes on the to date to whatever the user picked
    const tD = this.convertYmdToMoment(w.fromDate).add(w.days, 'days')
      .hours(this.convertHmToMoment(w.fromTime).hours())
      .minutes(this.convertHmToMoment(w.fromTime).minutes());

    // set a date based on whatever is found in the global
    w.toDate = this.convertMomentToYmd(tD);
    w.toTime = this.convertMomentToHm(tD);
    // save the global
    return w;
  }

  updateDisplay() {
    const w = this.wrdForm;

    this.validateInputs();
    if (this.validDate && this.validTime) {
      // If the dates are valid then we calculate.
      this.wrdForm = this.calculate(w);
      this.displayDate = this.convertYmdToMoment(w.toDate).format('YYYY-MM-DD');
      this.displayTime = this.convertHmToMoment(w.toTime).format('h:mm a');
      if (this.editMode === false) {
        this.readOnlyFromDate = this.convertYmdToMoment(w.fromDate).format('YYYY-MM-DD');
        this.readOnlyFromTime = this.convertHmToMoment(w.fromTime).format('h:mm a');
      }
    }
    // emit referral date (whether valid or not)
    this.emitReferralDate();
  }

  convertReferralDateToReferralDateForm(referralDate: ReferralDate): ReferralDateForm {
    // default number of days on the form
    // return the changes
    return {
      days: referralDate.days || this.defaultDays, // this is the form default
      fromDate: this.convertMomentToYmd(moment(referralDate.from)),
      fromTime: this.convertMomentToHm(moment(referralDate.from)),
      toDate: null, // these should be calculated
      toTime: null, // these should be calculated
    };
  }

  convertReferralDateFormToReferralDate(referralDateForm: ReferralDateForm): ReferralDate {
    // collect the dates and times and convert them
    const fD: moment.Moment = this.convertYmdToMoment(referralDateForm.fromDate);
    const tD: moment.Moment = this.convertYmdToMoment(referralDateForm.toDate);
    const fT: moment.Moment = this.convertHmToMoment(referralDateForm.fromTime);
    const tT: moment.Moment = this.convertHmToMoment(referralDateForm.toTime);

    // return the changes
    return {
      days: referralDateForm.days, // this is the form default
      to: moment()
        .set('year', tD.get('year'))
        .set('month', tD.get('month'))
        .set('date', tD.get('date'))
        .set('hour', tT.get('hour'))
        .set('minute', tT.get('minute'))
        .toDate(),
      from: moment()
        .set('year', fD.get('year'))
        .set('month', fD.get('month'))
        .set('date', fD.get('date'))
        .set('hour', fT.get('hour'))
        .set('minute', fT.get('minute'))
        .toDate(),
    };
  }

  validateInputs() {
    // check the to and from for a valid range sets the validDate and validTime flags
    this.wrdForm.fromDate && Date.parse(`${this.wrdForm.fromDate.year}-${this.wrdForm.fromDate.month}-${this.wrdForm.fromDate.day}`) ? this.validDate = true : this.validDate = false;
    // use today as a start to evaluate whether or not the time is valid
    const d = new Date();
    this.wrdForm.fromTime && Date.parse(`${d.getFullYear}-${d.getMonth()}-${d.getDate()} ${this.wrdForm.fromTime.hour}:${this.wrdForm.fromTime.minute}`) ? this.validTime = true : this.validTime = false;
  }

  convertMomentToYmd(date: moment.Moment): YearMonthDay {
    // convert a moment to a year month day object
    return {
      year: date.year(),
      month: date.month() + 1,
      day: date.date(),
    };
  }
  convertYmdToMoment(date: YearMonthDay): moment.Moment {
    // convert a ymd to a moment
    return moment()
      .year(date.year)
      .month(date.month - 1)
      .date(date.day);
  }
  convertMomentToHm(time: moment.Moment): HourMinute {
    return {
      hour: time.hour(),
      minute: time.minute(),
    };
  }
  convertHmToMoment(time: HourMinute): moment.Moment {
    // convert a hour minute to
    return moment()
      .hour(time.hour)
      .minute(time.minute);
  }
}
