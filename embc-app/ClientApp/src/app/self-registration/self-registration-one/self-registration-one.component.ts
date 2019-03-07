import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormControl, FormArray } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { takeWhile, find, filter, map } from 'rxjs/operators';

import { Registration, Country, Community, RelationshipType } from 'src/app/core/models';
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
  get isRegisteringFamilyMembers() { return this.form.get('isRegisteringFamilyMembers'); }
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
      isRestrictedAccess: null,
      familyRepresentative: this.fb.group({
        firstName: '',
        lastName: '',
        nickname: '',
        initials: '',
        gender: '',
        dob: null,
      }),
      isRegisteringFamilyMembers: null,
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
    this.isRegisteringFamilyMembers.valueChanges.subscribe((value: number) => {
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
        isRestrictedAccess: this.registration.isRestrictedAccess,
        familyRepresentative: {
          firstName: this.registration.familyRepresentative.firstName,
          lastName: this.registration.familyRepresentative.lastName,
          nickname: this.registration.familyRepresentative.nickname,
          initials: this.registration.familyRepresentative.initials,
          gender: this.registration.familyRepresentative.gender,
          dob: this.registration.familyRepresentative.dob,
        },
        isRegisteringFamilyMembers: this.registration.isRegisteringFamilyMembers,
        familyMembers: this.registration.familyMembers,
        phoneNumber: this.registration.familyRepresentative.profile.phoneNumber,
        phoneNumberAlt: this.registration.familyRepresentative.profile.phoneNumberAlt,
        email: this.registration.familyRepresentative.profile.email,
        primaryResidence: {
          addressLine1: this.registration.familyRepresentative.profile.primaryResidence.addressLine1,
          communityOrCity: this.registration.familyRepresentative.profile.primaryResidence.communityOrCity,
          provinceOrState: this.registration.familyRepresentative.profile.primaryResidence.provinceOrState,
          postalCodeOrZip: this.registration.familyRepresentative.profile.primaryResidence.postalCodeOrZip,
          country: this.registration.familyRepresentative.profile.primaryResidence.country,
        },
        hasMailingAddress: null,
      });

      const mailingAddress = this.registration.familyRepresentative.profile.mailingAddress;
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
      isRestrictedAccess: form.isRestrictedAccess,
      isRegisteringFamilyMembers: form.isRegisteringFamilyMembers,
      familyMembers: [...form.familyMembers],
      familyRepresentative: {
        ...this.registration.familyRepresentative,
        firstName: form.familyRepresentative.firstName,
        lastName: form.familyRepresentative.lastName,
        nickname: form.familyRepresentative.nickname,
        initials: form.familyRepresentative.initials,
        gender: form.familyRepresentative.gender,
        dob: form.familyRepresentative.dob,
        profile: {
          phoneNumber: form.phoneNumber,
          phoneNumberAlt: form.phoneNumberAlt,
          email: form.email,
          primaryResidence: { ...form.primaryResidence },
          mailingAddress: form.hasMailingAddress ? { ...form.mailingAddress } : undefined,
        },
        isEvacuee: true,
        isVolunteer: false,
        isFamilyMember: false,
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
