import { Component, OnInit } from '@angular/core';
import { Registration } from '../core/models';
import { RegistrationService } from '../core/services/registration.service';

interface Stub {
  id?: string; // the guid to link them to their file
  restrictedAccess: boolean; // should this file be shown or not?
  essFileNumber: number; // what is the ESS file number
  firstName: string;
  lastName: string;
  incidentTaskTaskNumber: string;
  requiresIncidentals: boolean; // do they need vouchers
  personType: string; // HOH || FMBR || VOLN
  evacuatedFrom: string; // community name
  evacuatedTo: string; // community name
  registrationCompletionDate: Date;

}
@Component({
  selector: 'app-volunteer-dashboard',
  templateUrl: './volunteer-dashboard.component.html',
  styleUrls: ['./volunteer-dashboard.component.scss']
})
export class VolunteerDashboardComponent implements OnInit {

  registrations: Stub[];
  raw: Registration[];

  constructor(
    private registrationService: RegistrationService
  ) { }

  ngOnInit() {
    // go get the data
    this.refreshRegistrations();
    // this.unreduceRegistrationToStubs([
    //   {
    //     id: '123',
    //     restrictedAccess: false,
    //     essFileNumber: 1289734,
    //     declarationAndConsent: false,
    //     dietaryNeeds: false,
    //     dietaryNeedsDetails: 'gluten intolerance',
    //     disasterAffectDetails: 'Freeform text',
    //     externalReferralsDetails: 'Freeform text',
    //     facility: '',
    //     familyRecoveryPlan: '',
    //     followUpDetails: '',
    //     insuranceCode: 'MANU120398',
    //     medicationNeeds: false,
    //     selfRegisteredDate: null,
    //     registrationCompletionDate: new Date('2019-03-11T20:48:31.246Z'),
    //     registeringFamilyMembers: 'yes',

    //     hasThreeDayMedicationSupply: true,
    //     hasInquiryReferral: false,
    //     hasHealthServicesReferral: false,
    //     hasFirstAidReferral: false,
    //     hasChildCareReferral: false,
    //     hasPersonalServicesReferral: false,
    //     hasPetCareReferral: false,
    //     hasPets: false,

    //     requiresAccommodation: false,
    //     requiresClothing: false,
    //     requiresFood: false,
    //     requiresIncidentals: false,
    //     requiresSupport: true,
    //     requiresTransportation: true,

    //     headOfHousehold:
    //     {
    //       id: 'qwertyuiop',
    //       firstName: 'John',
    //       lastName: 'Doe',
    //       nickname: 'Johnny',
    //       initials: 'JD',
    //       gender: 'male',
    //       dob: null,
    //       phoneNumber: '',
    //       phoneNumberAlt: '',
    //       personType: 'HOH' as 'HOH',
    //       email: 'person@address.org',
    //       primaryResidence: null,
    //       mailingAddress: null,
    //       familyMembers: [
    //         {
    //           id: 'qwertyuiop',
    //           firstName: 'Jen',
    //           lastName: 'Borgnine',
    //           nickname: 'Iron Jen',
    //           initials: 'JB',
    //           gender: 'female',
    //           dob: null,
    //           personType: 'FMBR' as 'FMBR',
    //           relationshipToEvacuee: {
    //             code: 'IMMF',
    //             description: 'Immed'
    //           },
    //           sameLastNameAsEvacuee: false
    //         }
    //       ],
    //       },
    //     incidentTask: {
    //       id: 'aslkdfjh',
    //       taskNumber: '10293847',
    //       details: 'This is an incident task.',
    //       region: null,
    //       regionalDistrict: null,
    //       community: {
    //         id: 'zxoicuvz',
    //         name: 'Grand Forks',
    //         regionalDistrict: null
    //       }
    //     },
    //     hostCommunity: {
    //       id: 'aslkdfjs',
    //       name: 'Niagra',
    //       regionalDistrict: null
    //     },
    //     completedBy: null
    //   },
    //   {
    //     id: '123',
    //     restrictedAccess: true,
    //     essFileNumber: 1289734,
    //     declarationAndConsent: false,

    //     dietaryNeeds: false,
    //     dietaryNeedsDetails: 'gluten intolerance',
    //     disasterAffectDetails: 'Freeform text',
    //     externalReferralsDetails: 'Freeform text',
    //     facility: '',
    //     familyRecoveryPlan: '',
    //     followUpDetails: '',
    //     insuranceCode: 'MANU120398',
    //     medicationNeeds: false,
    //     selfRegisteredDate: null,
    //     registrationCompletionDate: null,
    //     registeringFamilyMembers: 'yes',

    //     hasThreeDayMedicationSupply: true,
    //     hasInquiryReferral: false,
    //     hasHealthServicesReferral: false,
    //     hasFirstAidReferral: false,
    //     hasChildCareReferral: false,
    //     hasPersonalServicesReferral: false,
    //     hasPetCareReferral: false,
    //     hasPets: false,

    //     requiresAccommodation: false,
    //     requiresClothing: false,
    //     requiresFood: false,
    //     requiresIncidentals: false,
    //     requiresSupport: true,
    //     requiresTransportation: true,

    //     headOfHousehold:
    //     {
    //       id: 'qwertyuiop',
    //       firstName: 'Barry',
    //       lastName: 'Placebo',
    //       nickname: 'Bipo',
    //       initials: 'BP',
    //       gender: 'Female',
    //       dob: null,
    //       phoneNumber: '',
    //       phoneNumberAlt: '',
    //       personType: 'HOH',
    //       email: 'person@address.org',
    //       primaryResidence: null,
    //       mailingAddress: null
    //     },
    //     familyMembers: [],
    //     incidentTask: null,
    //     hostCommunity: {
    //       id: 'aslkdfjs',
    //       name: 'Townland',
    //       regionalDistrict: null
    //     },
    //     completedBy: null
    //   },
    // ]);

  }
  refreshRegistrations() {
    // go get a fresh list of registrations from the service
    this.registrationService.getRegistries()
      .subscribe((registrations: Registration[]) => {
        this.raw = registrations;
        // save the registrations into the local data blob
        this.registrations = this.unreduceRegistrationToStubs(registrations);
      });
  }
  unreduceRegistrationToStubs(registrations: Registration[]): Stub[] {
    const stubCollector: Stub[] = [];
    // loop through registrations and get each family member
    for (const registration of registrations) {
      // push the head of household as a stub
      const hoh: Stub = {
        id: registration.id, // the guid to link them to their file
        restrictedAccess: registration.restrictedAccess, // should this file be shown or not?
        essFileNumber: registration.essFileNumber, // what is the ESS file number
        firstName: registration.headOfHousehold.firstName,
        lastName: registration.headOfHousehold.lastName,
        requiresIncidentals: registration.requiresIncidentals, // do they need vouchers
        personType: 'HOH', // HOH || FMBR || VOLN
        incidentTaskTaskNumber: null,
        evacuatedFrom: null, // community name
        evacuatedTo: null, // community name
        registrationCompletionDate: registration.registrationCompletionDate
      };

      if (registration.incidentTask && registration.incidentTask.taskNumber) {
        // check for nulls
        hoh.incidentTaskTaskNumber = registration.incidentTask.taskNumber;
      } else {
        hoh.incidentTaskTaskNumber = '';
      }
      if (registration.incidentTask && registration.incidentTask.community && registration.incidentTask.community.name) {
        // check for nulls
        hoh.evacuatedFrom = registration.incidentTask.community.name;
      } else {
        hoh.evacuatedFrom = '';
      }
      if (registration.hostCommunity && registration.hostCommunity.name) {
        // check for nulls
        hoh.evacuatedTo = registration.hostCommunity.name;
      } else {
        hoh.evacuatedTo = '';
      }
      stubCollector.push(hoh);

      // push the family members of the HOH as stubs
      for (const familyMember of registration.headOfHousehold.familyMembers) {
        const fmbr = {
          id: registration.id, // the guid to link them to their file
          restrictedAccess: registration.restrictedAccess, // should this file be shown or not?
          essFileNumber: registration.essFileNumber, // what is the ESS file number
          firstName: familyMember.firstName,
          lastName: familyMember.lastName,
          incidentTaskTaskNumber: null,
          requiresIncidentals: registration.requiresIncidentals, // do they need vouchers
          personType: familyMember.personType, // HOH || FMBR || VOLN
          evacuatedFrom: null, // community name
          evacuatedTo: null, // community name
          registrationCompletionDate: registration.registrationCompletionDate
        };

        if (registration.incidentTask && registration.incidentTask.taskNumber) {
          // check for nulls
          fmbr.incidentTaskTaskNumber = registration.incidentTask.taskNumber;
        } else {
          fmbr.incidentTaskTaskNumber = '';
        }
        if (registration.incidentTask && registration.incidentTask.community && registration.incidentTask.community.name) {
          // check for nulls
          fmbr.evacuatedFrom = registration.incidentTask.community.name;
        } else {
          fmbr.evacuatedFrom = '';
        }
        if (registration.hostCommunity && registration.hostCommunity.name) {
          // check for nulls
          fmbr.evacuatedTo = registration.hostCommunity.name;
        } else {
          fmbr.evacuatedTo = '';
        }
        stubCollector.push(fmbr);
      }
    }
    return stubCollector;
  }
}
