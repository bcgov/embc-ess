import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../core/services/registration.service';
import { Router } from '@angular/router';
import { Registration } from '../core/models';
import { AuthService } from '../core/services/auth.service';
import { UniqueKeyService } from '../core/services/unique-key.service';

@Component({
  selector: 'app-registration-summary',
  templateUrl: './registration-summary.component.html',
  styleUrls: ['./registration-summary.component.scss']
})
export class RegistrationSummaryComponent implements OnInit {

  registration: Registration = null;
  path: string = null; // for relative routing
  selectedPurchaser = 'null';
  otherPurchaser: string = null;
  loading = true;

  get purchaser() {
    const otherPurchaser = this.otherPurchaser ? this.otherPurchaser.trim() : null;
    return (this.selectedPurchaser === 'other') ? otherPurchaser : (this.selectedPurchaser !== 'null') ? this.selectedPurchaser : null;
  }

  constructor(
    private router: Router,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe(p => this.path = p);

    // get lookup key and load registration data
    const key = this.uniqueKeyService.getKey();
    if (key) {
      this.registrationService.getRegistrationSummaryById(key)
        .subscribe(value => {
          this.loading = false;
          if (!value.id || !value.essFileNumber) {
            console.log('ERROR - invalid registration object = ', value);
            // done with the key. It was useless. Clear the reference key.
            this.uniqueKeyService.clearKey();
            this.goHome();
          } else {
            // store the registration object
            this.registration = value;
          }
        }, err => {
          this.loading = false;
          alert(`err = ${err}`);
          this.goHome();
        });
    } else {
      // key was not found
      this.loading = false;
      this.goHome();
    }
  }

  private goHome() {
    // send them back to their home page
    this.router.navigate([`/${this.path}`]);
  }

  showFullProfile() {
    // TODO: replace confirm with a better popup
    if (confirm('By clicking continue you acknowledge that all changes to this information will be collected, audited, and your administrator may contact you about them.')) {
      // save the key for lookup
      this.uniqueKeyService.setKey(this.registration.id);
      this.router.navigate([`/${this.path}/registration/summary/full`]);
    }
  }

  addReferrals() {
    this.router.navigate([`/${this.path}/referrals`, this.registration.id, this.purchaser]);
  }

}
