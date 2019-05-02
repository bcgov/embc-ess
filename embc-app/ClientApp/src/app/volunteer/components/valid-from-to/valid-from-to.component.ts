import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import range from 'lodash/range';
import { ReferralDate, ReferralDateForm, YearMonthDay } from 'src/app/core/models/referral-date';
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

  wrdForm: any; // The form elements (different data)

  constructor() { }

  ngOnInit() {
    // initialize the dates passed to this component
    this.initializeDates(this.referralDate);
  }

  initializeDates(referralDate: ReferralDate): void {
    // if there is no identifier this component is useless.
    if (referralDate.uuid) {
      // generate the referral date
      this.wrdForm = this.generateReferralDateForm(referralDate);
      // use the referral date to generate the useful form object
      console.log(this.generateReferralDateForm(referralDate));

    }
  }

  emitReferralDate(referralDate: ReferralDate): void {
    // After all changes are made
    this.dateStub.emit(referralDate);
  }

  generateReferralDateForm(referralDate: ReferralDate): ReferralDateForm {
    // return the changes
    const rd: ReferralDateForm = {
      uuid: referralDate.uuid || null,
      days: referralDate.days || 1,
      to: this.convertMomentToYmd(moment(referralDate.to))
        || this.convertMomentToYmd(moment()),
      from: this.convertMomentToYmd(moment(referralDate.from))
        || this.convertMomentToYmd(moment().add(1, 'd')),
    };
    return rd;
  }


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
    return moment().day(date.day).month(date.month).year(date.year);
  }

  onFromDateSelect(date) {
    // get date from angular bootstrap datepicker
    // {"year":2019,"month":5,"day":16}

    // the input is a date string consistent with the user's browser.
    // Using the browser we can then convert it into milliseconds
    // using moments we convert it to a consistent format
    alert(JSON.stringify(date));

    // parse the browser date into a moment value
    // alert(typeof date);

  }
}
