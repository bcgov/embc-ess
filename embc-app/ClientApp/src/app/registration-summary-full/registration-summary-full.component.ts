import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { RegistrationService } from 'src/app/core/services/registration.service';
import { Registration, Address, isBcAddress, User, Volunteer } from 'src/app/core/models';
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
  registration: Registration;
  path: string;

  constructor(
    private router: Router,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // get the path for routing
    this.authService.path.subscribe(p => this.path = p);
    // get the key for lookup
    const key = this.uniqueKeyService.getKey();
    if (key) {
      this.registrationService.getRegistrationById(key)
        .subscribe(r => {
          // if there is nothing useful returned route somewhere else.
          if (!r.essFileNumber) {
            // send them back to their home page
            this.router.navigate([`/${this.path}`]);
          } else {
            // Save the registration into the
            this.registration = r;
          }
        });
    } else {
      // send them back to their home page
      this.router.navigate([`/${this.path}`]);
    }
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
  yesNo(value: boolean) {
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
