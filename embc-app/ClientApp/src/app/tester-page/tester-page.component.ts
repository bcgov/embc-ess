import { Component, OnInit } from '@angular/core';
import { ReferralService } from '../core/services/referral.service';
import { ListResult, Referral, ReferralPost, ReferralPostItem } from '../core/models';

@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {
  constructor(
  ) { }

  ngOnInit() { }

}
