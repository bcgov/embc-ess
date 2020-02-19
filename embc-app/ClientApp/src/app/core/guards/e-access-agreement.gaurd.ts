import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

import { AuthService } from '../services/auth.service';
import { VolunteerService } from '../services/volunteer.service';
import { User, Volunteer } from '../models';

/**
 * Guard that requires an accepted electronic access agreement before proceeding to load a page.
 */
@Injectable({
  providedIn: 'root'
})
export class EAccessAgreementGuard implements CanActivate, CanActivateChild {
  constructor(
    private router: Router,
    private authService: AuthService,
    private volunteerService: VolunteerService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.checkAccessAgreement(state.url);
  }

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.canActivate(route, state);
  }

  checkAccessAgreement(url: string): Observable<boolean> {
    return this.authService.getCurrentUser()
      .pipe(switchMap((user: User) => {
        if(user.userType === 'business'){
        return this.volunteerService.getVolunteerById(user.contactid)
          .pipe(map((volunter: Volunteer) => volunter.electronicAccessAgreementAccepted));
        } else {
          return of(true);
        }
      }))
      .pipe(map((agrementAccepted: boolean) => {
        if (agrementAccepted) {
          return true;
        } else {
          // navigate to electronic access agreement page
          this.router.navigateByUrl(`/e-access-agreement?redirectUrl=${encodeURIComponent(url)}`, { skipLocationChange: true });
          return false;
        }
      }));
  }

}
