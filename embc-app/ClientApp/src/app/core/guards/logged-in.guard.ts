import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthService } from '../services/auth.service';

/**
 * Guard that requires a logged in user before proceeding to load a page.
 */
@Injectable({
  providedIn: 'root'
})
export class LoggedInGuard implements CanActivate, CanActivateChild {
  constructor(
    private router: Router,
    private authService: AuthService,
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.checkLoggedIn(state.url);
  }

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.canActivate(route, state);
  }

  checkLoggedIn(url: string): Observable<boolean> {
    return this.authService.isLoggedIn$
      .pipe(
        map(logged => {
          if (logged) {
            return true;
          }

          // navigate to login page
          this.router.navigateByUrl('/external/login', { skipLocationChange: true });

          // this.router.navigateByUrl('/login');
          return false;
        })
      );
  }
}
