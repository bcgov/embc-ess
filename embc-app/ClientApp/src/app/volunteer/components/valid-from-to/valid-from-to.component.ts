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
  model;

  wrdForm: ReferralDateForm; // The form elements (different data)

  constructor() { }

  ngOnInit() {
    // initialize the dates passed to this component
    // if there is no identifier this component is useless.
    if (this.referralDate.uuid) {
      // generate the referral date
      this.wrdForm = this.convertReferralDateToReferralDateForm(this.referralDate);
    }
  }

  emitReferralDate(): void {
    // After all changes are made
    this.dateStub.emit(this.convertReferralDateFormToReferralDate(this.wrdForm));
  }

  convertReferralDateToReferralDateForm(referralDate: ReferralDate): ReferralDateForm {
    // default number of days on the form
    const d = referralDate.days || 1;
    // return the changes
    return {
      uuid: referralDate.uuid || null,
      days: referralDate.days || d, // this is the form default
      fromDate: this.convertMomentToYmd(moment(referralDate.from))
        || this.convertMomentToYmd(moment()),
      toDate: this.convertMomentToYmd(moment(referralDate.from).add(d, 'd')),
      fromTime: this.convertMomentToHm(moment(referralDate.from))
        || this.convertMomentToHm(moment()),
      toTime: this.convertMomentToHm(moment(referralDate.from).add(d, 'd')),
    };
  }

  convertReferralDateFormToReferralDate(referralDateForm: ReferralDateForm): ReferralDate {
    // collect the dates and times and convert them
    const tD: moment.Moment = this.convertYmdToMoment(referralDateForm.toDate);
    const tT: moment.Moment = this.convertHmToMoment(referralDateForm.toTime);
    const fD: moment.Moment = this.convertYmdToMoment(referralDateForm.fromDate);
    const fT: moment.Moment = this.convertHmToMoment(referralDateForm.fromTime);

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
      month: date.month(),
      day: date.day(),
    };
  }
  convertYmdToMoment(date: YearMonthDay): moment.Moment {
    // convert a ymd to a moment
    return moment()
      .year(date.year)
      .month(date.month)
      .day(date.day)
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
