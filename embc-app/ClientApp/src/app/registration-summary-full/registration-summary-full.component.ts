import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { Registration, Address, isBcAddress } from 'src/app/core/models';
import { GENDER_OPTIONS, INSURANCE_OPTIONS } from '../constants';
import { UniqueKeyService } from '../core/services/unique-key.service';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-registration-summary-full',
  templateUrl: './registration-summary-full.component.html',
  styleUrls: ['./registration-summary-full.component.scss']
})
export class RegistrationSummaryFullComponent implements OnInit {

  // local copy of the application state
  registration: Registration = null;
  path: string = null; // for relative routing
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // get the path for routing
    this.authService.path.subscribe(p => this.path = p);

    // get URL param
    const reason = this.route.snapshot.paramMap.get('reason');

    // get lookup key and load registration data
    const key = this.uniqueKeyService.getKey();
    if (key) {
      this.registrationService.getRegistrationById(key, reason)
        .subscribe(r => {
          this.loading = false;
          if (!r.id || !r.essFileNumber) {
            console.log('ERROR - invalid registration object = ', r);
            // done with the key. It was useless. Clear the reference key.
            this.uniqueKeyService.clearKey();
            this.goHome();
          } else {
            // store the registration object
            this.registration = r;
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

  isBcAddress(address: Address): boolean {
    return isBcAddress(address);
  }

  genderOption(key: string) {
    const option = GENDER_OPTIONS.find(item => item.key === key);
    return option ? option.value : null;
  }

  insuranceOption(key: string) {
    const option = INSURANCE_OPTIONS.find(item => item.key === key);
    return option ? option.value : null;
  }

  yesNo(value: boolean): string {
    if (value === true) {
      return 'Yes';
    } else if (value === false) {
      return 'No';
    } else {
      return '';
    }
  }

  routeTo() {
    // save the key for lookup. This only ever links to the editor
    this.uniqueKeyService.setKey(this.registration.id);
    this.router.navigate([`/${this.path}/registration`]);
  }

}
