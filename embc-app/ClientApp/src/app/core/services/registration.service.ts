import { Injectable } from '@angular/core';
import { Evacuee, Registration } from '../models';
import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { of, Observable } from 'rxjs';

const FAKEREGISTRATIONS: Registration[] = [
  {
    id: '123',
    dietaryNeedsDetails: 'gluten intolerance',
    disasterAffectDetails: 'Freeform text',
    essFileNumber: 123,
    externalReferralsDetails: 'Freeform text',
    facility: '',
    familyRecoveryPlan: '',
    followUpDetails: '',
    insuranceCode: "MANU120398",
    selfRegisteredDate: null,
    requiresSupport: true,
    registrationCompletionDate: null,

    registrationFamilyMemberCount: 0,
    restrictedAccess: false,
    familyMemberTakesMedication: false,
    
    hasThreeDayMedicationSupply: true,
    dietaryNeeds: false,
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
    requiresTransportation: true,

    headOfHousehold: 
      {
        id: 'qwertyuiop',
        firstName: 'John',
        lastName: 'Doe',
        nickname: 'Johnny',
        initials: 'JD',
        gender: 'yes',
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
    hostCommunity: null,
    completedBy: null
  }
]

@Injectable({
  providedIn: CoreModule
})
export class RegistrationService extends RestService{

  getRegistries(page?:number, recordLimit?:number): Observable<Registration[]>{
    //records and page are set limits on the query number
    if(!recordLimit) recordLimit = 100;
    if(!page) page = 1;

    return of(FAKEREGISTRATIONS);
  }
}
