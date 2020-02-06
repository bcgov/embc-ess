import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';

import { AppState } from '../store';
import * as RegistrationActions from 'app/store/registration/registration.actions';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  window = window;
  busy: Subscription;
  browserIE = false;

  constructor(
    private store: Store<AppState>,
    private router: Router,
  ) {
    this.browserIE = /msie\s|trident\//i.test(window.navigator.userAgent);
  }

  ngOnInit() { }

  newRegistration(): void {
    this.store.dispatch(new RegistrationActions.ClearCurrentRegistration());
    this.router.navigate(['/self-registration']);
  }
}
