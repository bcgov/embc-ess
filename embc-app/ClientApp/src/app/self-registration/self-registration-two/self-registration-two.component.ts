import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { takeWhile } from 'rxjs/operators';

import { AppState } from 'src/app/store';
import { Registration, Country } from 'src/app/core/models';
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
  countries$ = this.store.select(s => s.lookups.countries.countries);

  CANADA: Country; // default country object

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
      requiresFood: {
        required: 'Please make a selection regarding food while evacuated.',
      },
      requiresClothing: {
        required: 'Please make a selection regarding clothing while evacuated.',
      },
      requiresAccommodation: {
        required: 'Please make a selection regarding lodging while evacuated.',
      },
      requiresIncidentals: {
        required: 'Please make a selection regarding incidentals while evacuated.',
      },
      requiresTransportation: {
        required: 'Please make a selection regarding transportation while evacuated.',
      },
    };

    // Define an instance of the validator for use with this form,
    // passing in this form's set of validation messages.
    this.validationHelper = new ValidationHelper(this.constraints);
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.form.controls;
  }

  // Shortcuts for this.form.get(...)
  ngOnInit() {
    // fetch the default country
    this.countries$.subscribe((countries: Country[]) => {
      // the only(first) element that is named Canada
      countries.forEach((country: Country) => {
        // if the canada is not set and we found one in the list
        if (!this.CANADA && country.name === 'Canada') {
          this.CANADA = country;
        }
      });
    });

    // Create form controls
    this.initForm();

    // Watch for value changes
    this.onFormChange();

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

  invalid(field: string, parent: FormGroup = this.form): boolean {
    return invalidField(field, parent, this.submitted);
  }

  // Define the form group
  initForm(): void {
    this.form = this.fb.group({
      dietaryNeeds: [null, Validators.required],
      medicationNeeds: [null, Validators.required],
      hasPets: [null, Validators.required],
      insuranceCode: [null, Validators.required],  // one of ['yes', 'yes-unsure', 'no', 'unsure']
      requiresFood: [null, Validators.required],
      requiresClothing: [null, Validators.required],
      requiresAccommodation: [null, Validators.required],
      requiresIncidentals: [null, Validators.required],
      requiresTransportation: [null, Validators.required],
    });
  }

  onFormChange() {
    // validate the whole form as we capture data
    this.form.valueChanges.subscribe(() => this.validateForm());
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
    // if there was no primary address country set by the form before submission
    if (!registration.headOfHousehold.primaryResidence.country) {
      registration.headOfHousehold.primaryResidence.country = this.CANADA;
    }
    // the user included a mailing address but the form did not set the country
    if (registration.headOfHousehold.mailingAddress && !registration.headOfHousehold.mailingAddress.country) {
      registration.headOfHousehold.mailingAddress.country = this.CANADA;
    }
    this.store.dispatch(new UpdateRegistration({ registration }));
  }

}
