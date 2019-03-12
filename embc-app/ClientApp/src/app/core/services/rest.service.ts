import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { map, catchError, retry } from 'rxjs/operators';
import { Store } from '@ngrx/store';

import { AppState } from 'src/app/store';
import { httpGet } from './mock-api';  // FIXME: <-- this will GO AWAY when backend is built

const headers = new HttpHeaders({
  'Content-Type': 'application/json'
});

@Injectable()
export abstract class RestService {

  // FIXME: change this when API is live!
  protected baseUrl = 'https://embcess-dev.pathfinder.gov.bc.ca/embcess/api';

  constructor(
    protected http: HttpClient,
    protected store: Store<AppState>,
  ) { }

  protected get(relativeUrl: string = '/', options?): Observable<any> {
    // TODO: Something like this...
    // return this.http.get(this.baseUrl + relativeUrl, new RequestOptions({headers: this.headers})).map(res => res.json());
    return httpGet(`${this.baseUrl}${relativeUrl}`, options)
      .pipe(
        retry(1),
        map(this.extractData),
        catchError(this.handleError)
      );
  }

  protected post(relativeUrl: string = '/', body: any | null, options?): Observable<any> {
    return this.http.post(`${this.baseUrl}${relativeUrl}`, body, { ...options, headers })
      .pipe(
        map(this.extractData),
        catchError(this.handleError)
      );
  }

  protected put(relativeUrl: string = '/', body: any | null, options?): Observable<any> {
    // TODO: Implement
    return throwError('Method not implemented');
  }

  protected patch(relativeUrl: string = '/', body: any | null, options?): Observable<any> {
    // TODO: Implement
    return throwError('Method not implemented');
  }

  protected delete(relativeUrl: string = '/', options?): Observable<any> {
    // TODO: Implement
    return throwError('Method not implemented');
  }

  // Process backend responses
  protected extractData(payload: any) {
    const { data, error } = payload;
    // Process backend errors if they exist
    if (error) {
      throw new Error(error.message);
    }
    // Otherwise
    return data || {};
  }

  // Error handling
  protected handleError(err): Observable<never> {
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = err.error.message;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Backend returned code ${err.status}, body was: ${err.message}`;
    }
    return throwError(errorMessage);
  }
}
