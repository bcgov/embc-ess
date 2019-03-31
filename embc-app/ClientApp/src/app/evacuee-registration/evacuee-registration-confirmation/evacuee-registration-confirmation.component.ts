import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { takeWhile, map } from 'rxjs/operators';

import { AppState } from 'src/app/store';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { Registration, Address, isBcAddress } from 'src/app/core/models';
import { normalize } from 'src/app/shared/utils';
import { INSURANCE_OPTIONS, GENDER_OPTIONS } from 'src/app/constants/lookups';

@Component({
  selector: 'app-evacuee-registration-confirmation',
  templateUrl: './evacuee-registration-confirmation.component.html',
  styleUrls: ['./evacuee-registration-confirmation.component.scss']
})
export class EvacueeRegistrationConfirmationComponent implements OnInit, OnDestroy {

  // current application state
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
  submitted = false;

  // local copy of the application state
  registration: Registration | null;

  constructor(
    private store: Store<AppState>, // ngrx app state
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private service: RegistrationService,
  ) { }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  ngOnInit() {
    // Create form controls
    this.initForm();

    // Update form values based on the state
    this.currentRegistration$
      .pipe(takeWhile(() => this.componentActive))
      .subscribe(value => this.displayRegistration(value));
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

  // Define the form group
  initForm() {
    this.form = this.fb.group({
      declarationAndConsent: [null, Validators.requiredTrue],
    });
  }

  displayRegistration(value: Registration | null): void {
    // Set the local registration property
    this.registration = value;

    if (this.registration && this.form) {
      // Reset the form back to pristine
      this.form.reset();

      // Update the data on the form
      this.form.patchValue({
        declarationAndConsent: value.declarationAndConsent,
      });
    }
  }

  reset() {
    this.f.declarationAndConsent.reset();
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.form.invalid) {
      return;
    }

    // Copy over all of the original properties
    // Then copy over the values from the form
    // This ensures values not on the form, such as the Id, are retained
    const value: Registration = { ...this.registration, ...this.form.value };

    // push changes to backend
    if (value.id == null) {
      this.service
        .createRegistration(value)
        .subscribe(() => this.goToDashboard({ evacuee_created: true }));
    } else {
      this.service
        .updateRegistration(value)
        .subscribe(() => this.goToDashboard({ evacuee_updated: true }));
    }
    // TODO: Error handling above...
  }

  goToDashboard(params: object = {}) {
    this.router.navigate(['/volunteer-dashboard'], { queryParams: { ...params } });
  }
}
