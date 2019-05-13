import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { Registration, ListResult } from '../models';
import { SearchQueryParameters } from '../models/search-interfaces';

@Injectable({
  providedIn: CoreModule
})
export class RegistrationService extends RestService {

  getRegistrations(props: SearchQueryParameters = {}): Observable<ListResult<Registration>> {
    const { limit = 100, offset = 0, q = '', sort = '' } = props;
    const params = {
      limit: limit.toString(), // query params are strings
      offset: offset.toString(),
      q,
      sort
    };
    return this.http.get<ListResult<Registration>>('api/registrations', { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  createRegistration(data: Registration): Observable<Registration> {
    return this.http.post<Registration>('api/registrations/', data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  updateRegistration(data: Registration): Observable<HttpResponse<any>> {
    return this.http.put<HttpResponse<any>>(`api/registrations/${data.id}`, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  getRegistrationSummaryById(id: string): Observable<Registration> {
    return this.http.get<Registration>(`api/registrations/${id}/summary`, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError),
      );
  }

  // NB: if registration is not finalized and no reason is provided, this will fail with response 400
  getRegistrationById(id: string, reason: string = null): Observable<Registration> {
    const params = { reason };
    return this.http.get<Registration>(`api/registrations/${id}`, { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError),
      );
  }
}
