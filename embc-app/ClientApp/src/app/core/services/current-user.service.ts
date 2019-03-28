import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CurrentUser } from '../models/current-user.model';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {
  // this is the current user object
  currentUser: BehaviorSubject<CurrentUser> = new BehaviorSubject({ role: 'none' });

  constructor() { }

  setCurrentUser(user: CurrentUser) {
    // this sets a current user to a value to test the current user observable all subscribers should update.
    this.currentUser.next(user);
  }
  getCurrentUser(): CurrentUser {
    // shouldn't have to use this but it is to do a one-off look at the user
    return this.currentUser.getValue();
  }
}
