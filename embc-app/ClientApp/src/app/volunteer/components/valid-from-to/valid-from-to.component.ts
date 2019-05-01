import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import range from 'lodash/range';
import { ReferralDate } from 'src/app/core/models/referral-date';
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

  wrd: ReferralDate; // Working Referral Date

  constructor() { }

  ngOnInit() {
    // initialize the dates passed to this component
    this.initializeDates(this.referralDate);
  }

  initializeDates(referralDate: ReferralDate): void {

    // if there is no identifier this component is useless.
    if (referralDate.identifier) {
      // generate the referral date
      this.wrd = this.generateReferralDate(referralDate);
    }
  }

  emitReferralDate(referralDate: ReferralDate): void {
    // After all changes are made
    this.dateStub.emit(referralDate);
  }

  generateReferralDate(referralDate: ReferralDate): ReferralDate {
    if (!referralDate.from) {
      // if no from date then datestamp to now
      referralDate.from = moment();
    }
    if (referralDate.days === null || referralDate.days === undefined) {
      // if the days are null then we instantiate to 1 so we can math
      referralDate.days = 1;
    }
    // set the to date
    referralDate.to = moment().add(referralDate.days, 'days');
    // return the changes
    return referralDate;
  }
}
