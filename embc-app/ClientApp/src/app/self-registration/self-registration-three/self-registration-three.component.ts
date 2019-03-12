import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { combineLatest } from 'rxjs';
import { map, takeWhile } from 'rxjs/operators';

import { INSURANCE_OPTIONS, GENDER_OPTIONS } from 'src/app/constants/lookups';
import { Registration } from 'src/app/core/models';
import { normalize } from 'src/app/shared/utils/stateUtils';
import { AppState } from 'src/app/store';
import { UpdateRegistration } from 'src/app/store/registration/registration.actions';


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
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    // Create form controls
    this.initForm();
    this.onFormChanges();

    // Update form values based on the state
    combineLatest(this.currentRegistration$, this.countries$, this.communities$, this.relationshipTypes$)
      .pipe(takeWhile(() => this.componentActive))
      .subscribe(([registration, countries, communities, relationshipTypes]) => {
        this.displayRegistration({ registration, countries, communities, relationshipTypes });
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

  // Define the form group
  initForm() {
    this.form = this.fb.group({
      declarationAndConsent: [null, Validators.requiredTrue],
    });
  }

  onFormChanges() {
  }

  displayRegistration(props: {
    registration: Registration | null;
    countries: any;
    communities: any;
    relationshipTypes: any;
  }): void {
    // Set the local registration property
    this.registration = props.registration;
    // this.countriesLookup = this.normalize(props.countries);
    // this.communitiesLookup = this.normalize(props.communities);
    // this.relationshipTypesLookup = this.normalize(props.relationshipTypes);

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
    this.onSave();
    this.router.navigate(['../step-4'], { relativeTo: this.route });
  }

  back() {
    // clear the consent checkbox if we go back to edit the information provided so far
    this.reset();
    this.onSave();
    this.router.navigate(['../step-2'], { relativeTo: this.route });
  }

  reset() {
    this.f.declarationAndConsent.reset();
  }

  onSave() {
    const registration: Registration = {
      ...this.registration,
      ...this.form.value
    };
    this.store.dispatch(new UpdateRegistration({ registration }));
  }
}
