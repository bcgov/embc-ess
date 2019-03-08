import { Injectable } from '@angular/core';
import { RestService } from './rest.service';
import { Observable, of } from 'rxjs';
import { Evacuee } from '../models';
import { CoreModule } from '../core.module';

const FAKEEVACUEES: Evacuee[] = [
  {
    id: 'qwertyuiop',
    personType: 'VOLN',
    firstName: 'John',
    lastName: 'Doe',
    nickname: 'Johnny',
    initials: 'JD',
    gender: 'yes',
    dob: null
  },
  {
    id: 'qwertyuiop',
    personType: 'VOLN',
    firstName: 'Billy',
    lastName: 'Donner',
    nickname: 'BIDO',
    initials: 'BD',
    gender: 'yes',
    dob: null
  },
] 

@Injectable({
  providedIn: CoreModule
})
export class EvacueeService extends RestService {
  getAllEvacuees(): Observable<Evacuee[]> {
    // simple return fakes
    return of(FAKEEVACUEES);
  }
}
