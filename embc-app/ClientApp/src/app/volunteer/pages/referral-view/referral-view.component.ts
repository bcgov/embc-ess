import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { ReferralService } from 'src/app/core/services/referral.service';
import { Referral } from 'src/app/core/models';

@Component({
  selector: 'app-referral-view',
  templateUrl: './referral-view.component.html',
  styleUrls: ['./referral-view.component.scss']
})
export class ReferralViewComponent implements OnInit {

  path: string = null; // for relative routing
  registrationId: string = null;
  referralId: string = null;
  referral: Referral = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private referralService: ReferralService,
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe(p => this.path = p);

    // get URL params
    this.registrationId = this.route.snapshot.paramMap.get('regId');
    this.referralId = this.route.snapshot.paramMap.get('refId');

    if (!this.registrationId || !this.referralId) {
      // error - return to summary page
      this.uniqueKeyService.setKey(this.registrationId);
      this.router.navigate([`/${this.path}/registration/summary`]);
    }

    // get referral data
    this.referralService.getReferralById(this.registrationId, this.referralId)
      .subscribe(r => {
        if (!r.id) {
          // error - send them back to their home page
          this.router.navigate([`/${this.path}`]);
        } else {
          this.referral = r;
        }
      }, err => {
        alert(err); // TODO: don't leave this here
        // error - send them back to their home page
        this.router.navigate([`/${this.path}`]);
      });
  }

  back() {
    // navigate back to summary page
    this.uniqueKeyService.setKey(this.registrationId);
    this.router.navigate([`/${this.path}/registration/summary`]);
  }

  deactivateReferral() {
    // TODO
  }

}
