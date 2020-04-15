import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, retry, map } from 'rxjs/operators';

import { CoreModule } from '../core.module';
import { Volunteer, ListResult } from '../models';
import { RestService } from './rest.service';
import { HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { SearchQueryParameters } from '../models/search-interfaces';

export interface VolunteerSearchQueryParameters extends SearchQueryParameters {
  org_id?: string;
  ess_only?: boolean;
  admin_only?: boolean;
}

@Injectable({
  providedIn: CoreModule
})
export class VolunteerService extends RestService {
  apiRoute = '/api/volunteers';

  getVolunteers({ limit, offset, q, sort, org_id, ess_only, admin_only }: VolunteerSearchQueryParameters = {}): Observable<ListResult<Volunteer>> {
    const params = {
      limit: (limit || 100).toString(), // query params are strings
      offset: (offset || 0).toString(),
      q: q || '',
      sort: sort || 'name',
      org_id: org_id || '',
      ess_only: (!!ess_only).toString(),
      admin_only: (!!admin_only).toString(),
    };

    // get a list of all volunteers back from the api
    return this.http.get<ListResult<Volunteer>>(this.apiRoute, { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  getVolunteerById(id: string): Observable<Volunteer> {
    // get a single volunteer by their bceidAccountNumber
    // return of(VOLUNTEERS[0]);
    return this.http.get<Volunteer>(this.apiRoute + '/' + id, { headers: this.headers })
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

  updateVolunteer(data: Volunteer): Observable<HttpResponse<any>> {
    // this will return a response string of 200. This may need to become a Response eventually
    // return of('200');
    return this.http.put<HttpResponse<any>>(this.apiRoute + '/' + data.id, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  // this API call will return:
  // * 200 OK when the bceid is taken
  // * 404 NOT_FOUND when the bceid is not taken
  isBceidTaken(bceid: string): Observable<boolean> {
    return this.http.get(`${this.apiRoute}/bceid/${bceid}`, { observe: 'response' })
      .pipe(
        map(res => res.status === 200),
        catchError((err: HttpErrorResponse) => {
          if (err.status === 404) {
            return of(false);
          }
          throwError(`Backend returned code ${err.status}, body was: ${err.message}`);
        })
      );
  }
}
