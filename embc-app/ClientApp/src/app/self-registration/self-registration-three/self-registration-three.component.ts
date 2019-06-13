import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { takeWhile } from 'rxjs/operators';

import { AppState } from 'src/app/store';
import * as RegistrationActions from 'src/app/store/registration/registration.actions';
import { INSURANCE_OPTIONS, GENDER_OPTIONS } from 'src/app/constants/lookups';
import { Registration, isBcAddress, Address } from 'src/app/core/models';
import { UpdateRegistration } from 'src/app/store/registration/registration.actions';
import { RegistrationService } from 'src/app/core/services/registration.service';

@Component({
  selector: 'app-self-registration-three',
  templateUrl: './self-registration-three.component.html',
  styleUrls: ['./self-registration-three.component.scss']
})
export class SelfRegistrationThreeComponent implements OnInit, OnDestroy {

  // state needed by this FORM
  currentRegistration$ = this.store.select(state => state.registrations.currentRegistration);

  componentActive = true;
  submitting = false;
  registration: Registration | null;

  // CAPTCHA stuff
  captchaVerified = false;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private route: ActivatedRoute,
    private service: RegistrationService,
  ) { }

  ngOnInit() {
    // Update form values based on the state
    this.currentRegistration$
      .pipe(takeWhile(() => this.componentActive))
      .subscribe((registration: Registration) => {
        if (!registration) {
          // you shouldn't be here without registration data (redirect to step-1)
          this.router.navigate(['../step-1'], { relativeTo: this.route });
          return;
        }
        this.displayRegistration(registration);
      }, err => {
        console.log('error getting current registration =', err);
      });
  }

  ngOnDestroy(): void {
    this.componentActive = false;
  }

  insuranceOption(key: string) {
    const option = INSURANCE_OPTIONS.find(item => item.key === key);
    return option ? option.value : null;
  }

  genderOption(key: string) {
    const option = GENDER_OPTIONS.find(item => item.key === key);
    return option ? option.value : null;
  }

  isBcAddress(address: Address): boolean {
    return isBcAddress(address);
  }

  displayRegistration(registration: Registration | null): void {
    // Set the local registration property
    this.registration = registration;
  }

  submit() {
    this.submitting = true;

    const reg = this.registration;
    this.processRegistration(reg);

    // update client-side state
    this.onSave(reg);

    // push changes to backend
    this.service.createRegistration(reg)
      .subscribe((registration: Registration) => {
        this.submitting = false; // turn off submission state
        this.clearRegistration(); // prevent double submissions
        this.router.navigate(['../step-4/' + registration.essFileNumber], { relativeTo: this.route });
      }, err => {
        console.log('error creating registration = ', err);
        // do not submit anymore
        this.submitting = false; // turn off submission state
        this.clearRegistration(); // prevent double submissions
        this.router.navigate(['../error'], { relativeTo: this.route });
      });
  }

  // process registration data before submitting to the server
  private processRegistration(reg: Registration): void {
    // stamp the dates that we want to track for this record
    reg.selfRegisteredDate = new Date().toJSON();
    // by clicking submit this has to be true because submit is consent
    reg.declarationAndConsent = true;
    // NOTE The customer asked for all of these to be reversed.
    // e.g. requiresAccomodation means hasAccomodation
    // reversal is done before submitting
    reg.requiresAccommodation = !reg.requiresAccommodation;
    reg.requiresClothing = !reg.requiresClothing;
    reg.requiresFood = !reg.requiresFood;
    reg.requiresIncidentals = !reg.requiresIncidentals;
    reg.requiresTransportation = !reg.requiresTransportation;

    // replace empty strings with null values to comply with server validation
    if (reg.headOfHousehold.email === '') { reg.headOfHousehold.email = null; }
  }

  back() {
    // clear the consent checkbox if we go back to edit the information provided so far
    const registration: Registration = {
      ...this.registration,
    };
    this.onSave(registration);
    this.router.navigate(['../step-1'], { relativeTo: this.route });
  }

  onSave(registration: Registration) {
    this.store.dispatch(new UpdateRegistration({ registration }));
  }

  clearRegistration(): void {
    this.store.dispatch(new RegistrationActions.ClearCurrentRegistration());
  }

  public onValidToken(token: any) {
    console.log('Valid token received: ', token);
    this.captchaVerified = true;
  }

  public onServerError(error: any) {
    console.log('Server error: ', error);
    this.captchaVerified = true;
  }

}
