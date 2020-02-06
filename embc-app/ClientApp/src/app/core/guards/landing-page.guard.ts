import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthService } from '../services/auth.service';
import { User } from '../models';
import { normalize } from 'app/shared/utils';
import { PROVINCIAL_ADMIN, LOCAL_AUTHORITY, VOLUNTEER } from 'app/constants';

/**
 */
@Injectable({
  providedIn: 'root'
})
export class LandingPageGuard implements CanActivate {
  constructor(
    public router: Router,
    public authService: AuthService,
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    // If the user state exists and it matches the link we return true
    // else we return false
    const navigateByRole: object = route.data.navigateByRole;

    return this.authService.getCurrentUser()
      .pipe(
        map(user => {
          if (!user || !navigateByRole) {
            window.location.replace('/login');
            return false;
          }

          const hash = {};
          const roles = user.appRoles || [];
          roles.forEach(s => hash[s] = true);

          if (!!hash[PROVINCIAL_ADMIN]) {
            this.router.navigate([navigateByRole[PROVINCIAL_ADMIN]]);
            return false;
          }

          if (!!hash[LOCAL_AUTHORITY]) {
            this.router.navigate([navigateByRole[LOCAL_AUTHORITY]]);
            return false;
          }

          if (!!hash[VOLUNTEER]) {
            this.router.navigate([navigateByRole[VOLUNTEER]]);
            return false;
          }

          this.router.navigate(['/404']);
          return false;
        })
      );
  }
}
