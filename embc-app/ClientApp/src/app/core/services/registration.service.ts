import { Injectable } from '@angular/core';
import { Evacuee, Registration } from '../models';
import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { of, Observable } from 'rxjs';

const FAKEREGISTRATIONS: Registration[] = [
  // {
  //   id: '123',
  //   essFileNumber: 123,
  //   isRestrictedAccess: false,
  //   isRegisteringFamilyMembers: 0,
  //   hasDietaryNeeds: false,
  //   isTakingMedication: false,
  //   hasThreeDaySupply: true,
  //   hasPets: false,
  //   insuranceCode: "MANU120398",
  //   supportRequired: true,
  //   requiresFood: false,
  //   requiresClothing: false,
  //   requiresIncidentals: false,
  //   requiresTransportation

  // }
]
// [
//   {
//     id: 'qwertyuiop',
//     personType: 'VOLN',
//     firstName: 'John',
//     lastName: 'Doe',
//     nickname: 'Johnny',
//     initials: 'JD',
//     gender: 'yes',
//     dob: null
//   },
//   {
//     id: 'qwertyuiop',
//     personType: 'VOLN',
//     firstName: 'Billy',
//     lastName: 'Donner',
//     nickname: 'BIDO',
//     initials: 'BD',
//     gender: 'yes',
//     dob: null
//   },
// ] 

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
