import { Injectable } from '@angular/core';
import { of, Observable } from 'rxjs';
import { IncidentTask } from '../models';
import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { SearchQueryParameters } from 'src/app/shared/components/search';
import { retry, catchError } from 'rxjs/operators';
import { MetaIncidentTask } from '../models/meta-incident-task';
import { HttpResponse } from '@angular/common/http';

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
    return this.http.get<IncidentTask>('api/incidenttasks/' + id, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
  createIncidentTask(data: IncidentTask): Observable<IncidentTask> {
    return this.http.post<IncidentTask>('api/incidenttasks/', data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
  updateIncidentTask(data: IncidentTask): Observable<HttpResponse<any>> {
    return this.http.put<HttpResponse<any>>('api/incidenttasks/' + data.id, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
}
