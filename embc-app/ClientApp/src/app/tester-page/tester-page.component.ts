import { Component, OnInit } from '@angular/core';

import { ReferralService } from '../core/services/referral.service';
import { ListResult, Referral, IncidentalsReferral } from '../core/models';


@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {

  referrals: ListResult<Referral>;
  referralsModified: Referral[];
  sampleReferral: IncidentalsReferral = {
    id: null,
    approvedItems: 'a bunch of stuff',
    essNumber: '100043',
    referralId: null,
    active: true,
    type: 'INCIDENTALS',
    subType: 'Groceries',
    purchaser: 'Percy Purchaser',
    dates: null,
    evacuees: [
      {
        id: '100035-1',
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
        id: '100035-2',
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
    supplier: null,
    comments: 'Most comments are nice. Some are not nice.',
    confirmChecked: true,
  };
  id = '100035';
  constructor(
    private referralService: ReferralService
  ) { }
  ngOnInit() {
    this.referralService.getCleanReferrals(this.id, true)
      .subscribe(r => {
        this.referrals = r;
        this.referralsModified = r.data.map(d => {
          d.supplier.name = d.supplier.name + '!';
          return d;
        });
      });
  }
  submit() {
    this.referralService.createReferrals(this.id, [this.sampleReferral]).subscribe(() => { });
  }
}
