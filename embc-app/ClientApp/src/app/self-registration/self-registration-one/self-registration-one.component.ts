import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { skipWhile, takeWhile } from 'rxjs/operators';

import { Registration, FamilyMember, isBcAddress } from 'src/app/core/models';
import { AppState } from 'src/app/store';
import { UpdateRegistration } from 'src/app/store/registration/registration.actions';
import { ValidationHelper } from 'src/app/shared/validation/validation.helper';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';
import { clearFormArray, hasErrors, invalidField } from 'src/app/shared/utils';


@Component({
  selector: 'app-self-registration-one',
  templateUrl: './self-registration-one.component.html',
  styleUrls: ['./self-registration-one.component.scss']
})
export class SelfRegistrationOneComponent implements OnInit, OnDestroy {

  // state needed by this FORM
  countries$ = this.store.select(state => state.lookups.countries.countries);
  communities$ = this.store.select(state => state.lookups.communities.communities);
  relationshipTypes$ = this.store.select(state => state.lookups.relationshipTypes.relationshipTypes);
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
      headOfHousehold: {
        firstName: {
          required: 'Please enter your first name.',
        },
        lastName: {
          required: 'Please enter your last name.',
        },
        dob: {
          required: 'Please enter your date of birth.',
          dateInThePast: 'Date of birth must be today or in the past.',
        },
      },
      registeringFamilyMembers: {
        required: 'Please register any immediate family members who live within the same household as you.',
      },
      primaryResidenceInBC: {
        required: 'Please make a selection regarding your primary residence.',
      },
      mailingAddressSameAsPrimary: {
        required: 'Please select whether your mailing address is the same as your primary residence.',
      },
      mailingAddressInBC: {
        required: 'Please make a selection regarding your mailing address.',
      },
      phoneNumber: {
        phone: 'Must be 10 digits, no spaces allowed.',
      },
      phoneNumberAlt: {
        phone: 'Must be 10 digits, no spaces allowed.',
      },
      email: {
        email: 'Please enter a valid email address.',
      }
    };

    // Define an instance of the validator for use with this form,
    // passing in this form's set of validation messages.
    this.validationHelper = new ValidationHelper(this.constraints);
  }

  // Form UI logic; i.e. show additional form fields when a checkbox is checked
  get ui() {
    return {
      showFamilyMembers: () => this.familyMembers.length > 0,
      showPrimaryAddressSection: () => this.f.primaryResidenceInBC.value !== null,
      showMailingAddressSelector: () => this.f.mailingAddressSameAsPrimary.value === false,
      showMailingAddressSection: () => this.f.mailingAddressInBC.value !== null,
    };
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.form.controls;
  }

  // Shortcuts for this.form.get(...)
  get familyMembers() {
    return this.f.familyMembers as FormArray;
  }

  get primaryResidence() {
    return this.f.primaryResidence as FormGroup;
  }

  get mailingAddress() {
    return this.f.mailingAddress as FormGroup;
  }

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
      restrictedAccess: false,
      headOfHousehold: this.fb.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        nickname: '',
        initials: '',
        gender: null,
        dob: [null, [Validators.required, CustomValidators.dateInThePast()]], // TODO: Add date format (MM/DD/YYYY)
      }),
      registeringFamilyMembers: [null, Validators.required],
      familyMembers: this.fb.array([]),
      phoneNumber: '', // only BC phones will be validates so keep validators out of here...
      phoneNumberAlt: '',
      email: ['', Validators.email],
      primaryResidenceInBC: [null, Validators.required],
      primaryResidence: this.fb.group({
        addressSubtype: '', // address fields are validated by sub-components (bc-address, other-address)
        addressLine1: '',
        postalCode: '',
        community: '',
        city: '',
        province: '',
        country: '',
      }),
      mailingAddressSameAsPrimary: [null, Validators.required],
      mailingAddressInBC: null, // this will be validated when 'mailingAddressSameAsPrimary == false'
      mailingAddress: this.fb.group({
        addressSubtype: '',
        addressLine1: '',
        postalCode: '',
        community: '',
        city: '',
        province: '',
        country: '',
      }),
    });
  }

  onFormChange(): void {
    // validate the whole form as we capture data
    this.form.valueChanges.subscribe(() => this.validateForm());

    // validate phone numbers, for BC residents ONLY!
    // NOTE - international numbers are not validated due to variance in formats, etc.
    this.f.primaryResidenceInBC.valueChanges
      .pipe(skipWhile(() => this.f.primaryResidenceInBC.pristine))
      .subscribe((checked: boolean) => {
        if (checked) {
          this.f.phoneNumber.setValidators([CustomValidators.phone]);
          this.f.phoneNumberAlt.setValidators([CustomValidators.phone]);
        } else {
          this.f.phoneNumber.setValidators(null);
          this.f.phoneNumberAlt.setValidators(null);
        }
        this.f.phoneNumber.updateValueAndValidity();
        this.f.phoneNumberAlt.updateValueAndValidity();
      });

    // validate mailing address selection (BC address vs out-of-BC)
    // NOTE - this depends on mailingAddressSameAsPrimary being `false`
    this.f.mailingAddressSameAsPrimary.valueChanges
      .pipe(skipWhile(() => this.f.mailingAddressSameAsPrimary.pristine))
      .subscribe((checked: boolean) => {
        if (checked) {
          this.f.mailingAddressInBC.setValidators(null);
        } else {
          this.f.mailingAddressInBC.setValidators([Validators.required]);
        }
        this.f.mailingAddressInBC.updateValueAndValidity();
      });

    // show/hide family members section based on the "family info" radio button
    this.f.registeringFamilyMembers.valueChanges
      .pipe(skipWhile(() => this.f.registeringFamilyMembers.pristine))
      .subscribe((value: string) => {
        if (value === 'yes') {
          this.addFamilyMember();
        } else {
          this.clearFamilyMembers();
        }
      });

    // set "family info" radio to "No family" when all members have been removed from the form
    this.familyMembers.valueChanges
      .pipe(skipWhile(() => this.f.registeringFamilyMembers.pristine))
      .subscribe((family: any[]) => {
        const radio = this.f.registeringFamilyMembers;
        if (radio.value === 'yes' && family.length === 0) {
          radio.setValue('no');
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

      const hoh = this.registration.headOfHousehold;
      const primaryResidence = hoh.primaryResidence;
      const mailingAddress = hoh.mailingAddress;

      // Update the data on the form
      this.form.patchValue({
        restrictedAccess: this.registration.restrictedAccess,
        headOfHousehold: {
          firstName: hoh.firstName,
          lastName: hoh.lastName,
          nickname: hoh.nickname,
          initials: hoh.initials,
          gender: hoh.gender,
          dob: hoh.dob,
        },
        registeringFamilyMembers: this.registration.registeringFamilyMembers,
        familyMembers: hoh.familyMembers,
        phoneNumber: hoh.phoneNumber,
        phoneNumberAlt: hoh.phoneNumberAlt,
        email: hoh.email,
        mailingAddressSameAsPrimary: null,
      });

      if (primaryResidence != null) {
        this.form.patchValue({
          primaryResidenceInBC: isBcAddress(primaryResidence),
          primaryResidence: {
            addressSubtype: primaryResidence.addressSubtype,
            addressLine1: primaryResidence.addressLine1,
            postalCode: primaryResidence.postalCode,
            community: primaryResidence.community,
            city: primaryResidence.city,
            province: primaryResidence.province,
            country: primaryResidence.country,
          },
        });
      }

      if (mailingAddress != null) {
        this.form.patchValue({
          mailingAddressSameAsPrimary: false,
          mailingAddressInBC: isBcAddress(mailingAddress),
          mailingAddress: {
            addressSubtype: mailingAddress.addressSubtype,
            addressLine1: mailingAddress.addressLine1,
            postalCode: mailingAddress.postalCode,
            community: mailingAddress.community,
            city: mailingAddress.city,
            province: mailingAddress.province,
            country: mailingAddress.country,
          },
        });
      }
    }
  }

  // family member formgroup
  createFamilyMember(): FormGroup {
    return this.fb.group({
      sameLastNameAsEvacuee: true,
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      initials: '',
      gender: null,
      dob: [null, [Validators.required, CustomValidators.dateInThePast()]], // TODO: Add date format (MM/DD/YYYY)
      relationshipToEvacuee: [null, Validators.required],
    });
  }

  addFamilyMember(): void {
    this.familyMembers.push(this.createFamilyMember());
  }

  removeFamilyMember(): void {
    const last = this.familyMembers.length - 1;
    this.familyMembers.removeAt(last);
  }

  clearFamilyMembers() {
    clearFormArray(this.familyMembers);
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

  next(): void {
    this.saveState();
    this.router.navigate(['../step-2'], { relativeTo: this.route });
  }

  saveState(): void {
    const form = this.form.value;

    // ensure proper sub-types are assigned
    const personType: 'FMBR' = 'FMBR';
    const familyMembers: FamilyMember[] = (form.familyMembers as FamilyMember[]).map(fmr => ({ ...fmr, personType }));

    // Use form values to create evacuee registration
    const registration: Registration = {
      ...this.registration,
      // restrictedAccess: form.restrictedAccess ,
      restrictedAccess: false,
      // Todo: restrictedAccess should never be true because we do not handle sensitive information with it
      registeringFamilyMembers: form.registeringFamilyMembers,
      headOfHousehold: {
        ...this.registration.headOfHousehold,
        ...form.headOfHousehold,
        personType: 'HOH',
        phoneNumber: form.phoneNumber,
        phoneNumberAlt: form.phoneNumberAlt,
        email: form.email,
        familyMembers,
        primaryResidence: { ...form.primaryResidence },
        mailingAddress: form.mailingAddressSameAsPrimary ? null : { ...form.mailingAddress },
      }
    };
    alert(registration.restrictedAccess);
    this.store.dispatch(new UpdateRegistration({ registration }));
  }
  stop() {
    // if the user clicks the access restrict we stop them in their tracks.
    alert('Halt');
  }
}
