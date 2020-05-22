import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppState } from '../store';
import * as RegistrationActions from 'src/app/store/registration/registration.actions';
import { LogInEnvironmentComponent } from '../shared/modals/log-in-environment/log-in-environment.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  window = window;
  busy: Subscription;
  browserIE = false;

    // Modal for environment confirmation modal
    private envModal: NgbModalRef = null;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private modals: NgbModal
  ) {
    this.browserIE = /msie\s|trident\//i.test(window.navigator.userAgent);
  }

  ngOnInit() { }

  newRegistration(): void {
    this.store.dispatch(new RegistrationActions.ClearCurrentRegistration());
    this.router.navigate(['/self-registration']);
  }

  openModal() {
    if (!this.envModal) {
      this.envModal = this.modals.open(LogInEnvironmentComponent, { size: 'lg', centered: true });
      this.envModal.result.then(
        () => { 
            this.envModal = null; 
        },
        () => { this.envModal = null; }
      );
    }
  }
}
