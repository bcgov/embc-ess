import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { takeWhile } from 'rxjs/operators';

import { AppState } from 'src/app/store';
import { Registration } from 'src/app/core/models';
import { UpdateRegistration } from 'src/app/store/registration/registration.actions';

@Component({
  selector: 'app-self-registration-two',
  templateUrl: './self-registration-two.component.html',
  styleUrls: ['./self-registration-two.component.scss']
})
export class SelfRegistrationTwoComponent implements OnInit, OnDestroy {

  // state needed by this FORM
  currentRegistration$ = this.store.select(state => state.registrations.currentRegistration);

  form: FormGroup;
  componentActive = true;

  registration: Registration | null;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  // TODO: Form UI logic; i.e. show additional form fields when a checkbox is checked
  get ui() {
    return {
      showAvailableServices: () => this.f.requiresSupport.value === true,
    };
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.form.controls;
  }

  // Shortcuts for this.form.get(...)
  ngOnInit() {
    // Create form controls
    this.initForm();
    this.onFormChanges();

    // Update form values based on the state
    this.currentRegistration$
      .pipe(takeWhile(() => this.componentActive))
      .subscribe(value => this.displayRegistration(value));
  }

  ngOnDestroy(): void {
    this.componentActive = false;
  }

  // Define the form group
  initForm(): void {
    this.form = this.fb.group({
      dietaryNeeds: null,
      medicationNeeds: null,
      hasPets: null,
      insuranceCode: null,  // one of ['yes', 'yes-unsure', 'no', 'unsure']
      requiresSupport: null,
      requiresFood: null,
      requiresClothing: null,
      requiresAccommodation: null,
      requiresIncidentals: null,
      requiresTransportation: null,
    });
  }

  // Watch for value changes
  onFormChanges() {
    // clear any previous supports section selections based on the "require supports" radio button
    this.f.requiresSupport.valueChanges.subscribe((value: boolean) => {
      if (value === false) {
        this.resetSupports();
      }
    });
  }

  displayRegistration(registration: Registration | null): void {
    // Set the local registration property
    this.registration = registration;

    if (this.registration && this.form) {
      // Reset the form back to pristine
      this.form.reset();

      // Update the data on the form
      this.form.patchValue({
        dietaryNeeds: this.registration.dietaryNeeds,
        medicationNeeds: this.registration.medicationNeeds,
        hasPets: this.registration.hasPets,
        insuranceCode: this.registration.insuranceCode,
        requiresSupport: this.registration.requiresSupport,
        requiresFood: this.registration.requiresFood,
        requiresClothing: this.registration.requiresClothing,
        requiresAccommodation: this.registration.requiresAccommodation,
        requiresIncidentals: this.registration.requiresIncidentals,
        requiresTransportation: this.registration.requiresTransportation,
      });
    }
  }

  resetSupports(): void {
    this.form.patchValue({
      requiresFood: null,
      requiresClothing: null,
      requiresAccommodation: null,
      requiresIncidentals: null,
      requiresTransportation: null,
    });
  }

  next() {
    this.onSave();
    this.router.navigate(['../step-3'], { relativeTo: this.route });
  }

  back() {
    this.onSave();
    this.router.navigate(['../step-1'], { relativeTo: this.route });
  }

  onSave() {
    const registration: Registration = {
      ...this.registration,
      ...this.form.value
    };
    this.store.dispatch(new UpdateRegistration({ registration }));
  }
}
