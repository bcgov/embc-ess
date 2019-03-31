import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { RestService } from './rest.service';
import { User } from '../models';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends RestService {

  // this is the current user object
  currentUser$ = new BehaviorSubject<User>(null);

  // whether the user is currently logged in.
  isLoggedIn$ = new BehaviorSubject<boolean>(false);

  login(): void {
    // Code here would log into a back end service and return user information
    this.getCurrentUser()
      .pipe(
        tap(user => this.setCurrentUser(user)),
        map(user => !!user ? true : false), // if we got a user, then we are "logged in"
        tap(logged => this.setLoggedIn(logged))
      )
      .subscribe(); // make it go!
  }

  logout(): void {
    this.setCurrentUser(null);
    this.setLoggedIn(false);
  }

  getCurrentUser(): Observable<User> {
    return this.http.get<User>('api/users/current', { headers: this.headers })
      .pipe(catchError(() => of<User>(null))); // ignore errors; i.e. 401 error means "no current user available/logged in"
  }

  // this sets a current user to a value to test the current user observable all subscribers should update.
  setCurrentUser(user: User): void {
    this.currentUser$.next(user);
  }

  setLoggedIn(value: boolean): void {
    this.isLoggedIn$.next(value);
  }

}
