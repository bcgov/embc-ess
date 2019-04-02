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

  countries$ = this.store
    .select(state => state.lookups.countries.countries)
    .pipe(map(arr => normalize(arr)));

  communities$ = this.store
    .select(state => state.lookups.communities.communities)
    .pipe(map(arr => normalize(arr)));

  relationshipTypes$ = this.store
    .select(state => state.lookups.relationshipTypes.relationshipTypes)
    .pipe(map(arr => normalize(arr, 'code')));

  form: FormGroup;
  componentActive = true;

  registration: Registration | null;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private service: RegistrationService,
  ) { }

  ngOnInit() {
    // Create form controls
    this.initForm();

    // Watch for value changes
    this.onFormChange();

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

  // convenience getter for easy access to form fields
  get f() {
    return this.form.controls;
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

  // Define the form group
  initForm() {
    this.form = this.fb.group({
      declarationAndConsent: [null, Validators.requiredTrue],
    });
  }

  onFormChange() { }

  displayRegistration(props: { registration: Registration | null }): void {
    // Set the local registration property
    this.registration = props.registration;

    if (this.registration && this.form) {
      // Reset the form back to pristine
      this.form.reset();

      // Update the data on the form
      this.form.patchValue({
        declarationAndConsent: this.registration.declarationAndConsent,
      });
    }
  }

  next() {
    const registration: Registration = {
      ...this.registration,
      ...this.form.value
    };

    // update client-side state
    this.onSave(registration);

    // TODO: POST to server, then go the "thank you" OR "error"
    this.service.createRegistration(registration).subscribe(
      data => {
        console.log('NEW REGISTRATION ==>');
        console.log(data);
        this.router.navigate(['../step-4/' + data.essFileNumber], { relativeTo: this.route });
      },
      err => {
        this.router.navigate(['../error'], { relativeTo: this.route });
      }
    );


  }

  back() {
    // clear the consent checkbox if we go back to edit the information provided so far
    this.reset();

    const registration: Registration = {
      ...this.registration,
      ...this.form.value
    };
    this.onSave(registration);
    this.router.navigate(['../step-1'], { relativeTo: this.route });
  }

  reset() {
    this.f.declarationAndConsent.reset();
  }

  onSave(registration: Registration) {
    this.store.dispatch(new UpdateRegistration({ registration }));
  }
}
