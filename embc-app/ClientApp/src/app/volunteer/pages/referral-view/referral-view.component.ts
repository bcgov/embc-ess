import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { ReferralService } from 'src/app/core/services/referral.service';
import {
  Referral, isAccommodationReferral, isClothingReferral,
  isFoodReferral, isIncidentalsReferral, isTransportationReferral
} from 'src/app/core/models';

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
  loading = true;
  deactivating = false;

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

    // get and verify URL params
    this.registrationId = this.route.snapshot.paramMap.get('regId');
    this.referralId = this.route.snapshot.paramMap.get('refId');

    if (!this.registrationId || !this.referralId) {
      alert(`err = invalid registration ID or referral ID`);
      this.back();
    }

    // get referral data
    this.referralService.getReferralById(this.registrationId, this.referralId)
      .subscribe(value => {
        this.loading = false;
        if (!value.id || !value.type) {
          console.log('ERROR - invalid referral object = ', value);
          this.goHome();
        } else {
          this.referral = value;
        }
      }, err => {
        this.loading = false;
        alert(`err = ${err}`);
        this.goHome();
      });
  }

  private goHome() {
    // go back to their home page
    this.router.navigate([`/${this.path}`]);
  }

  back() {
    // go back to summary page
    this.uniqueKeyService.setKey(this.registrationId);
    this.router.navigate([`/${this.path}/registration/summary`]);
  }

  deactivate() {
    this.deactivating = true;
    this.referralService.deactivateReferral(this.registrationId, this.referralId)
      .subscribe(() => {
        this.deactivating = false;
        // return to summary page
        this.back();
      }, err => {
        this.deactivating = false;
        alert(`err = ${err}`);
        this.goHome();
      });
  }

  // --------------------HELPERS-----------------------------------------
  isAccommodationReferral(referral: Referral): boolean {
    return isAccommodationReferral(referral);
  }

  isClothingReferral(referral: Referral): boolean {
    return isClothingReferral(referral);
  }

  isFoodReferral(referral: Referral): boolean {
    return isFoodReferral(referral);
  }

  isIncidentalsReferral(referral: Referral): boolean {
    return isIncidentalsReferral(referral);
  }

  isTransportationReferral(referral: Referral): boolean {
    return isTransportationReferral(referral);
  }

}
