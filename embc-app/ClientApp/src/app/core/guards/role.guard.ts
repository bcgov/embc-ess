import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AuthService } from '../services/auth.service';
import { User } from '../models';

/**
 * Implements Role-based guarding.
 * Guard that requires the current user to have an specific role before proceeding to load a page.
 */
@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(
    public router: Router,
    public authService: AuthService,
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    // If the user state exists and it matches the link we return true
    // else we return false
    const expectedRole: string = route.data.expectedRole;
    return this.authService.getCurrentUser().pipe(map(user => this.checkAuthorization(user, expectedRole)));

    // TODO: list of routes and who should be able to access them
  }

  // determines if user has a matching role
  checkAuthorization(user: User, expectedRole: string): boolean {
    if (!user || !expectedRole) {
      this.router.navigate(['/404']);
      return false;
    }

    const roles = user.appRoles || [];
    if (roles.indexOf(expectedRole) !== -1) {
      return true;
    }

    // role not authorized so redirect to home page
    this.router.navigate(['/404']);
    return false;
  }
}
