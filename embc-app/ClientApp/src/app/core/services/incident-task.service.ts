import { Injectable } from '@angular/core';
import { of, Observable } from 'rxjs';
import { IncidentTask } from '../models';
import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { SearchQueryParameters } from 'src/app/shared/components/search';
import { retry, catchError } from 'rxjs/operators';
import { MetaIncidentTask } from '../models/meta-incident-task';

@Injectable({
  providedIn: CoreModule
})
export class IncidentTaskService extends RestService {
  getIncidentTasks(props: SearchQueryParameters = {}): Observable<MetaIncidentTask> {
    // return a list of all incident task numbers for use in the application
    // return of(INCIDENTTASKS);
    const { limit = 100, offset = 0, q, sort } = props;
    const params = {
      limit: limit.toString(), // query params are strings
      offset: offset.toString(),
      q,
      sort
    };
    return this.http.get<MetaIncidentTask>('api/incidenttasks', { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  getIncident(id: string): Observable<IncidentTask> {
    // return a single matching incident
    return of(null);
  }
}
