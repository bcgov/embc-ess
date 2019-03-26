import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

import { CoreModule } from '../core.module';
import { Registration } from '../models';
import { RestService } from './rest.service';
import { MetaRegistration } from '../models/meta-registration';
import { SearchQueryParameters } from 'src/app/shared/components/search';
import { HttpResponse } from '@angular/common/http';

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
    return this.http.post<Registration>('api/registrations/', data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
  updateRegistration(data: Registration): Observable<HttpResponse<any>> {
    return this.http.put<HttpResponse<any>>('api/registrations/' + data.id, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  getRegistrationById(id: string): Observable<Registration> {
    return this.http.get<Registration>('api/registrations/' + id)
      .pipe(
        retry(3),
        catchError(this.handleError),
      );
  }
}
