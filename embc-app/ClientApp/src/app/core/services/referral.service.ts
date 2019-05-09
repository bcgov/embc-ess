import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { CoreModule } from '../core.module';
import { Referral, ListResult } from '../models';
import { RestService } from './rest.service';

@Injectable({
  providedIn: CoreModule
})
export class ReferralService extends RestService {

  // GET api/registrations/<id>/referrals
  getReferrals(id: string, getActive: boolean = true): Observable<ListResult<Referral>> {
    // NB: hard-coded server limit is 500
    // NB: default sort order is validFrom
    // NB: if not specified, default active flag is True
    const params = { limit: '500', offset: '0', q: '', sort: '', active: getActive ? 'true' : 'false' };
    return this.http.get<ListResult<Referral>>(`api/registrations/${id}/referrals`, { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  // POST api/registrations/<id>/referrals ???
  // printReferrals(registrationId: string, referralIds: string[]): Observable<HttpResponse<any>> {
  //   return this.http.post<HttpResponse<any>>(`api/registrations/${registrationId}/referrals`, referralIds, { headers: this.headers })
  //     .pipe(
  //       retry(3),
  //       catchError(this.handleError)
  //     );
  // }

  // POST api/registrations/<id>/referrals ???
  createReferrals(registrationId: string, referrals: Referral[]): Observable<HttpResponse<any>> {
    // TODO: assemble data object for BE
    // TODO: add id [1..n] to referrals
    // TODO: only send array of selected evacuees (evacuee[] not evacuees[])
    const data = referrals;
    return this.http.post<HttpResponse<any>>(`api/registrations/${registrationId}/referrals`, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  // DELETE api/registrations/<id>/referral/<id> ???
  deactivateReferral(registrationId: string, referralId: string): Observable<HttpResponse<any>> {
    return this.http.delete<HttpResponse<any>>(`api/registrations/${registrationId}/referrals/${referralId}`, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  // GET api/registrations/<id>/referral/<id> ???
  getReferralById(registrationId: string, referralId: string): Observable<Referral> {
    return this.http.get<Referral>(`api/registrations/${registrationId}/referrals/${referralId}`, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError),
      );
  }

}
