import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { takeWhile, map } from 'rxjs/operators';

import { RegistrationService } from 'src/app/core/services/registration.service';
import { AppState } from 'src/app/store';
import { Registration, Address, isBcAddress, User, Volunteer } from 'src/app/core/models';
import { normalize } from 'src/app/shared/utils';
import { GENDER_OPTIONS, INSURANCE_OPTIONS } from '../constants';

@Component({
  selector: 'app-evacuee-summary-page',
  templateUrl: './evacuee-summary-page.component.html',
  styleUrls: ['./evacuee-summary-page.component.scss']
})
export class EvacueeSummaryPageComponent implements OnInit {

  // local copy of the application state
  registration: Registration;

  constructor(
    private router: Router,
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
  routeToEditor() {
    this.router.navigate(['/']);// todo: make this go to the edit page
  }
}
