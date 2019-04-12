import { Component, OnDestroy, OnInit, ɵConsole } from '@angular/core';
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

  submit() {
    this.submitting = true;

    // process the registration record before submission to the backend
    this.processData(this.registration);

    // update client-side state
    this.onSave(this.registration);

    // push changes to backend
    this.service.createRegistration(this.registration).subscribe(
      data => {
        this.submitting = false; // turn off submission state
        this.clearRegistration(); // prevent double submissions
        this.router.navigate(['../step-4/' + data.essFileNumber], { relativeTo: this.route });
      },
      err => {
        console.log(err);
        // do not submit anymore
        this.submitting = false; // turn off submission state
        this.clearRegistration(); // prevent double submissions
        this.router.navigate(['../error'], { relativeTo: this.route });
      }
    );
  }

  processData(value: Registration): void {
    // stamp the dates that we want to track for this record
    value.selfRegisteredDate = new Date().toJSON();
    // by clicking submit this has to be true because submit is consent
    value.declarationAndConsent = true;
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
}
