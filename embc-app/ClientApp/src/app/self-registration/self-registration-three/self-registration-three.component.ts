import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { combineLatest } from 'rxjs';
import { map, takeWhile } from 'rxjs/operators';

import { INSURANCE_OPTIONS, GENDER_OPTIONS } from 'src/app/constants/lookups';
import { Registration, isBcAddress, Address } from 'src/app/core/models';
import { normalize } from 'src/app/shared/utils';
import { AppState } from 'src/app/store';
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
      .subscribe(registration => {
        this.displayRegistration({ registration });
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

  displayRegistration(props: { registration: Registration | null }): void {
    // Set the local registration property
    this.registration = props.registration;
  }

  next() {
    this.submitting = true;
    // process the registration record before submission to the backend

    // by clicking submit this has to be true because submit is consent
    this.registration.declarationAndConsent = true;

    // update client-side state
    this.onSave(this.registration);

    // push changes to backend
    this.service.createRegistration(this.registration).subscribe(
      data => {
        this.submitting = false; // turn off submission state
        this.router.navigate(['../step-4/' + data.essFileNumber], { relativeTo: this.route });
      },
      err => {
        alert(err);
        // do not submit anymore
        this.submitting = false; // turn off submission state
        this.router.navigate(['../error'], { relativeTo: this.route });
      }
    );
  }

  // stamp the dates that we want to track for this record
  processData(value: Registration): void {
    value.selfRegisteredDate = new Date().toJSON();
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
}
