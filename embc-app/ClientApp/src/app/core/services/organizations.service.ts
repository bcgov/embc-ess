import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

import { CoreModule } from '../core.module';
import { Organization, ListResult } from '../models';
import { RestService } from './rest.service';
import { HttpResponse } from '@angular/common/http';
import { SearchQueryParameters } from 'src/app/shared/components/search';

@Injectable({
  providedIn: CoreModule
})
export class OrganizationsService extends RestService {
  apiRoute = 'api/organizations';

  getOrganizations(limit?: number, offset?: number, q?: string, sort?: string): Observable<ListResult<Organization>> {
    const params = {
      limit: (limit || 100).toString(), // query params are strings
      offset: (offset || 0).toString(),
      q: q || '',
      sort: sort || 'name'
    };

    // get a list of all organizations back from the api
    return this.http.get<ListResult<Organization>>(this.apiRoute, { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  getOrganizationById(id: string): Observable<Organization> {
    // get a single org by their bceidAccountNumber
    return this.http.get<Organization>(this.apiRoute + '/' + id, { headers: this.headers })
      .pipe(
        catchError(this.handleError)
      );
  }

  createOrganization(data: Organization): Observable<Organization> {
    // this will return a response string of 200. This may need to become a Response eventually
    // return of('200');
    return this.http.post<Organization>(this.apiRoute, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  updateOrganization(data: Organization): Observable<HttpResponse<any>> {
    // this will return a response string of 200. This may need to become a Response eventually
    // return of('200');
    return this.http.put<HttpResponse<any>>(this.apiRoute + '/' + data.id, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
}
