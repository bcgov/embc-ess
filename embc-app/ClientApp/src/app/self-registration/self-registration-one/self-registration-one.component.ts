import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { takeWhile } from 'rxjs/operators';

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

  // Shortcuts for this.form.get(...)
  get registeringFamilyMembers() { return this.form.get('registeringFamilyMembers'); }
  get isPrimaryResidenceInBC() { return this.form.get('isPrimaryResidenceInBC'); }
  get isMailingAddressInBC() { return this.form.get('isMailingAddressInBC'); }

  get familyMembers() { return this.form.get('familyMembers') as FormArray; }
  get primaryResidence() { return this.form.get('primaryResidence') as FormGroup; }
  get mailingAddress() { return this.form.get('mailingAddress') as FormGroup; }

  // Form UI logic; i.e. show additional form fields when a checkbox is checked
  get ui() {
    return {
      showMailingAddress: () => this.form.get('hasMailingAddress').value === true,
      showFamilyMembers: () => this.familyMembers.length > 0,
    };
  }

  ngOnInit() {
    // Create form controls
    this.initForm();
    this.handleFormChanges();

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
        gender: '',
        dob: null,
      }),
      registeringFamilyMembers: null,
      familyMembers: this.fb.array([]),
      phoneNumber: '',
      phoneNumberAlt: '',
      email: '',
      isPrimaryResidenceInBC: null,
      primaryResidence: this.fb.group({
        addressLine1: '',
        communityOrCity: '',
        provinceOrState: '',
        postalCodeOrZip: '',
        country: '',
      }),
      hasMailingAddress: null,
      isMailingAddressInBC: null,
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
  private handleFormChanges(): void {
    this.registeringFamilyMembers.valueChanges.subscribe((value: number) => {
      if (value === 1) {
        this.addFamilyMember();
      } else {
        this.clearFamilyMembers();
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

  onSave(): void {
    const form = this.form.value;
    const newState: Registration = {
      ...this.registration,
      restrictedAccess: form.restrictedAccess,
      registeringFamilyMembers: form.registeringFamilyMembers,
      familyMembers: [...form.familyMembers],
      headOfHousehold: {
        ...this.registration.headOfHousehold,
        firstName: form.headOfHousehold.firstName,
        lastName: form.headOfHousehold.lastName,
        nickname: form.headOfHousehold.nickname,
        initials: form.headOfHousehold.initials,
        gender: form.headOfHousehold.gender,
        dob: form.headOfHousehold.dob,
        phoneNumber: form.phoneNumber,
        phoneNumberAlt: form.phoneNumberAlt,
        email: form.email,
        primaryResidence: { ...form.primaryResidence },
        mailingAddress: form.hasMailingAddress ? { ...form.mailingAddress } : undefined,
      }
    };

    this.store.dispatch(new UpdateRegistration({ registration: newState }));
  }

  addFamilyMember(): void {
    this.familyMembers.push(this.fb.group({
      relationshipToEvacuee: [''],
      sameLastNameAsEvacuee: [true],
      firstName: [''],
      lastName: [''],
      initials: [''],
      gender: [undefined],
      dob: [undefined],
    }));
  }

  clearFamilyMembers() {
    this.clear(this.familyMembers);
  }

  // TODO: Refactor into utils method
  private clear(formArray: FormArray): void {
    while (formArray.length !== 0) {
      formArray.removeAt(0);
    }
  }

  next(): void {
    this.onSave();
    this.router.navigate(['../step-2'], { relativeTo: this.route });
  }
}
