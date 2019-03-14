import { Component, OnInit } from '@angular/core';
import { combineLatest } from 'rxjs';

import { User, Registration } from './core/models'; // TODO: remove registration
import { detectIE10orLower } from './shared/utils/environmentUtils';
import { ControlledListService } from './core/services/controlled-list.service';
import { RegistrationService } from './core/services/registration.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = '';
  isIE = false;
  currentUser: User;

  registrations: Registration[]; // TODO: Delete this testing var

  constructor(
    private lookups: ControlledListService,
    private registrationService: RegistrationService, // TODO: Delete this. It is for testing only
    ) { }

  ngOnInit() {
    this.isIE = detectIE10orLower();
    this.initializeApp();
    this.registrationService.getRegistries().subscribe(r => this.registrations = r );
  }

  submitARegistrationTest() {
    // this submits a constant to the api endpoint so that I don't have to fill out the form over and over
    const fakeReg: Registration = {
      id: '',
      restrictedAccess: false,
      essFileNumber:  null,
      declarationAndConsent: false,
  
      dietaryNeeds: false,
      dietaryNeedsDetails: 'gluten intolerance',
      disasterAffectDetails: 'Freeform text',
      externalReferralsDetails: 'Freeform text',
      facility: '',
      familyRecoveryPlan: '',
      followUpDetails: '',
      insuranceCode: 'MANU120398',
      medicationNeeds: false,
      selfRegisteredDate: null,
      registrationCompletionDate: null,
      registeringFamilyMembers: 'yes',
      
      hasThreeDayMedicationSupply: true,
      hasInquiryReferral: false,
      hasHealthServicesReferral: false,
      hasFirstAidReferral: false,
      hasChildCareReferral: false,
      hasPersonalServicesReferral: false,
      hasPetCareReferral: false,
      hasPets: false,
  
      requiresAccommodation: false,
      requiresClothing: false,
      requiresFood: false,
      requiresIncidentals: false,
      requiresSupport: true,
      requiresTransportation: true,
  
      headOfHousehold:
      {
        id: 'qwertyuiop',
        firstName: 'Barry',
        lastName: 'Placebo',
        nickname: 'Bipo',
        initials: 'BP',
        gender: 'Female',
        dob: null,
        phoneNumber: '',
        phoneNumberAlt: '',
        personType: 'HOH',
        email: 'person@address.org',
        primaryResidence: null,
        mailingAddress: null
      },
      familyMembers: [],
      incidentTask: null,
      hostCommunity: {
        id: 'aslkdfjs',
        name: 'Townland',
        regionalDistrict: null
      },
      completedBy: null
    }
    this.registrationService.createRegistration(fakeReg).subscribe(r => this.registrations = r);
  }


  get versionInfo(): any {
    return null;
  }

  get isAuthenticated(): boolean {
    return this.currentUser != undefined;
  }

  get isNewUser(): boolean {
    return this.isAuthenticated && this.currentUser.isNewUser === true;
  }

  get isReturningUser(): boolean {
    return this.isAuthenticated && this.currentUser.isNewUser === false;
  }

  initializeApp() {
    // TODO: Load current user (if authenticated)

    // Loaded once at init time, as they do not change very often, and
    // certainly not within the app.
    this.getLookups().subscribe();
  }

  getLookups() {
    return combineLatest([
      this.lookups.getAllCountries(),
      this.lookups.getAllRegions(),
      this.lookups.getAllRegionalDistricts(),
      this.lookups.getAllCommunities(),
      this.lookups.getAllFamilyRelationshipTypes(),
      // ...add more
    ]);
  }
}
