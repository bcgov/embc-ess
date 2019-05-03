import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import range from 'lodash/range';
import { ReferralDate, ReferralDateForm, YearMonthDay, HourMinute } from 'src/app/core/models/referral-date';
import * as moment from 'moment';

@Component({
  selector: 'app-valid-from-to',
  templateUrl: './valid-from-to.component.html',
  styleUrls: ['./valid-from-to.component.scss']
})
export class ValidFromToComponent implements OnInit {
  @Input() referralDate: ReferralDate;
  @Output() dateStub = new EventEmitter<ReferralDate>();
  days = range(1, 6); // [1,2,3,4,5]

  displayDate: string;
  displayTime: string;

  wrdForm: ReferralDateForm; // The form elements (different data)

  constructor() { }

  ngOnInit() {
    // initialize the dates passed to this component
    // if there is no identifier this component is useless.
    if (this.referralDate.uuid) {
      // generate the referral date
      this.wrdForm = this.convertReferralDateToReferralDateForm(this.referralDate);
    }
    this.updateDisplay();
  }

  emitReferralDate(): void {
    // After all changes are made
    this.dateStub.emit(this.convertReferralDateFormToReferralDate(this.wrdForm));
  }

  calculate(w: ReferralDateForm): ReferralDateForm {
    // calculate to date
    // set the hours and minutes on the to date to whatever the user picked
    const tD = this.convertYmdToMoment(w.fromDate).add(w.days, 'd')
      .hours(this.convertHmToMoment(w.fromTime).hours())
      .minutes(this.convertHmToMoment(w.fromTime).minutes());
    // tD = tD.hours()

    // set a date based on whatever is found in the global
    w.toDate = this.convertMomentToYmd(tD);
    w.toTime = this.convertMomentToHm(tD);
    // save the global
    return w;
  }

  updateDisplay() {
    const w = this.wrdForm;
    this.wrdForm = this.calculate(w);
    this.displayDate = this.convertYmdToMoment(w.toDate).format('YYYY-MM-DD');
    this.displayTime = this.convertHmToMoment(w.toTime).format('h:mm a');
    this.emitReferralDate();
  }

  convertReferralDateToReferralDateForm(referralDate: ReferralDate): ReferralDateForm {
    // default number of days on the form
    const d = referralDate.days || 1;
    // return the changes
    return {
      uuid: referralDate.uuid || null,
      days: referralDate.days || d, // this is the form default
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
      uuid: referralDateForm.uuid,
      days: referralDateForm.days, // this is the form default
      to: moment()
        .set('year', tD.get('year'))
        .set('month', tD.get('month'))
        .set('day', tD.get('day'))
        .set('hour', tT.get('hour'))
        .set('minute', tT.get('minute'))
        .toDate(),
      from: moment()
        .set('year', fD.get('year'))
        .set('month', fD.get('month'))
        .set('day', fD.get('day'))
        .set('hour', fT.get('hour'))
        .set('minute', fT.get('minute'))
        .toDate(),
    };
  }

  // ****************************helpers
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
