import { Injectable } from '@angular/core';
import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { Observable } from 'rxjs';
import { ListResult, Evacuee } from '../models';
import { catchError, retry } from 'rxjs/operators';

import { EvacueeSearchQueryParameters } from '../models/search-interfaces';

type StringParams = {
  [P in keyof EvacueeSearchQueryParameters]?: string;
};

@Injectable({
  providedIn: CoreModule
})
export class EvacueeService extends RestService {

  getEvacuees(props: EvacueeSearchQueryParameters = {}): Observable<ListResult<Evacuee>> {
    // get the results
    const params = this.propsToParams(props);
    return this.http.get<ListResult<Evacuee>>('api/evacuees', { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  // convert everything to string. Query params are strings.
  private propsToParams(props: EvacueeSearchQueryParameters = {}): StringParams {
    // establish parameters
    const { limit = 100, offset = 0, } = props;
    const params: StringParams = {
      limit: limit.toString(), // query params are strings
      offset: offset.toString(),
    };

    // basic search stuff
    if (props.q) { params.q = props.q; }
    if (props.sort) { params.sort = props.sort; }

    // advanced search stuff
    if (props.last_name) { params.last_name = props.last_name; }
    if (props.first_name) { params.first_name = props.first_name; }
    if (props.task_no) { params.task_no = props.task_no; }
    if (props.ess_file_no) { params.ess_file_no = props.ess_file_no; }
    if (props.evacuated_from) { params.evacuated_from = props.evacuated_from; }
    if (props.evacuated_to) { params.evacuated_to = props.evacuated_to; }
    // convert boolean values (only if present)
    if (props.registration_completed === true || props.registration_completed === false) { params.registration_completed = props.registration_completed.toString(); }
    if (props.referrals_provided === true || props.referrals_provided === false) { params.referrals_provided = props.referrals_provided.toString(); }

    return params;
  }
}
