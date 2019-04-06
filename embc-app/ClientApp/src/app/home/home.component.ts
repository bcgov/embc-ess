import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';

import { AppState } from '../store';
import * as RegistrationActions from 'src/app/store/registration/registration.actions';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  window = window;
  busy: Subscription;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private authService: AuthService,
  ) { }

  ngOnInit() {
  }

  newRegistration(): void {
    this.store.dispatch(new RegistrationActions.InitializeCurrentRegistration());
    this.router.navigate(['/self-registration']);
  }
}
