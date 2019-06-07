import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { SessionExpiringModalComponent } from 'src/app/shared/modals/session-expiring/session-expiring.component';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { User, Config } from 'src/app/core/models';
import debounce from 'lodash/debounce';

// defaults (in case of empty config)
const DEFAULT_WARNING_IN_MINUTES = 5;
const DEFAULT_WARNING_DURATION_IN_MINUTES = 2;

export enum RefreshReason {
  ResponseOk,
  LoginChange,
  UserAction
}

@Injectable({
  providedIn: 'root'
})
export class WatchdogService {

  // the watcdog timer is a timer that counts up since last interaction with the server
  private sessionWatchdogTimer: number = null;
  // the modal popup object
  private sessionExpiringModal: NgbModalRef = null;
  // a listener for an event in the DOM
  private attachedEventListener: EventListenerOrEventListenerObject = null;
  // The configuration that holds timer information. (as well as other information about the application)
  private config: Config = null;

  constructor(
    private router: Router,
    protected authService: AuthService,
    private modals: NgbModal,
    private store: Store<AppState>, // ngrx app state
  ) {
    // watch for login changes
    this.authService.user.subscribe((user: User) => {
      if (user) {
        // don't watch user actions if logged in
        this.unwatchUserActions();
      } else {
        // watch user actions if not logged in (ie, evacuees)
        this.watchUserActions();
      }
      this.refreshWatchdog(RefreshReason.LoginChange);
    });

    // get config so that we can use timing
    this.store.select(s => s.lookups.config.config).subscribe((config: Config) => {
      this.config = config;
    });

    // initialize the watchdog and start the timer
    this.refreshWatchdog(RefreshReason.LoginChange);
  }

  private watchUserActions() {
    // No event attached listener watching user behaviour? Make one.
    if (!this.attachedEventListener) {
      this.attachedEventListener = this.refreshWatchdog.bind(null, RefreshReason.UserAction);
      for (const type of ['keydown', 'click', 'mousemove', 'wheel']) {
        document.body.addEventListener(type, this.attachedEventListener);
      }
    }
  }

  private unwatchUserActions() {
    // If there is an event listener watching for user action remove it.
    if (this.attachedEventListener) {
      for (const type of ['keydown', 'click', 'mousemove', 'wheel']) {
        document.body.removeEventListener(type, this.attachedEventListener);
      }
      this.attachedEventListener = null;
    }
  }

  // NB: debounced function executes when 1000ms have elapsed since last call
  // tslint:disable-next-line:member-ordering
  public refreshWatchdog = debounce((reason: RefreshReason) => {
    // console.log(reason);

    // ignore all refreshes if modal is already open
    if (this.sessionExpiringModal) { return; }

    // clear previous timer
    this.clearWatchdog();

    // don't time out if we're on the home page but not logged in
    if (this.router.url === '/' && !this.authService.isLoggedIn) { return; }

    // don't time out if we're on the session-expired page
    if (this.router.url === '/session-expired') { return; }

    const timeoutWarningInMinutes = this.config ?
      (this.authService.currentUser ? this.config.clientTimeoutWarningInMinutes : this.config.defaultTimeoutWarningInMinutes)
      : DEFAULT_WARNING_IN_MINUTES;
    const timeoutWarningDurationInMinutes = this.config ?
      (this.authService.currentUser ? this.config.clientTimeoutWarningDurationInMinutes : this.config.defaultWarningDurationInMinutes)
      : DEFAULT_WARNING_DURATION_IN_MINUTES;

    // start a new session watchdog timer
    this.sessionWatchdogTimer = window.setTimeout(() => {
      this.sessionWatchdogTimer = null;
      this.openModal(timeoutWarningDurationInMinutes * 60);
    }, timeoutWarningInMinutes * 60 * 1000);
  }, 1000);

  private clearWatchdog() {
    // if there is a timer null it
    if (this.sessionWatchdogTimer) {
      clearTimeout(this.sessionWatchdogTimer);
      this.sessionWatchdogTimer = null;
    }
  }

  private openModal(durationInSeconds: number) {
    // open a SessionExpiringModalComponent modal with the duration of the countdown timer contained inside of it.
    this.sessionExpiringModal = this.modals.open(SessionExpiringModalComponent, { backdrop: 'static', keyboard: false });
    this.sessionExpiringModal.componentInstance.durationInSeconds = durationInSeconds;

    // handle result
    this.sessionExpiringModal.result.then(() => {
      // CASE: user has clicked in modal box to stay logged in

      // clear the modal for next time it is called
      this.sessionExpiringModal = null;

      // reload user to refresh the session and session watchdog timer
      this.authService.login(true).subscribe();
    }, () => {
      // CASE: user has let the modal box expire
      // clear for next time
      this.sessionExpiringModal = null;

      // perform auto-logout
      this.authService.logout(true).subscribe();

      // redirect to session expired page
      this.router.navigateByUrl('/session-expired');
    });
  }

}
