import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { User } from '../models';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {
  // this is the current user object
  currentUser: BehaviorSubject<User> = new BehaviorSubject(null);

  constructor() { }

  setCurrentUser(user: User) {
    // this sets a current user to a value to test the current user observable all subscribers should update.
    this.currentUser.next(user);
  }
  getCurrentUser(): User {
    // shouldn't have to use this but it is to do a one-off look at the user
    return this.currentUser.getValue();
  }
}
