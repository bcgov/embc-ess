import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { RegistrationService } from 'src/app/core/services/registration.service';
import { Registration, Address, isBcAddress, User, Volunteer } from 'src/app/core/models';
import { GENDER_OPTIONS, INSURANCE_OPTIONS } from '../constants';

@Component({
  selector: 'app-registration-summary-full',
  templateUrl: './registration-summary-full.component.html',
  styleUrls: ['./registration-summary-full.component.scss']
})
export class RegistrationSummaryFullComponent implements OnInit {

  // local copy of the application state
  registration: Registration;

  constructor(
    private route: ActivatedRoute,
    private registrationService: RegistrationService,
  ) { }

  ngOnInit() {
    // if there are route params we should grab them
    if (this.route.snapshot.params.id) {
      this.registrationService.getRegistrationById(this.route.snapshot.params.id)
        .subscribe(r => {
          // Save the registration into the
          this.registration = r;
        });
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
  // routeToEditor() {
  //   this.router.navigate(['../register-evacuee/fill/' + this.registration.id]);// todo: make this go to the edit page
  // }
  yesNo(value: boolean) {
    if (value === true) {
      return 'Yes';
    } else if (value === false) {
      return 'No';
    } else {
      return '';
    }
  }
}
