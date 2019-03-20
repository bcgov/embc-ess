import { Component, OnInit } from '@angular/core';
import { FormControl, FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { Store } from '@ngrx/store';
import { AppState } from '../store';
import { state } from '@angular/animations';
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

  // state needed by this FORM
  countries$ = this.store.select(state => state.lookups.countries.countries);
  regionalDistrics$ = this.store.select(state => state.lookups.regionalDistricts);
  regions$ = this.store.select(state => state.lookups.regions);
  relationshipTypes$ = this.store.select(state => state.lookups.relationshipTypes);
  incidentTask$ = this.store.select(state => state.incidentTasks.incidentTasks);

  // The model for the form data collected
  form: FormGroup;

  registration: Registration | null;
  submission: any;
  // the ess file number on its own is useful for looking up information from the DB
  // essFileNumber: string;

  constructor(
    private formBuilder: FormBuilder,
    private store: Store<AppState>, // ngrx app state
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
          // alert(JSON.stringify(r));
          this.displayRegistration(r);
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
      restrictedAccess: null,
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
      dietaryNeedsDetails: '',
      dietaryNeeds: null,
      medicationNeeds: null,
      requiresSupport: null,
      requiresAccomodation: null,
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
        isOtherAddress: null,
      }),
      hasMailingAddress: null,
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
      externalReferralsDetails: '',

      hostCommunity: this.formBuilder.group({}), // which community is hosting
      incidentTask: this.formBuilder.group({}), // which task is this from
      // primaryResidenceCommunity: this.formBuilder.group({}), //easier to grab this outside of the headOfHousehold

      familyMembers: this.formBuilder.array([]), // array of formGroups
      followUpDetails: '',
    });
  }

  displayRegistration(r: Registration | null): void {
    // Set the local registration property
    this.registration = r;

    // TODO: Why does this stop working if there is no this in front?
    const familyMembers: FamilyMember[] = r.headOfHousehold.familyMembers;
    const primaryResidence: Address = r.headOfHousehold.primaryResidence;
    const mailingAddress: Address = r.headOfHousehold.mailingAddress;

    // If the evacuee is here now then the defer to later of the registration of family members is now currently yes.
    if (r.registeringFamilyMembers === 'yes-unsure') {
      r.registeringFamilyMembers = 'yes';
    }

    // Update the data on the form
    this.form.patchValue({
      restrictedAccess: r.restrictedAccess as boolean,
      headOfHousehold: {
        firstName: r.headOfHousehold.firstName as string,
        lastName: r.headOfHousehold.lastName as string,
        nickname: r.headOfHousehold.nickname as string,
        initials: r.headOfHousehold.initials as string,
        gender: r.headOfHousehold.gender as string,
        dob: new Date(r.headOfHousehold.dob) as Date,
        phoneNumber: r.headOfHousehold.phoneNumber as string,
        phoneNumberAlt: r.headOfHousehold.phoneNumberAlt as string,
        email: r.headOfHousehold.email as string,
        // community: regis

      } as HeadOfHousehold,
      registeringFamilyMembers: r.registeringFamilyMembers as string,
      primaryResidence: r.headOfHousehold.primaryResidence as Address,
      followUpDetails: r.followUpDetails as string,
      externalReferralsDetails: r.externalReferralsDetails as string,
      hostCommunity: r.hostCommunity as Community,
    });

    // iterate over the array and collect each family member as a formgroup and put them into a form array
    if (familyMembers != null) {
      familyMembers.forEach((m: FamilyMember) => {
        this.addFamilyMember(m);
      });
    }

    // add the primary residence back into the form
    if (primaryResidence != null) {
      // alert('Primary not null!');
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
          // this line should call itself but unfortunately it calls itself infinitely.
          // isOtherAddress: isOtherAddress(primaryResidence) as boolean,
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

  formCleanup() {
    // TODO: make sure this is sent back to the api in a well formed way.
    const r = this.form.value;
    const reg: any = {
      id: r.id as string,
      restrictedAccess: r.restrictedAccess as boolean,
      declarationAndConsent: r.declarationAndConsent as boolean,
      essFileNumber: r.essFileNumber as number,

      dietaryNeeds: r.dietaryNeeds as boolean,
      dietaryNeedsDetails: r.dietaryNeedsDetails as string,
      disasterAffectDetails: r.disasterAffectDetails as string,
      externalReferralsDetails: r.externalReferralsDetails as string,
      facility: r.facility as string,
      familyRecoveryPlan: r.familyRecoveryPlan as string,
      followUpDetails: r.followUpDetails as string,
      insuranceCode: r.insuranceCode as string,
      medicationNeeds: r.medicationNeeds as boolean,
      registrationCompletionDate: new Date() as Date, // this stamps whenever the data is cleaned up
      registeringFamilyMembers: r.registeringFamilyMembers as string, // 'yes'or'no'
      selfRegisteredDate: r.selfRegisteredDate as Date,

      hasThreeDayMedicationSupply: r.hasThreeDayMedicationSupply as boolean,
      hasInquiryReferral: r.hasInquiryReferral as boolean,
      hasHealthServicesReferral: r.hasHealthServicesReferral as boolean,
      hasFirstAidReferral: r.hasFirstAidReferral as boolean,
      hasChildCareReferral: r.hasChildCareReferral as boolean,
      hasPersonalServicesReferral: r.hasPersonalServicesReferral as boolean,
      hasPetCareReferral: r.hasPetCareReferral as boolean,
      hasPets: r.hasPets as boolean,

      requiresAccomodation: r.requiresAccomodation as boolean,
      requiresClothing: r.requiresClothing as boolean,
      requiresFood: r.requiresFood as boolean,
      requiresIncidentals: r.requiresIncidentals as boolean,
      requiresTransportation: r.requiresTransportation as boolean,
      requiresSupport: r.requiresSupport as boolean,
      headOfHousehold: null,
    };
    // the ones that are other entities
    reg.headOfHousehold = r.headOfHousehold;
    reg.incidentTask = r.incidentTask;
    reg.hostCommunity = r.hostCommunity;

    return reg;
  }

  submit() {
    // assume that the registration data is dirty or unformatted
    const reg = this.formCleanup();
    // Submit the registration
    if (this.registration) {
      // update
      this.submission = reg;
      // this.registrationService.putRegistration(this.registration).subscribe(r => { alert(JSON.stringify(r)); });
    } else {
      // post new
      this.submission = reg;
      // this.registrationService.createRegistration(this.registration).subscribe(r => { alert(JSON.stringify(r)); });
    }
  }
}
