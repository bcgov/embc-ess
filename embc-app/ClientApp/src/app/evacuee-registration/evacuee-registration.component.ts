import { Component, OnInit } from '@angular/core';
import { FormControl, FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { Registration } from '../core/models';

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
  // registration: Registration | null;

  constructor(
    private formBuilder: FormBuilder,
    // private store: Store<AppState>, // ngrx app state
  ) {
    // build the form with formbuilder
    this.initForm();
  }

  ngOnInit() {
  }

  addFamilyMember(): void {
    const familyMembers = this.form.get('familyMembers') as FormArray;
    familyMembers.push(this.createFamilyMember());
    this.form.setValue(familyMembers)
  }
  createFamilyMember(): FormGroup {
    // make a new family member blank and return it.
    return this.formBuilder.group({
      firstName: '',
      lastName: '',
      nickname: '',
      initials: '',
      gender: null,
      dob: null,
    });
  }

  getBoolean(booleanString: string): boolean {
    // convert boolean strings into actual boolean values
    if (booleanString === 'false') {
      return false;
    } else if (booleanString === 'true' ) {
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
      }),
      insuranceCode: '',
      dietaryNeeds: null,
      medicationNeeds: null,
      requiresSupport: null,
      disasterAffectDetails: null,
      registeringFamilyMembers: null,
      familyMembers: this.formBuilder.array([this.createFamilyMember()]),
      phoneNumber: '',
      phoneNumberAlt: '',
      email: '',
      primaryResidenceInBC: null,
      primaryResidence: this.formBuilder.group({
        addressLine1: '',
        communityOrCity: '',
        provinceOrState: '',
        postalCodeOrZip: '',
        country: '',
      }),
      hasMailingAddress: null,
      mailingAddressInBC: null,
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
    });
  }
}
