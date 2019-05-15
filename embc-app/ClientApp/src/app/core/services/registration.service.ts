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

  async printReferrals(registrationId: string, referralIds: string[], addSummary: boolean): Promise<void> {
    const blob = await this.getReferralPdfs(registrationId, referralIds, addSummary);

    // check if IE, Edge, etc
    if (window.navigator.msSaveOrOpenBlob) {
      // save PDF file
      const filename = `${registrationId}.pdf`; // FUTURE: add date stamp to filename?
      window.navigator.msSaveBlob(blob, filename);
    } else {
      // open PDF in new tab
      const tab = window.open();
      const fileURL = URL.createObjectURL(blob);
      tab.location.href = fileURL;
    }
  }

  private getReferralPdfs(registrationId: string, referralIds: string[], addSummary: boolean): Promise<Blob> {
    const data = { ReferralIds: referralIds, AddSummary: addSummary };
    return this.http.post<Blob>(`api/registrations/${registrationId}/referrals/referralPdfs`, data, { headers: this.headers, responseType: 'blob' as 'json' })
      .pipe(
        retry(3),
        catchError(this.handleError)
      )
      .toPromise();
  }

}
