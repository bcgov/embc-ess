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
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private auth: AuthService
  ) { }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // handle any http request and if there is a 401 error AND the user is logged in we assume that there is a timeout.
    return next.handle(req)
      .pipe(
        catchError(err => {
          alert('You have intercepted a 401');
          if (err instanceof HttpErrorResponse && err.status === 401 && this.auth.isLoggedIn) {
            // navigate to login page
            this.router.navigateByUrl('/session-expired');
            return of(err as any);
          }
          throw err;
        })
      );
  }
}
