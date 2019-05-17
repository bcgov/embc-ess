import { Component, OnInit } from '@angular/core';

import { ReferralService } from '../core/services/referral.service';
import { ListResult, Referral } from '../core/models';


@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {

  referrals: ListResult<Referral>;
  referralsModified: Referral[];

  constructor(
    private referralService: ReferralService
  ) { }
  ngOnInit() {
    const id = '100035';
    this.referralService.getCleanReferrals(id, true)
      .subscribe(r => {
        this.referrals = r;
        this.referralsModified = r.data.map(d => {
          d.supplier.name = d.supplier.name + '!';
          return d;
        });
      });
  }

}
