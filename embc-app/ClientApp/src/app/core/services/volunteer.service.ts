import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

import { CoreModule } from '../core.module';
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
    gender: 'X',
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

  apiRoute: string = 'api/volunteers';

  getAllVolunteers(): Observable<Volunteer[]> {
    // get a list of all volunteers back from the api
    return of(VOLUNTEERS);
    // return this.http.get<Volunteer[]>(this.apiRoute, { headers: this.headers })
    // .pipe(
    //   catchError(this.handleError)
    // );
  }
  getVolunteerByBceidAccountNumber(bceidAccountNumber: string): Observable<Volunteer> {
    // get a single volunteer by their bceidAccountNumber
    return of(VOLUNTEERS[0]);
    // return this.http.get<Volunteer>(this.apiRoute + bceidAccountNumber, { headers: this.headers })
    //   .pipe(
    //     catchError(this.handleError)
    //   );
  }
  createVolunteer(data: Volunteer): Observable<Volunteer> {
    // this will return a response string of 200. This may need to become a Response eventually
    // return of('200');
    return this.http.post<Volunteer>(this.apiRoute, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
  updateVolunteer(data: Volunteer): Observable<Volunteer> {
    // this will return a response string of 200. This may need to become a Response eventually
    // return of('200');
    return this.http.put<Volunteer>(this.apiRoute + data.id, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
}
