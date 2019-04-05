import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { takeWhile, map } from 'rxjs/operators';

import { RegistrationService } from 'src/app/core/services/registration.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { VolunteerService } from 'src/app/core/services/volunteer.service';
import { AppState } from 'src/app/store';
import { Registration, Address, isBcAddress, User, Volunteer } from 'src/app/core/models';
import { normalize } from 'src/app/shared/utils';
import { INSURANCE_OPTIONS, GENDER_OPTIONS } from 'src/app/constants/lookups';

@Component({
  selector: 'app-evacuee-registration-confirmation',
  templateUrl: './evacuee-registration-confirmation.component.html',
  styleUrls: ['./evacuee-registration-confirmation.component.scss']
})
export class EvacueeRegistrationConfirmationComponent implements OnInit, OnDestroy {

  // current application state
  currentRegistration$ = this.store
    .select(state => state.registrations.currentRegistration);

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
  submitting = false;

  // local copy of the application state
  registration: Registration | null;

  // who's completing/editing this evacuee registration
  currentUser: User;

  constructor(
    private store: Store<AppState>, // ngrx app state
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private registrations: RegistrationService,
    private volunteers: VolunteerService,
    private auth: AuthService,
  ) { }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  ngOnInit() {
    this.auth.getCurrentUser().subscribe(u => this.currentUser = u);

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
    // in transmission
    this.submitting = true;
    // stop here if form is invalid
    if (this.form.invalid) {
      this.submitting = false;
      return;
    }

    // Copy over all of the original properties
    // Then copy over the values from the form
    // This ensures values not on the form, such as the Id, are retained
    const value: Registration = { ...this.registration, ...this.form.value };

    // process the registration record before submission to the backend
    this.processData(value);

    // push changes to backend
    if (value.id == null) {
      this.registrations
        .createRegistration(value)
        .subscribe(() => {
          this.submitting = false;
          this.goToDashboard({ evacuee_created: true });
        });
    } else {
      this.registrations
        .updateRegistration(value)
        .subscribe(() => {
          this.submitting = false;
          this.goToDashboard({ evacuee_updated: true });
        });
    }
    // TODO: Error handling above...
  }

  goToDashboard(params: object = {}) {
    this.router.navigate(['../../evacuees'], { relativeTo: this.route, queryParams: { ...params } });
  }

  processData(value: Registration): void {
    // stamp the dates that we want to track for this record
    value.registrationCompletionDate = value.registrationCompletionDate || new Date().toJSON();

    // TODO: IMPORTANT - how do we want to track provincial staff?

    // track who completed the registration for reporting purposes
    const id = this.currentUser.contactid;
    const volunteer: Partial<Volunteer> = id ? { id } : null;
    value.completedBy = value.completedBy || volunteer;
  }

}
