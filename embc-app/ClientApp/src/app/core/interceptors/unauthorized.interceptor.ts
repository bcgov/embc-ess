import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
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
      map(event => event),
      // handle errors
      catchError((error: HttpErrorResponse) => {
        // check if error is "not logged in"
        if (error.status === 401 && (this.auth.isLoggedIn || this.router.url !== '/')) {
          // perform logout
          this.auth.logout(true).subscribe();

          // redirect to session expired page
          this.router.navigateByUrl('/session-expired');
        }
        return throwError(error);
      })
    );
  }

}
