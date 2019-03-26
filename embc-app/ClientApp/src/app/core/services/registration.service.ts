import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

import { CoreModule } from '../core.module';
import { Registration } from '../models';
import { RestService } from './rest.service';
import { MetaRegistration } from '../models/meta-registration';
import { SearchQueryParameters } from 'src/app/shared/components/search';

const TEST: Registration[] = [
  {
    id: '3dd31180-8d58-4f6c-95c2-102186b936e3',
    active: true,
    restrictedAccess: false,
    declarationAndConsent: true,
    essFileNumber: 1234,
    dietaryNeeds: true,
    dietaryNeedsDetails: 'Gluten intolerance',
    disasterAffectDetails: 'It was horrible...',
    externalReferralsDetails: 'There are a bunch...',
    facility: 'Quartech Lab',
    familyRecoveryPlan: 'Do what we can.',
    followUpDetails: 'Granny needs a new hip.',
    insuranceCode: 'yes',
    medicationNeeds: true,
    selfRegisteredDate: new Date(),
    registrationCompletionDate: new Date(),
    registeringFamilyMembers: 'yes-later',
    hasThreeDayMedicationSupply: false,
    hasInquiryReferral: false,
    hasHealthServicesReferral: true,
    hasFirstAidReferral: false,
    hasChildCareReferral: true,
    hasPersonalServicesReferral: false,
    hasPetCareReferral: true,
    hasPets: true,
    requiresAccommodation: true,
    requiresClothing: true,
    requiresFood: true,
    requiresIncidentals: false,
    requiresTransportation: true,
    requiresSupport: true,
    headOfHousehold: {
      phoneNumber: '1234',
      phoneNumberAlt: '1234143',
      email: 'Curtis.laycarft@quartech.com',
      primaryResidence: {
        id: 'd3b03f54-bb3b-4daa-a6f7-c79c7dceaf1d',
        addressSubtype: null,
        addressLine1: 'asldkjfhlaksjfh',
        addressLine2: 'Nothing',
        addressLine3: 'Nothing',
        postalCode: 'V8V8V8',
        community: {
          id: '747652f4-f4b3-424b-aa6d-cd5366e3f13a',
          active: true,
          name: 'Victoria',
          regionalDistrict: {
            id: '4c05ee84-1c3f-4e1c-9c1c-6c09fd765aa0',
            active: true,
            name: 'Capital Region',
            region: {
              id: 'd4f12809-1fe1-4b7a-9b06-6d865fe279fb',
              active: true,
              name: 'Vancouver Island'
            }
          }
        },
        city: 'Calgary',
        province: 'AB',
        country: {
          id: 'alksjfh',
          name: 'Canada'
        },
      },
      mailingAddress: {
        id: 'd3b03f54-bb3b-4daa-a6f7-c79c7dceaf1d',
        addressSubtype: null,
        addressLine1: 'asldkjfhlaksjfh',
        addressLine2: null,
        addressLine3: null,
        postalCode: 'V8V8V8',
        community: {
          id: '747652f4-f4b3-424b-aa6d-cd5366e3f13a',
          active: true,
          name: 'Victoria',
          regionalDistrict: {
            id: '4c05ee84-1c3f-4e1c-9c1c-6c09fd765aa0',
            active: true,
            name: 'Capital Region',
            region: {
              id: 'd4f12809-1fe1-4b7a-9b06-6d865fe279fb',
              active: true,
              name: 'Vancouver Island'
            }
          }
        },
        city: 'Calgary',
        province: 'Alberta',
        country: {
          id: 'alksjfh',
          name: 'Canada'
        },
      },
      familyMembers: [
        {
          id: 'qwpeory',
          firstName: 'Baby',
          lastName: 'McGee',
          initials: 'J',
          nickname: 'Beebs',
          sameLastNameAsEvacuee: false,
          relationshipToEvacuee: {
            code: 'IMMF',
            description: 'Could be a cousin or something.'
          },
          personType: 'FMBR',
          gender: 'female',
          dob: new Date('2019-03-06T00:00:00-08:00')

        }
      ],
      bcServicesNumber: '1092838740912834709875',
      id: 'c7f5b285-3276-4a1e-8d1e-2f821a74987d',
      active: true,
      personType: 'HOH',
      firstName: 'Curtis',
      lastName: 'LayCraft',
      nickname: 'Curty',
      initials: 'J',
      gender: 'male',
      dob: new Date('1984-03-06T00:00:00-08:00')
    },
    incidentTask: {
      id: 'wert',
      taskNumber: '56789',
      details: 'House fire',
      region: {
        id: 'lkasdjfh',
        name: 'South BC'
      },
      regionalDistrict: {
        id: 'frustrated',
        name: 'CRD',
        region: {
          id: 'lkasdjfh',
          name: 'South BC'
        },
      },
      community: {
        id: '963f9ff6-f637-4375-aac4-69b0678e1f81',
        active: true,
        name: 'Port McNeil',
        regionalDistrict: {
          id: '760bb5e1-7f59-4d43-aed8-c83887f5b9f4',
          active: true,
          name: 'Mount Waddington',
          region: {
            id: '49f5a575-ce49-42c4-a117-1207a87ccd81',
            active: true,
            name: 'Vancouver Island'
          }
        }
      },
    },
    hostCommunity: {
      id: 'asdasd',
      name: 'Victoria',
      regionalDistrict: {
        id: 'frustrated',
        name: 'CRD',
        region: {
          id: 'lkasdjfh',
          name: 'South BC'
        },
      }
    },
    completedBy: {
      id: 'askldfjhaskljdfh',
      nickname: 'Betsy',
      firstName: 'Beebs',
      lastName: 'Racklette',
      initials: 'BRB',
      personType: 'VOLN',
      gender: 'X',
      dob: new Date(),
      organization: {
        name: 'Quartech',
        bceidAccountNumber: 'askljdfhlkasjhfdlk',
        id: 'klasdjfhlkasjdfhlkasdjhf',
      },
      bceidAccountNumber: 'klsadjfhdlkasjfh',
      canAccessRestrictedFiles: true,
      isAdministrator: true,
      isPrimaryContact: true,
    }
  }
];

@Injectable({
  providedIn: CoreModule
})
export class RegistrationService extends RestService {

  getRegistrations(props: SearchQueryParameters = {}): Observable<MetaRegistration> {
    const { limit = 100, offset = 0, q, sort } = props;
    const params = {
      limit: limit.toString(), // query params are strings
      offset: offset.toString(),
      q,
      sort
    };
    return this.http.get<MetaRegistration>('api/registrations', { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  createRegistration(data: Registration): Observable<Registration> {
    // alert("createRegistration service");
    return this.http.post<Registration>('api/registrations/', data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
  updateRegistration(data: Registration): Observable<Registration> {
    // alert("putRegistration service");
    return this.http.put<Registration>('api/registrations/' + data.id, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  getRegistrationByEssFileNumber(essFileNumber: string): Observable<Registration> {
    // TODO: this needs to become its own api
    return of(TEST[0]);
    // return this.http.get<Registration>('api/registrations/' + essFileNumber)
    //   .pipe(
    //     retry(3),
    //     catchError(this.handleError),
    //   );
  }
}
