import { Component, OnInit } from '@angular/core';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { Router } from '@angular/router';
import { Registration } from 'src/app/core/models';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';

@Component({
  selector: 'app-referral-maker',
  templateUrl: './referral-maker.component.html',
  styleUrls: ['./referral-maker.component.scss']
})
export class ReferralMakerComponent implements OnInit {

  //
  // UNDER CONSTRUCTION
  //

  // TODO: need Registration object (for evacuee names) + purchaser name
  // TODO: retrieve incidentTask (for start date/time) if not already attached to Registration object

  registration: Registration = null;
  path: string; // for relative routing
  selectedPurchaser = 'null';

  constructor(
    private router: Router,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe(p => this.path = p);

    // TODO: get id from route parameters instead

    // get lookup key and load registration data
    const key = this.uniqueKeyService.getKey();
    // ensure we have a lookup key
    if (key) {
      this.registrationService.getRegistrationById(key)
        .subscribe(r => {
          // ensure we have an ESS File Number
          if (!r.essFileNumber) {
            // send them back to their home page
            this.router.navigate([`/${this.path}`]);
          } else {
            // save the registration object
            this.registration = r;
          }
        });
    } else {
      // send them back to their home page
      this.router.navigate([`/${this.path}`]);
    }
  }

  showFullProfile() {
    // TODO: replace confirm with a better popup
    if (confirm('By clicking continue you acknowledge that all changes to this information will be collected, audited, and your administrator may contact you about them.')) {
      // save the key for lookup
      this.uniqueKeyService.setKey(this.registration.id);
      this.router.navigate([`/${this.path}/registration/summary/full`]);
    }
  }

  purchaserChange(value: string) {
    // if (value !== 'null') {
    console.log('value =', value);
    // }
  }

  addReferrals() {
    // TODO: route to new page, deal with data, etc
    console.log('add referrrals...');
  }

}
