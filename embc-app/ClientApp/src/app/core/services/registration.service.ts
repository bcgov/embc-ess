import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { CoreModule } from '../core.module';
import { Registration } from '../models';
import { RestService } from './rest.service';
@Injectable({
  providedIn: CoreModule
})
export class RegistrationService extends RestService {

  getRegistries(page?: number, recordLimit?: number): Observable<Registration[]> {
    // records and page are set limits on the query number
    if (!recordLimit) { recordLimit = 100; }
    if (!page) { page = 1; }

    return this.http.get<Registration[]>('api/registrations', { headers: this.headers })
      .pipe(
        catchError(this.handleError)
      );
  }

  createRegistration(data: Registration): Observable<Registration> {
    return this.http.post<Registration>('api/registrations', data, { headers: this.headers })
      .pipe(
        catchError(this.handleError)
      );
  }
  getRegistrationByEssFileNumber(essFileNumber: string): Observable<Registration[]> {
    // TODO: this needs to become its own api
    return this.http.get<Registration[]>('api/registrations');
    // return this.http.get<Registration>('api/registrations/' + essFileNumber);
  }
}
