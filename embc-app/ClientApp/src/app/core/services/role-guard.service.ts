import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RoleGuardService implements CanActivate {

  constructor(
    public router: Router,
  ) { }

  canActivate(): boolean {
    // If the user state exists and it matches the link we return true
    // else we return false
    return true;
    /**
    Todo: list of routes and who should be able to access them
    */

  }
}
