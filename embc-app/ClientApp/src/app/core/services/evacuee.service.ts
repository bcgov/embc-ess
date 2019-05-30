import { Injectable } from '@angular/core';
import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { Observable } from 'rxjs';
import { ListResult, Evacuee } from '../models';
import { catchError, retry } from 'rxjs/operators';

import { SearchQueryParameters } from '../models/search-interfaces';

@Injectable({
  providedIn: CoreModule
})
export class EvacueeService extends RestService {

  getEvacuees(props: SearchQueryParameters = {}): Observable<ListResult<Evacuee>> {
    // establish parameters
    const { limit = 100, offset = 0, q = '', sort = '' } = props;
    const params = {
      limit: limit.toString(), // query params are strings
      offset: offset.toString(),
      q,
      sort
    };
    // get the results
    return this.http.get<ListResult<Evacuee>>('api/evacuees', { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
}
