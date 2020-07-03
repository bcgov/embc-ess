import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';

import { IncidentTask, ListResult, OpenAndClosedTasksMetadata } from '../models';
import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { HttpResponse } from '@angular/common/http';
import { IncidentTaskSearchQueryParameters, SearchQueryParameters } from '../models/search-interfaces';

@Injectable({
  providedIn: CoreModule
})
export class IncidentTaskService extends RestService {
  getIncidentTasks(props: IncidentTaskSearchQueryParameters = {}): Observable<ListResult<IncidentTask>> {
    const { limit = 100, offset = 0, q = '', sort = '', activeTasks = '' } = props;
    const params = {
      limit: (limit || 100).toString(), // query params are strings
      offset: (offset || 0).toString(),
      q: q || '',
      sort: sort || '',
      activeTasks: (!!activeTasks).toString(),
    };
    return this.http.get<ListResult<IncidentTask>>('/api/incidenttasks', { headers: this.headers, params })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  getIncidentTask(id: string): Observable<IncidentTask> {
    // return a single matching incident
    return this.http.get<IncidentTask>('/api/incidenttasks/' + id, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  getOpenIncidentTasks(props: SearchQueryParameters = {}): Observable<ListResult<IncidentTask>> {
    const { limit = 100, offset = 0, q = '', sort = '' } = props;
    const params = {
      limit: (limit || 100).toString(), // query params are strings
      offset: (offset || 0).toString(),
      q: q || '',
      sort: sort || '',
    };
    // Returns all incident tasks that are open (e.g. EndDate is in the future)
    return this.http.get<ListResult<IncidentTask>>('/api/incidenttasks/getopenincidenttasks', {headers: this.headers, params})
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  createIncidentTask(data: IncidentTask): Observable<IncidentTask> {
    return this.http.post<IncidentTask>('/api/incidenttasks/', data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  updateIncidentTask(data: IncidentTask): Observable<HttpResponse<any>> {
    return this.http.put<HttpResponse<any>>('/api/incidenttasks/' + data.id, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  isTaskNumberUnique(taskNum: string): Observable<boolean> {
    return this.http.get<boolean>('/api/incidenttasks/getIsUniqueTaskNumber/' + taskNum, {headers: this.headers})
    .pipe(
      retry(3),
      catchError(this.handleError)
    );
  }

}
