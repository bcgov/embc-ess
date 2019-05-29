import { Component, OnInit } from '@angular/core';

import { ReferralService } from '../core/services/referral.service';
import { ListResult, Referral, IncidentalsReferral, ReferralPost, ReferralPostItem } from '../core/models';

@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {

  referrals: ListResult<Referral>;
  referralsModified: Referral[];
  sampleReferral: ReferralPostItem = {
    approvedItems: 'a bunch of stuff',
    essNumber: '100043',
    referralId: null,
    active: true,
    type: 'FOOD',
    subType: 'GROCERIES',
    purchaser: 'Percy Purchaser',
    validDates: {
      to: new Date(),
      from: new Date()
    },
    evacuees: [
      {
        id: '100043-1',
        // id: '1', // will be this
        active: null,
        personType: 'HOH',
        firstName: 'Woop',
        lastName: 'test',
        nickname: null,
        initials: '',
        gender: null,
        dob: '1988-02-29'
      },
      {
        id: '100043-2',
        // id: '2',
        active: null,
        personType: 'FMBR',
        firstName: 'Bork',
        lastName: 'test',
        nickname: null,
        initials: '',
        gender: null,
        dob: '1998-02-29'
      }
    ],
    totalAmount: 1, // NB: set to 0 if not used
    supplier: {
      id: null,
      name: 'Bork\'s Kitchen',
      city: 'Victoria',
      address: '1234 Anywhere Ave',
      province: 'BC',
      postalCode: 'v8v8v8v',
      telephone: '123456',
      fax: null,
      active: true,
    },
    comments: 'Most comments are nice. Some are not nice.',
  };
  id = '100035';
  reply;

  constructor(
    private referralService: ReferralService
  ) { }

  ngOnInit() {
    this.referralService.getCleanReferrals(this.id, true)
      .subscribe(r => {
        this.referrals = r;
        // this.referralsModified = r.data.map(d => {
        //   d.supplier.name = d.supplier.name + '!';
        //   return d;
        // });
      });
  }

  submit() {
    const x: ReferralPost = {
      confirmChecked: true,
      referrals: [this.sampleReferral],
    };
    this.reply = x;
    this.referralService.createReferrals(this.id, x).subscribe(
      value => {
        console.log(value);
      }, err => {
        console.log(err);
      }
    );
  }
}
