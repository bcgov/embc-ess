import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { RestService } from './rest.service';
import { User } from '../models';
import { doesNotThrow } from 'assert';
import { HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends RestService {

  // this is the current user object
  user: BehaviorSubject<User> = new BehaviorSubject<User>(null);
  public role: BehaviorSubject<string> = new BehaviorSubject<string>('role_everyone');

  // // whether the user is currently logged in.
  // isLoggedIn$ = new BehaviorSubject<boolean>(false);

  currentUser: User = null; // local copy of user profile

  get isLoggedIn(): boolean {
    return !!this.currentUser;
  }

  login(): Observable<void> {
    const done = new Subject<void>();

    // Code here would log into a back end service and return user information
    this.getCurrentUser().subscribe(user => {
      this.setCurrentUser(user);
      done.next();
    });

    return done;
  }

  logout(callServer = false): Observable<void> {
    this.setCurrentUser(null);
    if (callServer) {
      return this.http.get<void>('logout', { headers: this.headers });
    }
    return of();
  }

  getCurrentUser(): Observable<User> {
    // check local cache first
    if (this.currentUser) {
      return of(this.currentUser);

    }

    return this.http.get<User>('api/users/current', { headers: this.headers })
      .pipe(
        tap(user => {
          // if the response worked we should move the contents to a behaviorSubject
          if (user instanceof HttpResponse) {
            this.updateBS(user);
          }
        }),
        catchError(() => of<User>(null))); // ignore errors; i.e. 401 error means "no current user available/logged in"
  }

  // this sets a current user to a value to test the current user observable all subscribers should update.
  setCurrentUser(user: User): void {
    // this.currentUser$.next(user);
    this.currentUser = user;
    // update the behavioursubject
    this.updateBS(user);
  }

  updateBS(user?: User) {
    // save the user
    this.user.next(user);

    let role: string;
    // check that the user exists and has roles
    if (user !== null && user.appRoles.length > 0) {
      // handle the role and save the user
      // if there are elements in the array
      // the most privileged comes last
      role = user.appRoles[user.appRoles.length - 1];
    } else {
      role = 'role_everyone';
    }
    // save the role
    this.role.next(role);
  }
  // setLoggedIn(value: boolean): void {
  //   this.isLoggedIn$.next(value);
  // }

}
