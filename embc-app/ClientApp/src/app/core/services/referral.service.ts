import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { retry, catchError, map } from 'rxjs/operators';
import { CoreModule } from '../core.module';
import { Referral, ListResult, RawReferralCollection, ReferralPost, ReferralSuccess } from '../models';
import { RestService } from './rest.service';

@Injectable({
  providedIn: CoreModule
})
export class ReferralService extends RestService {

  // GET api/registrations/<id>/referrals
  getReferrals(id: string, getActive: boolean = true): Observable<ListResult<Referral>> {
    // TODO: THIS DOESN'T RETURN WHAT IT SAYS IT DOES but to change it results in cascading component changes.
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

  // temporary function that will replace getReferrals() when Referral Table Component is updated
  // GET api/registrations/<id>/referrals
  getCleanReferrals(id: string, getActive: boolean = true): Observable<ListResult<Referral>> {
    // NB: hard-coded server limit is 500
    // NB: default sort order is validFrom
    // NB: if not specified, default active flag is True
    const params = { limit: '500', offset: '0', q: '', sort: '', active: getActive ? 'true' : 'false' };
    return this.http.get<RawReferralCollection>(`api/registrations/${id}/referrals`, { headers: this.headers, params })
      .pipe(
        retry(3),
        map((rCollection: RawReferralCollection) => {
          const referrals: ListResult<Referral> = rCollection.referrals;
          referrals.data.map((referral: Referral): Referral => {
            const cleanReferral = referral;
            cleanReferral.essNumber = rCollection.registrationId;
            return cleanReferral;
          });
          return referrals;
        }),
        catchError(this.handleError)
      );
  }

  // POST api/registrations/<id>/referrals
  createReferrals(registrationId: string, referrals: ReferralPost): Observable<HttpResponse<ReferralSuccess>> {
    return this.http.post<HttpResponse<ReferralSuccess>>(`api/registrations/${registrationId}/referrals`, referrals, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  // DELETE api/registrations/<id>/referral/<id>
  deactivateReferral(registrationId: string, referralId: string): Observable<HttpResponse<any>> {
    return this.http.delete<HttpResponse<any>>(`api/registrations/${registrationId}/referrals/${referralId}`, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  // GET api/registrations/<id>/referral/<id>
  getReferralById(registrationId: string, referralId: string): Observable<Referral> {
    return this.http.get<Referral>(`api/registrations/${registrationId}/referrals/${referralId}`, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError),
      );
  }

}
