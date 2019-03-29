import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { CurrentUserService } from './current-user.service';
import { User } from '../models';

@Injectable({
  providedIn: 'root'
})
export class RoleGuardService implements CanActivate {
  userIsAuthenticated = true;

  constructor(
    public router: Router,
    public currentUserService: CurrentUserService,
  ) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    // If the user state exists and it matches the link we return true
    // else we return false
    const expectedRole: string = route.data.expectedRole;
    // const token = localStorage.getItem('token'); // not doing this yet.
    const currentUser: User = this.currentUserService.getCurrentUser();

    if (!this.userIsAuthenticated || currentUser.role !== expectedRole) {
      // 
      this.router.navigate(['login']); // TODO: Is this the real login place???
      return false;
    } else {
      return true;
    }

    /**
    Todo: list of routes and who should be able to access them
    */

  }
}
