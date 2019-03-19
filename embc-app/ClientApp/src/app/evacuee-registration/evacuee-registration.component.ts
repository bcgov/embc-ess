import { Component, OnInit } from '@angular/core';
import { FormControl, FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { RegistrationService } from '../core/services/registration.service';
import {
  Registration, FamilyMember, isBcAddress, Community, Country,
  RelationshipType, HeadOfHousehold, Address
} from 'src/app/core/models';


@Component({
  selector: 'app-evacuee-registration',
  templateUrl: './evacuee-registration.component.html',
  styleUrls: ['./evacuee-registration.component.scss']
})
export class EvacueeRegistrationComponent implements OnInit {

  // TODO: Delete this demo version of reactive forms.
  // name = new FormControl('');

  // The model for the form data collected
  form: FormGroup;

  registration: Registration | null;
  // the ess file number on its own is useful for looking up information from the DB
  // essFileNumber: string;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private registrationService: RegistrationService
    // private store: Store<AppState>, // ngrx app state
  ) {
    // build the form with formbuilder
    this.initForm();
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.form.controls;
  }

  // Shortcuts for this.form.get(...)
  get familyMembers() {
    // this is a way to grab the familymembers in a typed way
    return this.f.familyMembers as FormArray;
  }

  ngOnInit() {
    // if there are route params we should grab them
    if (this.route.snapshot.params.essFileNumber) {
      // TODO: go get the evacuee from db eventually
      this.registrationService.getRegistrationByEssFileNumber(this.route.snapshot.params.essFileNumber)
        .subscribe(r => {
          // TODO: get first registration for now
          alert(JSON.stringify(r));
          this.displayRegistration(r[0]);
        });
    }
  }

  addFamilyMember(fmbr?: FamilyMember): void {
    // get the existing family members
    const familyMembers = this.familyMembers;
    if (fmbr) {
      // push the new family member into the array
      familyMembers.push(this.createFamilyMember(fmbr));
      // set the value for familymembers
      this.form.setValue(familyMembers);
    } else {
      // push the new family member into the array
      familyMembers.push(this.createFamilyMember());
      // set the value for familymembers
      this.form.setValue(familyMembers);
    }
  }
  removeFamilyMember(i: number): void {
    // get the existing family members
    const familyMembers = this.familyMembers;
    familyMembers.removeAt(i);
    this.form.setValue(familyMembers);
  }
  createFamilyMember(fmbr?: FamilyMember): FormGroup {
    if (fmbr) {
      return this.formBuilder.group({
        firstName: fmbr.firstName as string,
        lastName: fmbr.lastName as string,
        nickname: fmbr.nickname as string,
        initials: fmbr.initials as string,
        relationshipToEvacuee: fmbr.relationshipToEvacuee as RelationshipType,
        sameLastNameAsEvacuee: fmbr.sameLastNameAsEvacuee as boolean,
        personType: 'FMBR',
        gender: fmbr.gender as string,
        dob: new Date(fmbr.dob) as Date,
      });
    } else {
      // make a new family member blank and return it.
      return this.formBuilder.group({
        firstName: '',
        lastName: '',
        nickname: '',
        initials: '',
        relationshipToEvacuee: '',
        sameLastNameAsEvacuee: null,
        personType: 'FMBR',
        gender: null,
        dob: null,
      });
    }
  }
  clearFamilyMembers(): void {
    // reset the list of family members
    this.clear(this.familyMembers);
  }
  // TODO: Refactor into utils method
  private clear(formArray: FormArray): void {
    while (formArray && formArray.length !== 0) {
      formArray.removeAt(0);
    }
  }

  getBoolean(booleanString: string): boolean {
    // convert boolean strings into actual boolean values
    if (booleanString === 'false') {
      return false;
    } else if (booleanString === 'true') {
      return true;
    } else {
      return null;
    }
  }

  initForm(): void {
    this.form = this.formBuilder.group({
      restrictedAccess: true,
      headOfHousehold: this.formBuilder.group({
        firstName: '',
        lastName: '',
        nickname: '',
        initials: '',
        gender: null,
        dob: null,
        phoneNumber: '',
        phoneNumberAlt: '',
        email: '',
      }),
      insuranceCode: '',
      dietaryNeeds: null,
      medicationNeeds: null,
      requiresSupport: null,
      disasterAffectDetails: null,
      registeringFamilyMembers: null,
      familyRecoveryPlan: '',

      primaryResidence: this.formBuilder.group({
        addressLine1: '',
        communityOrCity: '',
        provinceOrState: '',
        postalCodeOrZip: '',
        country: '',
        isBcAddress: null,
      }),
      hasMailingAddress: null,
      distinctMailingAddress: null,
      mailingAddress: this.formBuilder.group({
        addressLine1: '',
        communityOrCity: '',
        provinceOrState: '',
        postalCodeOrZip: '',
        country: '',
      }),
      hasThreeDayMedicationSupply: null,
      hasInquiryReferral: null,
      hasHealthServicesReferral: null,
      hasFirstAidReferral: null,
      hasChildCareReferral: null,
      hasPersonalServicesReferral: null,
      hasPetCareReferral: null,
      hasPets: null,
      familyMembers: this.formBuilder.array([]), // array of formGroups
    });
  }

  displayRegistration(registration: Registration | null): void {
    // Set the local registration property
    this.registration = registration;

    // TODO: Why does this stop working if there is no this in front?
    const familyMembers: FamilyMember[] = registration.headOfHousehold.familyMembers;
    const primaryResidence: Address = registration.headOfHousehold.primaryResidence;
    const mailingAddress: Address = registration.headOfHousehold.mailingAddress;

    // If the evacuee is here now then the defer to later of the registration of family members is now currently yes.
    if (registration.registeringFamilyMembers === 'yes-unsure') {
      registration.registeringFamilyMembers = 'yes';
    }

    // Update the data on the form
    this.form.patchValue({
      restrictedAccess: registration.restrictedAccess as boolean,
      headOfHousehold: {
        firstName: registration.headOfHousehold.firstName as string,
        lastName: registration.headOfHousehold.lastName as string,
        nickname: registration.headOfHousehold.nickname as string,
        initials: registration.headOfHousehold.initials as string,
        gender: registration.headOfHousehold.gender as string,
        dob: new Date(registration.headOfHousehold.dob) as Date,
        phoneNumber: registration.headOfHousehold.phoneNumber as string,
        phoneNumberAlt: registration.headOfHousehold.phoneNumberAlt as string,
        email: registration.headOfHousehold.email as string,

      } as HeadOfHousehold,
      registeringFamilyMembers: registration.registeringFamilyMembers as string,
      primaryResidence: registration.headOfHousehold.primaryResidence as Address
    });

    // iterate over the array and collect each family member as a formgroup and put them into a form array
    if (familyMembers != null) {
      familyMembers.forEach((m: FamilyMember) => {
        this.addFamilyMember(m);
      });
    }

    // add the primary residence back into the form
    if (primaryResidence != null) {
      alert('Primary not null!');
      this.form.patchValue({
        // primaryResidenceInBC: isBcAddress(primaryResidence) as boolean,
        primaryResidence: {
          addressSubtype: primaryResidence.addressSubtype as string,
          addressLine1: primaryResidence.addressLine1 as string,
          postalCode: primaryResidence.postalCode as string,
          community: primaryResidence.community as Community,
          city: primaryResidence.city as string,
          province: primaryResidence.province as string,
          country: primaryResidence.country as Country,
          isBcAddress: isBcAddress(primaryResidence) as boolean,
        },
      });
    }
    // add the mailing address back into the form
    if (mailingAddress != null) {
      alert('Mailing not null!');
      this.form.patchValue({
        hasMailingAddress: true,
        mailingAddress: {
          addressSubtype: mailingAddress.addressSubtype as string,
          addressLine1: mailingAddress.addressLine1 as string,
          postalCode: mailingAddress.postalCode as string,
          community: mailingAddress.community as Community,
          city: mailingAddress.city as string,
          province: mailingAddress.province as string,
          country: mailingAddress.country as Country,
          isBcAddress: isBcAddress(mailingAddress) as boolean,
        },
      });
    }
  }
}
