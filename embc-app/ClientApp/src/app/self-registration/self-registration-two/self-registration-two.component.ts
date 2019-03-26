import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { takeWhile } from 'rxjs/operators';

import { AppState } from 'src/app/store';
import { Registration } from 'src/app/core/models';
import { UpdateRegistration } from 'src/app/store/registration/registration.actions';
import { ValidationHelper } from 'src/app/shared/validation/validation.helper';
import { hasErrors, invalidField } from 'src/app/shared/utils';

@Component({
  selector: 'app-self-registration-two',
  templateUrl: './self-registration-two.component.html',
  styleUrls: ['./self-registration-two.component.scss']
})
export class SelfRegistrationTwoComponent implements OnInit, OnDestroy {

  // state needed by this FORM
  currentRegistration$ = this.store.select(state => state.registrations.currentRegistration);

  form: FormGroup;
  submitted = false;
  componentActive = true;

  registration: Registration | null;

  // `validationErrors` represents an object with field-level validation errors to display in the form
  validationErrors: { [key: string]: any } = {};

  // error summary to display; i.e. 'Some required fields have not been completed.'
  errorSummary = '';

  // generic validation helper
  private constraints: { [key: string]: { [key: string]: string | { [key: string]: string } } };
  private validationHelper: ValidationHelper;

  // convenience getters so we can use helper functions in Angular templates
  hasErrors = hasErrors;
  invalidField = invalidField;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {
    // Defines all of the validation messages for the form.
    // These could instead be retrieved from a file or database.
    this.constraints = {
      dietaryNeeds: {
        required: 'Please make a selection regarding dietary requirements.',
      },
      medicationNeeds: {
        required: 'Please make a selection regarding medication.',
      },
      hasPets: {
        required: 'Please make a selection regarding pets.',
      },
      insuranceCode: {
        required: 'Please make a selection regarding insurance coverage.',
      },
      requiresSupport: {
        required: 'Please select whether supports are required.',
      },
    };

    // Define an instance of the validator for use with this form,
    // passing in this form's set of validation messages.
    this.validationHelper = new ValidationHelper(this.constraints);
  }

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

    // Watch for value changes
    this.onFormChange();

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
      dietaryNeeds: [null, Validators.required],
      medicationNeeds: [null, Validators.required],
      hasPets: [null, Validators.required],
      insuranceCode: [null, Validators.required],  // one of ['yes', 'yes-unsure', 'no', 'unsure']
      requiresSupport: [null, Validators.required],
      requiresFood: null,
      requiresClothing: null,
      requiresAccommodation: null,
      requiresIncidentals: null,
      requiresTransportation: null,
    });
  }

  onFormChange() {
    // validate the whole form as we capture data
    this.form.valueChanges.subscribe(() => this.validateForm());

    // clear any previous supports section selections based on the "require supports" radio button
    this.f.requiresSupport.valueChanges.subscribe((checked: boolean) => {
      if (!checked && this.f.requiresSupport.dirty) {
        this.resetSupports();
      }
    });
  }

  validateForm(): void {
    this.validationErrors = this.validationHelper.processMessages(this.form);
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

  onSubmit() {
    this.submitted = true;
    this.validateForm();

    // stop here if form is invalid
    if (this.form.invalid) {
      this.errorSummary = 'Some required fields have not been completed.';
      return;
    }

    // success!
    this.errorSummary = null;
    this.next();
  }

  next() {
    this.saveState();
    this.router.navigate(['../step-3'], { relativeTo: this.route });
  }

  back() {
    this.saveState();
    this.router.navigate(['../step-1'], { relativeTo: this.route });
  }

  saveState() {
    const registration: Registration = {
      ...this.registration,
      ...this.form.value
    };
    this.store.dispatch(new UpdateRegistration({ registration }));
  }
}
