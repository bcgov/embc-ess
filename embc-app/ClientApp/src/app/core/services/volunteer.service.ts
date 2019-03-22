import { Injectable } from '@angular/core';
import { CoreModule } from '../core.module';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Volunteer } from '../models';
import { RestService } from './rest.service';

const VOLUNTEERS: Volunteer[] = [
  {
    personType: 'VOLN',
    id: 'none',
    bceidAccountNumber: 'TESTBCEIDNUMBER',
    isAdministrator: true,
    isPrimaryContact: true,
    canAccessRestrictedFiles: true,
    firstName: '',
    initials: '',
    lastName: '',
    nickname: '',
    gender: null,
    dob: new Date(),
    organization: {
      id: '1234',
      name: 'Quartech',
      bceidAccountNumber: 'BCEIDACCOUNTNUMBER'
    }
  }
];

@Injectable({
  providedIn: CoreModule
})
export class VolunteerService extends RestService {

  getAllVolunteers(): Observable<Volunteer[]> {
    // get a list of all volunteers back from the api
    return of(VOLUNTEERS);
    // return this.http.get<Volunteer[]>('api/registrations', { headers: this.headers })
    // .pipe(
    //   catchError(this.handleError)
    // );
  }
  getVolunteerByBceidAccountNumber(bceidAccountNumber: string): Observable<Volunteer> {
    // get a single volunteer by their bceidAccountNumber
    return of(VOLUNTEERS[0]);
    // return this.http.get<Volunteer>('api/registrations/' + bceidAccountNumber, { headers: this.headers })
    //   .pipe(
    //     catchError(this.handleError)
    //   );
  }

}
