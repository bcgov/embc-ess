import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, retry, map } from 'rxjs/operators';

import { CoreModule } from '../core.module';
import { Volunteer, ListResult } from '../models';
import { RestService } from './rest.service';
import { HttpResponse, HttpErrorResponse, HttpClient } from '@angular/common/http';
import { SearchQueryParameters } from '../models/search-interfaces';
import { VolunteerTask } from '../models/volunteer-task.model';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { CookieService } from './cookie.service';
import * as VolunteerTaskActions from 'src/app/store/volunteer-task/volunteer-task.actions';
import { WatchdogService } from './watchdog.service';


@Injectable({
  providedIn: CoreModule
})
export class VolunteerTaskService extends RestService {

  apiRoute = '/api/volunteer-task';

  /**
   *
   */
  constructor(protected http: HttpClient,
    protected store: Store<AppState>,
    private watchDogService: WatchdogService,
    protected cookieService: CookieService, ) {
    super(http, store, cookieService);
      this.loadVolunteerTask();
      // this.watchDogService.sessionRefresed
      // .subscribe(() => {
      //   this.loadVolunteerTask();
      // })
  }

  loadVolunteerTask() {
    this.getActiveVolunteerTask()
    .subscribe(task => {
      if(task){
        this.setVolunteerTask(task.incidentTask.taskNumber)
        .subscribe();
      }
    });
  }

  setVolunteerTask(taskId: string): Observable<VolunteerTask> {
    // get a single volunteer by their bceidAccountNumber
    // return of(VOLUNTEERS[0]);
    return this.http.post<VolunteerTask>(this.apiRoute + '/task/' + taskId, null, { headers: this.headers })
      .pipe(map(result => {
        //  add task to store
        this.store.dispatch(new VolunteerTaskActions.SetCurrentVolunteerTask({ task: result.incidentTask.taskNumber }));
        return result;
      }))
      .pipe(
        catchError(this.handleError)
      );
  }

  getActiveVolunteerTask(): Observable<VolunteerTask> {
    return this.http.get<VolunteerTask>(this.apiRoute + '/active-task/', { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  createVolunteer(data: VolunteerTask): Observable<VolunteerTask> {
    // this will return a response string of 200. This may need to become a Response eventually
    // return of('200');
    return this.http.post<VolunteerTask>(this.apiRoute, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

  updateVolunteer(data: VolunteerTask): Observable<HttpResponse<any>> {
    // this will return a response string of 200. This may need to become a Response eventually
    // return of('200');
    return this.http.put<HttpResponse<any>>(this.apiRoute + '/' + data.id, data, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }

}
