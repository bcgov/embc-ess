import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { RestService } from './rest.service';
import { User } from '../models';
import { EVERYONE, VOLUNTEER, LOCAL_AUTHORITY, PROVINCIAL_ADMIN } from 'src/app/constants';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends RestService {

  public user: BehaviorSubject<User> = new BehaviorSubject<User>(null);
  public path: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public role: BehaviorSubject<string> = new BehaviorSubject<string>(EVERYONE);

  public isEveryone$ = this.role.pipe(map(x => x === EVERYONE));
  public isVolunteer$ = this.role.pipe(map(x => x === VOLUNTEER));
  public isLocalAuthority$ = this.role.pipe(map(x => x === LOCAL_AUTHORITY));
  public isProvincialAdmin$ = this.role.pipe(map(x => x === PROVINCIAL_ADMIN));

  // // whether the user is currently logged in.
  // isLoggedIn$ = new BehaviorSubject<boolean>(false);

  // event that is fired when user logs in
  public userLogOutEvent$ = new Subject<boolean>();

  public currentUser: User; // local copy of user profile

  get isLoggedIn(): boolean {
    return !!this.currentUser;
  }

  get isNewUser(): boolean {
    return this.currentUser && this.currentUser.isNewUser === true;
  }

  get isReturningUser(): boolean {
    return this.currentUser && this.currentUser.isNewUser === false;
  }

  public login(force: boolean = false): Observable<void> {
    const done = new Subject<void>();

    // log into back end service and set user information
    this.getCurrentUser(force).subscribe((user: User) => {
      this.setCurrentUser(user);
      done.next();
    },
      err => console.log(err)
    );

    return done.asObservable();
  }

  public logout(force: boolean = false): Observable<void> {
    const wasLoggedIn = this.isLoggedIn;

    // remove all saved data from session storage
    sessionStorage.clear();
    
    // clear all cookies
    this.cookieService.clear();
    
    if (wasLoggedIn) {
      this.userLogOutEvent$.next(true);
      document.location.href = '/logout';
    }

    return of();
  }

  public getCurrentUser(force: boolean = false): Observable<User> {
    // check local cache first
    if (!force && this.currentUser) {
      return of(this.currentUser);
    }

    return this.http.get<User>('/api/users/current', { headers: this.headers })
      .pipe(
        catchError(() => of<User>(null)) // ignore errors; i.e. 401 error means "no current user available/logged in"
      );
  }

  // sets current user to a value to test the current user observable all subscribers should update
  private setCurrentUser(user: User): void {
    // this.currentUser$.next(user);
    this.currentUser = user;
    // update the behaviour subjects
    this.updateBS(user);
  }

  private updateBS(user?: User) {
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
      role = EVERYONE;
    }

    // save the role
    this.role.next(role);
    // set the routing path
    this.setPath();
  }

  setPath() {
    // set the correct path for routing the user in the application
    switch (this.role.getValue()) {
      case PROVINCIAL_ADMIN:
        this.path.next('provincial-admin');
        break;
      case LOCAL_AUTHORITY:
        this.path.next('local-authority');
        break;
      case VOLUNTEER:
        this.path.next('volunteer');
        break;
      default:
        this.path.next('');
    }
  }

}
