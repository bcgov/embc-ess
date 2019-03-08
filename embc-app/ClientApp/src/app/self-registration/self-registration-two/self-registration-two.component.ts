import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { combineLatest } from 'rxjs';
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
  form: FormGroup;
  registration: Registration | null;
  componentActive = true;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  // Shortcuts for this.form.get(...)
  get supportRequired() { return this.form.get('supportRequired'); }

  // TODO: Form UI logic; i.e. show additional form fields when a checkbox is checked
  get ui() {
    return {
      showAvailableServices: () => this.form.get('supportRequired').value === true,
    };
  }

  ngOnInit() {
    this.initForm();
    this.handleFormChanges();

    // fetch data, then display form
    this.getInitialState()
      .subscribe(([current, countries, communities, relationshipTypes]) => {
        this.displayRegistration(current);
      });
  }

  ngOnDestroy(): void {
    this.componentActive = false;
  }

  getInitialState() {
    return combineLatest(
      this.store.select(state => state.registrations.currentRegistration),
      this.store.select(state => state.lookups.countries),
      this.store.select(state => state.lookups.communities),
      this.store.select(state => state.lookups.relationshipTypes),
    );
    // .pipe(
    //   takeWhile(() => this.componentActive)
    // );
  }

  // Define the form group
  initForm() {
    this.form = this.fb.group({
      hasDietaryNeeds: null,
      isTakingMedication: null,
      hasPets: null,
      insuranceCode: null,
      supportRequired: null,
      requiresFood: null,
      requiresClothing: null,
      requiresAccommodation: null,
      requiresIncidentals: null,
      requiresTransportation: null,
    });
  }

  handleFormChanges() {
    // TODO: Register any value change listeners here...
    this.supportRequired.valueChanges.subscribe((value: boolean) => {
      if (!value) {
        this.resetSupportServices();
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
        hasDietaryNeeds: this.registration.hasDietaryNeeds,
        isTakingMedication: this.registration.isTakingMedication,
        hasPets: this.registration.hasPets,
        insuranceCode: this.registration.insuranceCode,
        supportRequired: this.registration.supportRequired,
        requiresFood: this.registration.requiresFood,
        requiresClothing: this.registration.requiresClothing,
        requiresAccommodation: this.registration.requiresAccommodation,
        requiresIncidentals: this.registration.requiresIncidentals,
        requiresTransportation: this.registration.requiresTransportation,
      });
    }
  }

  resetSupportServices(): void {
    // const checkboxes = this.requestedSupportServices.controls;
    // checkboxes.forEach(cb => cb.setValue(false));
  }

  onSave() {
    const form = this.form.value;
    const newState: Registration = { ...this.registration, ...this.form.value };
    this.store.dispatch(new UpdateRegistration({ registration: newState }));
  }

  next() {
    this.onSave();
    this.router.navigate(['../step-3'], { relativeTo: this.route });
  }

  back() {
    this.router.navigate(['../step-1'], { relativeTo: this.route });
  }
}
