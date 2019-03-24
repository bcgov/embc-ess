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
    firstName: 'Fonzie',
    initials: 'D',
    lastName: 'Delphon',
    nickname: 'Fando',
    gender: 'Male',
    dob: new Date(),
    organization: {
      id: '1234',
      name: 'Quartech',
      bceidAccountNumber: 'BCEIDACCOUNTNUMBER'
    }
  },
  {
    personType: 'VOLN',
    id: 'none',
    bceidAccountNumber: 'NUMBERGOESHERE',
    isAdministrator: false,
    isPrimaryContact: true,
    canAccessRestrictedFiles: false,
    firstName: 'Beana',
    initials: 'K',
    lastName: 'Andervig',
    nickname: 'Bean',
    gender: 'female',
    dob: new Date(),
    organization: {
      id: '1234',
      name: 'Quartech',
      bceidAccountNumber: 'BCEIDACCOUNTNUMBER'
    }
  },
  {
    personType: 'VOLN',
    id: 'none',
    bceidAccountNumber: 'IAMACATMEOW',
    isAdministrator: false,
    isPrimaryContact: false,
    canAccessRestrictedFiles: true,
    firstName: 'Fluffer',
    initials: '',
    lastName: 'Macgoodie',
    nickname: 'Fluf',
    gender: 'x',
    dob: new Date(),
    organization: {
      id: '764',
      name: 'Pet Store',
      bceidAccountNumber: 'KLASHIFUAYFUISOAYDOIU'
    }
  },
  {
    personType: 'VOLN',
    id: 'none',
    bceidAccountNumber: 'WHATISTHISANDWHATAMIDOING',
    isAdministrator: false,
    isPrimaryContact: false,
    canAccessRestrictedFiles: false,
    firstName: 'Allan',
    initials: '',
    lastName: 'McGruff',
    nickname: 'Big Al',
    gender: 'male',
    dob: new Date(),
    organization: {
      id: '6970',
      name: 'Land Movers',
      bceidAccountNumber: 'BCEIDACCOUNTNUMBER'
    }
  },
];

@Injectable({
  providedIn: CoreModule
})
export class VolunteerService extends RestService {

  apiRoute = 'api/volunteers';

  getAllVolunteers(): Observable<Volunteer[]> {
    // get a list of all volunteers back from the api
    // return of(VOLUNTEERS);
    return this.http.get<Volunteer[]>(this.apiRoute, { headers: this.headers })
      .pipe(
        catchError(this.handleError)
      );
  }
  getVolunteerByBceidAccountNumber(bceidAccountNumber: string): Observable<Volunteer> {
    // get a single volunteer by their bceidAccountNumber
    // return of(VOLUNTEERS[0]);
    return this.http.get<Volunteer>(this.apiRoute + bceidAccountNumber, { headers: this.headers })
      .pipe(
        catchError(this.handleError)
      );
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
    return this.http.put<Volunteer>(this.apiRoute, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
}
