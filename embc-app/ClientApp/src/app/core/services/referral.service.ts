import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { CoreModule } from '../core.module';
import { Referral, ListResult } from '../models';
import { RestService } from './rest.service';
import { SearchQueryParameters } from '../models/search-interfaces';

@Injectable({
  providedIn: CoreModule
})
export class ReferralService extends RestService {

  // GET api/registrations/<id>/referrals
  getReferrals(props: SearchQueryParameters = {}): Observable<ListResult<Referral>> {
    const { limit = 100, offset = 0, q = '', sort = '' } = props;
    const params = {
      limit: limit.toString(), // query params are strings
      offset: offset.toString(),
      q,
      sort
    };
    return this.http.get<ListResult<Referral>>('api/referrals', { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  // POST api/registrations/<id>/referrals ???
  // TODO: send array
  printReferrals(data: Referral): Observable<Referral> {
    return this.http.post<Referral>('api/referrals/', data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  // POST api/registrations/<id>/assessment ???
  // TODO: send array
  createReferral(data: Referral): Observable<HttpResponse<any>> {
    return this.http.put<HttpResponse<any>>(`api/referrals/${data.id}`, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  deactivateReferral(data: Referral): Observable<HttpResponse<any>> {
    return this.http.delete<HttpResponse<any>>(`api/referrals/${data.id}`, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  // GET api/registrations/<id>/referrals/<id>
  getReferralById(id: string): Observable<Referral> {
    return this.http.get<Referral>(`api/referrals/${id}`, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError),
      );
  }

}
