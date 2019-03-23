import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { skipWhile, takeWhile } from 'rxjs/operators';

import { Registration, FamilyMember, isBcAddress } from 'src/app/core/models';
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
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        nickname: '',
        initials: '',
        gender: [null, Validators.required],
        dob: [null, [Validators.required]], // TODO: Add extra DOB validation (must be in the past)
      }),
      registeringFamilyMembers: [null, Validators.required],
      familyMembers: this.fb.array([]),
      phoneNumber: '',
      phoneNumberAlt: '',
      email: ['', Validators.email],
      primaryResidenceInBC: [null, Validators.required],
      primaryResidence: this.fb.group({
        addressSubtype: '',
        addressLine1: ['', Validators.required],
        postalCode: '',
        community: '',
        city: '',
        province: '',
        country: '',
      }),
      hasMailingAddress: null,
      mailingAddressInBC: null,
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
        hasMailingAddress: null,
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
          hasMailingAddress: true,
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

  newFamilyMember(): FormGroup {
    return this.fb.group({
      sameLastNameAsEvacuee: true,
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      initials: '',
      gender: [null, Validators.required],
      dob: [null, [Validators.required]], // TODO: Add extra DOB validation (must be in the past)
      relationshipToEvacuee: [null, Validators.required],
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

    // ensure proper sub-types are assigned
    const personType: 'FMBR' = 'FMBR';
    const familyMembers: FamilyMember[] = (form.familyMembers as FamilyMember[]).map(fmr => ({ ...fmr, personType }));

    // Use form values to create evacuee registration
    const registration: Registration = {
      ...this.registration,
      restrictedAccess: form.restrictedAccess,
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
        mailingAddress: form.hasMailingAddress ? { ...form.mailingAddress } : null,
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
