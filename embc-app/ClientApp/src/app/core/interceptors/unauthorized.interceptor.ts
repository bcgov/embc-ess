import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, of, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private auth: AuthService
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // handle all of the requests
    return next.handle(request).pipe(
      // handle all of the events by mapping them
      map((event: HttpEvent<any>) => event),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && this.auth.isLoggedIn) {
          // if the error is a "not logged in" error then we should redirect them to the session expired page.
          this.router.navigateByUrl('/session-expired');
        }
        // build an error response and return it
        let data = {};
        data = {
          reason: error && error.error.reason ? error.error.reason : '',
          status: error.status
        };
        return throwError(error);
      })
    );
  }
}
