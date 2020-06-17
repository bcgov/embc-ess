import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { skipWhile, takeWhile, tap, filter } from 'rxjs/operators';
import * as moment from 'moment';

import { AppState } from 'src/app/store';
import * as RegistrationActions from 'src/app/store/registration/registration.actions';
import { Registration, FamilyMember, isBcAddress } from 'src/app/core/models';
import { UpdateRegistration } from 'src/app/store/registration/registration.actions';
import { ValidationHelper } from 'src/app/shared/validation/validation.helper';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';
import { clearFormArray, hasErrors, invalidField } from 'src/app/shared/utils';
import { from } from 'rxjs';

@Component({
  selector: 'app-self-registration-one',
  templateUrl: './self-registration-one.component.html',
  styleUrls: ['./self-registration-one.component.scss']
})
export class SelfRegistrationOneComponent implements OnInit, OnDestroy {

  // state needed by this FORM
  countries$ = this.store.select(state => state.lookups.countries.countries);
  relationshipTypes$ = this.store.select(state => state.lookups.relationshipTypes.relationshipTypes);
  currentRegistration$ = this.store.select(state => state.registrations.currentRegistration);

  disableForm = null; // if this is true the form is hidden
  form: FormGroup;
  submitted = false;
  componentActive = true;
  registration: Registration | null;

  // `validationErrors` represents an object with field-level validation errors to display in the form
  validationErrors: { [key: string]: any } = {};

  // error summary to display; i.e. 'Some required fields have not been completed.'
  errorSummary = '';

  readonly dateMask = [/\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/]; // yyyy-mm-dd
  readonly phoneMask = [/\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/]; // 999-999-9999

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
      headOfHousehold: {
        firstName: {
          required: 'Please enter your first name.',
        },
        lastName: {
          required: 'Please enter your last name.',
        },
        dob: {
          required: 'Please enter your date of birth.',
          date: 'Please enter a valid date.',
          maxDate: 'Date of birth must be today or in the past.',
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
        phone: 'Please enter a valid telephone number.',
      },
      phoneNumberAlt: {
        phone: 'Please enter a valid telephone number.',
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

  // convenience getter for easy access to validation errors within the HTML template
  get e(): any {
    return this.validationErrors;
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

  get primaryAddressIsInBC() {
    return this.form.get("primaryResidenceInBC").value;
  }

  get evacuatedFromPrimaryAddress() {
    return this.form.get("evacuatedFromPrimaryAddress").value;
  }

  ngOnInit() {
    // Create form controls
    this.initForm();

    // Watch for value changes
    this.onFormChange();

    // Update form values based on the state
    this.currentRegistration$
      .pipe(
        takeWhile(() => this.componentActive),
        tap((registration: Registration) => {
          if (!registration) {
            // initialize the form if no registration supplied
            this.store.dispatch(new RegistrationActions.InitializeCurrentRegistration());
          }
        }),
        filter((registration: Registration) => !!registration)
      )
      .subscribe((registration: Registration) => {
        this.displayRegistration(registration);

        // TODO: I don't know where this goes in the massive amount of code below.
        // if something is coming out of the state that is not null we should turn the restriction to true
        this.disableForm = registration.restrictedAccess;
        this.form.patchValue({ restrictedAccess: registration.restrictedAccess });
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
      restrictedAccess: null,
      headOfHousehold: this.fb.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        nickname: '',
        initials: '',
        gender: null,
        dob: [null, [Validators.required, CustomValidators.date('YYYY-MM-DD'), CustomValidators.maxDate(moment())]],
      }),
      registeringFamilyMembers: [null, Validators.required],
      familyMembers: this.fb.array([]),
      phoneNumber: [null, Validators.required ], // only BC phones will be validated so keep validators out of here...
      noPhoneNumber: [null],
      phoneNumberAlt: '',
      email: ['', Validators.email], //
      noEmail: [null],
      primaryResidenceInBC: [null, Validators.required],
      primaryResidence: this.fb.group({
        addressSubtype: '', // address fields are validated by sub-components (bc-address, other-address)
        addressLine1: '',
        postalCode: '',
        community: '',
        province: '',
        country: '',
        city: '',
      }),
      mailingAddressSameAsPrimary: [null, Validators.required],
      mailingAddressInBC: null, // this will be validated when 'mailingAddressSameAsPrimary == false'
      mailingAddress: this.fb.group({
        addressSubtype: '',
        addressLine1: '',
        postalCode: '',
        community: '',
        province: '',
        country: '',
        city: '',
      }),
      evacuatedFromPrimaryAddress: null,
      evacuatedFrom: null,
    });
    // Add custom validation for the 'not provided' inputs
    // Can't do this in the above function because at that point
    // the parent property of the controls is undefined.
    const email             = this.form.get("email");
    const phoneNumber       = this.form.get("phoneNumber");
    const evacFrom          = this.form.get("evacuatedFrom");
    const evacFromPrimeAddr = this.form.get("evacuatedFromPrimaryAddress");
    const primaryAddr       = this.form.get("primaryResidence");

    email.setValidators([Validators.email, CustomValidators.requiredWhenNull("noEmail")]);
    email.updateValueAndValidity();

    phoneNumber.setValidators(CustomValidators.requiredWhenNull("noPhoneNumber"));
    phoneNumber.updateValueAndValidity();

    evacFrom.setValidators(CustomValidators.requiredWhenFalse("evacuatedFromPrimaryAddress"));
    evacFrom.updateValueAndValidity();

    evacFromPrimeAddr.setValidators(CustomValidators.requiredWhenTrue("primaryResidenceInBC"));
    evacFromPrimeAddr.valueChanges.subscribe(value => {
      const evacFrom = this.form.get("evacuatedFrom");
      const primAddr = this.form.get("primaryResidence").value.community;
      // Update evacuated on
      if (value) {
        evacFrom.setValue(primAddr);
      }
      // Clear evacuatedOn
      else {
        evacFrom.setValue(null);
      }
    });
    evacFromPrimeAddr.updateValueAndValidity();

    primaryAddr.valueChanges.subscribe(value => {
      const evacFromPrimeAddr = this.form.get("evacuatedFromPrimaryAddress").value;
      const evacFrom          = this.form.get("evacuatedFrom");
      if (evacFromPrimeAddr) {
        evacFrom.setValue(value.community);
      }
    });

    this.form.updateValueAndValidity();
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
          this.f.phoneNumber.setValidators([CustomValidators.phone, CustomValidators.requiredWhenNull("noPhoneNumber")]);
          this.f.phoneNumberAlt.setValidators([CustomValidators.phone]);
        } else {
          this.f.phoneNumber.setValidators(CustomValidators.requiredWhenNull("noPhoneNumber"));
          this.f.phoneNumberAlt.setValidators(null);
        }
        this.f.phoneNumber.updateValueAndValidity();
        this.f.phoneNumberAlt.updateValueAndValidity();
      });

    // validate mailing address selection (BC address vs out-of-BC)
    // NOTE - this depends on mailingAddressSameAsPrimary being `false`
    this.f.mailingAddressSameAsPrimary.valueChanges
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

  displayRegistration(registration: Registration): void {
    if (registration !== null) {
      this.disableForm = null;
    }

    // Set the local registration property
    this.registration = registration;

    // if there is a registration that is not null and the form is initialized
    // clear the form and patch the values back into it.
    if (this.registration && this.form && !this.registration.restrictedAccess) {
      // Reset the form back to pristine
      this.form.reset();

      const hoh = this.registration.headOfHousehold;
      const primaryResidence = hoh.primaryResidence;
      const mailingAddress = hoh.mailingAddress;

      // iterate over the array and collect each family member as a formgroup and put them into a form array
      // we need to do this before we update the main form so it populates the FormArray properly
      if (hoh.familyMembers != null) {
        hoh.familyMembers.forEach((m: FamilyMember) => {
          this.addFamilyMember(m);
        });
      }

      // set the page state and value for the restricted access
      // if (this.registration.restrictedAccess === null) {
      // } else if (this.registration.restrictedAccess === true) {
      //   this.setRestricted(true);
      // } else if (this.registration.restrictedAccess === false) {
      //   this.setRestricted(false);
      // }

      // Update the data on the form
      this.form.patchValue({
        restrictedAccess: this.registration.restrictedAccess,
        headOfHousehold: {
          firstName: hoh.firstName,
          lastName: this.asStringAndUpperCase(hoh.lastName),
          nickname: hoh.nickname,
          initials: hoh.initials,
          gender: hoh.gender,
          dob: hoh.dob,
        },
        registeringFamilyMembers: this.registration.registeringFamilyMembers,
        phoneNumber: hoh.phoneNumber,
        noPhoneNumber: hoh.noPhoneNumber,
        phoneNumberAlt: hoh.phoneNumberAlt,
        email: hoh.email,
        noEmail: hoh.noEmail,
        evacuatedFrom: this.registration.hostCommunity, // even when host community has a value evacuatedFrom is often null
        evacuatedFromPrimaryAddress: this.registration.evacuatedFromPrimaryAddress,
      });

      // Handle no email and no phone number logic
      this.noEmailToggle();
      this.noPhoneNumberToggle();
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
          // if we got a primary address and no mailing address, it means they are the same.
          mailingAddressSameAsPrimary: (mailingAddress == null),
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
      // Patching this value in the form.patchValue function does not always work
      // Taking it out of that function and doing it before updating the form values as a fix
      const evacFrom = this.form.get("evacuatedFrom");
      evacFrom.setValue(this.registration.hostCommunity);
      evacFrom.updateValueAndValidity();
      this.form.updateValueAndValidity();
    }
  }

  // family member formgroup
  createFamilyMember(fmbr?: FamilyMember): FormGroup {
    if (fmbr) {
      return this.fb.group({
        id: fmbr.id || null,
        active: fmbr.active || null,
        sameLastNameAsEvacuee: fmbr.sameLastNameAsEvacuee,
        firstName: [fmbr.firstName, Validators.required],
        lastName: [this.asStringAndUpperCase(fmbr.lastName), Validators.required],
        initials: fmbr.initials,
        gender: fmbr.gender,
        dob: [fmbr.dob, [Validators.required, CustomValidators.date('YYYY-MM-DD'), CustomValidators.maxDate(moment())]],
      });
    } else {
      return this.fb.group({
        sameLastNameAsEvacuee: true,
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        initials: '',
        gender: null,
        dob: [null, [Validators.required, CustomValidators.date('YYYY-MM-DD'), CustomValidators.maxDate(moment())]],
      });
    }
  }

  private asStringAndUpperCase(value: any): string {
    const s = value as string;
    return s ? s.toUpperCase() : null;
  }

  addFamilyMember(fmbr?: FamilyMember): void {
    this.familyMembers.push(this.createFamilyMember(fmbr));
  }

  removeFamilyMember(i: number): void {
    this.familyMembers.removeAt(i);
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

    // ensure proper sub-types are assigned to people entities
    const personType: 'FMBR' = 'FMBR';
    const familyMembers: FamilyMember[] = (form.familyMembers as FamilyMember[]).map(fmr => ({ ...fmr, personType }));

    // Use form values to create evacuee registration
    const registration: Registration = {
      ...this.registration,
      restrictedAccess: form.restrictedAccess,
      // Todo: restrictedAccess should never be true because we do not handle sensitive information with it
      registeringFamilyMembers: form.registeringFamilyMembers,
      headOfHousehold: {
        ...this.registration.headOfHousehold,
        ...form.headOfHousehold,
        personType: 'HOH',
        phoneNumber: form.phoneNumber,
        noPhoneNumber: form.noPhoneNumber,
        phoneNumberAlt: form.phoneNumberAlt,
        email: form.email,
        noEmail: form.noEmail,
        familyMembers,
        primaryResidence: { ...form.primaryResidence },
        mailingAddress: form.mailingAddressSameAsPrimary ? null : { ...form.mailingAddress },
      },
      hostCommunity: form.evacuatedFrom,
      evacuatedFromPrimaryAddress: form.evacuatedFromPrimaryAddress,
    };
    // alert(registration.restrictedAccess);
    this.store.dispatch(new UpdateRegistration({ registration }));
  }

  setRestricted(state: boolean) {
    // if restricted equals true then hide the form.
    // this turns on or off the form view.
    this.disableForm = state;
    // set the value of the restricted form element
    this.form.patchValue({ restrictedAccess: state });
  }

  nullMailingAddress() {
    this.f.mailingAddressInBC.setValidators(null);
  }

  evacdFromPrimaryResidenceChange(value: boolean) {
    const evacFrom = this.form.get("evacuatedFrom");
    if (value) {
      evacFrom.setValidators(Validators.required);
    }
    else {
      evacFrom.clearValidators();
    }
  }

  noPhoneNumberToggle() {
    const noPhoneNumber = this.form.get("noPhoneNumber");
    const phoneNumber = this.form.get("phoneNumber");              
    // If no phone number is going to be provided, disable control and clear value
    if (noPhoneNumber.value) {
      phoneNumber.setValue(null);
      phoneNumber.disable();
      phoneNumber.clearValidators();
    }
    // Else enable control
    else {
      phoneNumber.enable();
    }
    // Update validators - bc phone numbers get an additional validator
    if (this.f.primaryResidenceInBC.value) {
      phoneNumber.setValidators([CustomValidators.phone, CustomValidators.requiredWhenNull("noPhoneNumber")])
    }
    else {
      phoneNumber.setValidators(CustomValidators.requiredWhenNull("noPhoneNumber"));
    }
    noPhoneNumber.updateValueAndValidity();
    phoneNumber.updateValueAndValidity();
  }

  noEmailToggle() {
    const noEmail = this.form.get("noEmail");
    const email = this.form.get("email");
    // If no email will be provided, disable control and clear value
    if (noEmail.value) {
      email.setValue(null);
      email.disable();
    }
    // Else enable control
    else {
      email.enable();
    }
    // Update validators
    noEmail.updateValueAndValidity();
    email.updateValueAndValidity();
  }

  // The pimrary address in BC value affects other controls
  pimraryAddrInBCToggle(value : boolean) {
    const evacFromPrimeAddr = this.form.get("evacuatedFromPrimaryAddress");
    // If false, set value of evacuatedFromPrimaryAddress to false 
    // since they can't be evac'd from a non-BC address
    if (!value) {
      evacFromPrimeAddr.setValue(false);
    }
    else {
      evacFromPrimeAddr.setValue(null);
    }
    this.form.updateValueAndValidity();
  }

  evacdFromPrimaryAddress() {
    const evacuatedFrom = this.form.get("evacuatedFrom");
    const primaryAddr = this.form.get("primaryResidence").value.community;
    evacuatedFrom.setValue(primaryAddr);
    evacuatedFrom.updateValueAndValidity();

    this.form.updateValueAndValidity();
  }

}
