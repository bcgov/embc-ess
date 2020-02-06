import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { AppState } from 'app/store';
import { CookieService } from './cookie.service';

@Injectable()
export abstract class RestService {

  constructor(
    protected http: HttpClient,
    protected store: Store<AppState>,
    protected cookieService: CookieService,
  ) { }

  get headers(): HttpHeaders {
    return new HttpHeaders({ 'Content-Type': 'application/json' });
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
