import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { skipWhile, takeWhile } from 'rxjs/operators';

import { Registration } from 'src/app/core/models';
import { AppState } from 'src/app/store';
import { UpdateRegistration } from 'src/app/store/registration/registration.actions';


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
  componentActive = true;

  registration: Registration | null;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  // Form UI logic; i.e. show additional form fields when a checkbox is checked
  get ui() {
    return {
      showFamilyMembers: () => this.familyMembers.length > 0,
      showPrimaryAddressSection: () => this.f.primaryResidenceInBC.value !== null,
      showMailingAddressSelector: () => this.f.hasMailingAddress.value === true,
      showMailingAddressSection: () => this.f.mailingAddressInBC.value !== null,
      showStrandedTravellerBlurb: () => this.f.primaryResidenceInBC.value === false,
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
      restrictedAccess: null,
      headOfHousehold: this.fb.group({
        firstName: '',
        lastName: '',
        nickname: '',
        initials: '',
        gender: null,
        dob: null,
      }),
      registeringFamilyMembers: null,
      familyMembers: this.fb.array([]),
      phoneNumber: '',
      phoneNumberAlt: '',
      email: '',
      primaryResidenceInBC: null,
      primaryResidence: this.fb.group({
        addressLine1: '',
        communityOrCity: '',
        provinceOrState: '',
        postalCodeOrZip: '',
        country: '',
      }),
      hasMailingAddress: null,
      mailingAddressInBC: null,
      mailingAddress: this.fb.group({
        addressLine1: '',
        communityOrCity: '',
        provinceOrState: '',
        postalCodeOrZip: '',
        country: '',
      }),
    });
  }

  // Watch for value changes
  onFormChanges(): void {
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

  displayRegistration(registration: Registration | null): void {
    // Set the local registration property
    this.registration = registration;

    if (this.registration && this.form) {
      // Reset the form back to pristine
      this.form.reset();

      // Update the data on the form
      this.form.patchValue({
        restrictedAccess: this.registration.restrictedAccess,
        headOfHousehold: {
          firstName: this.registration.headOfHousehold.firstName,
          lastName: this.registration.headOfHousehold.lastName,
          nickname: this.registration.headOfHousehold.nickname,
          initials: this.registration.headOfHousehold.initials,
          gender: this.registration.headOfHousehold.gender,
          dob: this.registration.headOfHousehold.dob,
        },
        registeringFamilyMembers: this.registration.registeringFamilyMembers,
        familyMembers: this.registration.familyMembers,
        phoneNumber: this.registration.headOfHousehold.phoneNumber,
        phoneNumberAlt: this.registration.headOfHousehold.phoneNumberAlt,
        email: this.registration.headOfHousehold.email,
        primaryResidence: {
          addressLine1: this.registration.headOfHousehold.primaryResidence.addressLine1,
          communityOrCity: this.registration.headOfHousehold.primaryResidence.communityOrCity,
          provinceOrState: this.registration.headOfHousehold.primaryResidence.provinceOrState,
          postalCodeOrZip: this.registration.headOfHousehold.primaryResidence.postalCodeOrZip,
          country: this.registration.headOfHousehold.primaryResidence.country,
        },
        hasMailingAddress: null,
      });

      const mailingAddress = this.registration.headOfHousehold.mailingAddress;
      if (mailingAddress != null) {
        this.form.patchValue({
          hasMailingAddress: true,
          mailingAddress: {
            addressLine1: mailingAddress.addressLine1,
            communityOrCity: mailingAddress.communityOrCity,
            provinceOrState: mailingAddress.provinceOrState,
            postalCodeOrZip: mailingAddress.postalCodeOrZip,
            country: mailingAddress.country,
          },
        });
      }
    }
  }

  newFamilyMember(): FormGroup {
    return this.fb.group({
      relationshipToEvacuee: null,
      sameLastNameAsEvacuee: true,
      firstName: '',
      lastName: '',
      initials: '',
      gender: null,
      dob: null,
    });
  }

  addFamilyMember(): void {
    const newOne = this.newFamilyMember();
    this.familyMembers.push(newOne);
  }

  removeFamilyMember(): void {
    const last = this.familyMembers.length - 1;
    this.familyMembers.removeAt(last);
  }

  clearFamilyMembers() {
    this.clear(this.familyMembers);
  }

  next(): void {
    this.onSave();
    this.router.navigate(['../step-2'], { relativeTo: this.route });
  }

  onSave(): void {
    const form = this.form.value;
    const registration: Registration = {
      ...this.registration,
      restrictedAccess: form.restrictedAccess,
      registeringFamilyMembers: form.registeringFamilyMembers,
      familyMembers: [...form.familyMembers],
      headOfHousehold: {
        ...this.registration.headOfHousehold,
        ...form.headOfHousehold,
        phoneNumber: form.phoneNumber,
        phoneNumberAlt: form.phoneNumberAlt,
        email: form.email,
        primaryResidence: { ...form.primaryResidence },
        mailingAddress: form.hasMailingAddress ? { ...form.mailingAddress } : undefined,
      }
    };

    this.store.dispatch(new UpdateRegistration({ registration }));
  }

  // TODO: Refactor into utils method
  private clear(formArray: FormArray): void {
    while (formArray && formArray.length !== 0) {
      formArray.removeAt(0);
    }
  }
}
