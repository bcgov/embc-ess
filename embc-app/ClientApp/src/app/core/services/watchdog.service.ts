import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { SessionExpiringModalComponent } from 'src/app/shared/modals/session-expiring/session-expiring.component';
import * as _ from 'lodash';

// defaults (used for non-session users, ie, evacuees)
const DEFAULT_WARNING_IN_SECONDS = 5 * 60;
const DEFAULT_WARNING_DURATION_IN_SECONDS = 2 * 60;

export enum RefreshReason {
  ResponseOk,
  LoginChange,
  UserAction
}

@Injectable({
  providedIn: 'root'
})
export class WatchdogService {

  private sessionWatchdogTimer: NodeJS.Timer = null;
  private sessionExpiringModal: NgbModalRef = null;
  private attachedEventListener: EventListenerOrEventListenerObject = null;

  constructor(
    private router: Router,
    protected auth: AuthService,
    private modals: NgbModal,
  ) {
    // watch for login changes
    this.auth.user.subscribe(user => {
      if (user) {
        // don't watch user actions if logged in
        this.unwatchUserActions();
      } else {
        // watch user actions if not logged in (ie, evacuees)
        this.watchUserActions();
      }
      this.refreshWatchdog(RefreshReason.LoginChange);
    });
  }

  private watchUserActions() {
    if (!this.attachedEventListener) {
      this.attachedEventListener = this.refreshWatchdog.bind(null, RefreshReason.UserAction);
      for (const type of ['keydown', 'click', 'mousemove', 'wheel']) {
        document.body.addEventListener(type, this.attachedEventListener);
      }
    }
  }

  private unwatchUserActions() {
    if (this.attachedEventListener) {
      for (const type of ['keydown', 'click', 'mousemove', 'wheel']) {
        document.body.removeEventListener(type, this.attachedEventListener);
      }
      this.attachedEventListener = null;
    }
  }

  // NB: debounced function executes when 1000ms have elapsed since last call
  // tslint:disable-next-line:member-ordering
  public refreshWatchdog = _.debounce((reason: RefreshReason) => {
    // ignore all calls if modal is already open
    if (this.sessionExpiringModal) { return; }

    // clear previous timer
    this.clearWatchdog();

    // don't time out if we're on the home page but not logged in
    if (this.router.url === '/' && !this.auth.isLoggedIn) { return; }

    // don't time out if we're on the session-expired page
    if (this.router.url === '/session-expired') { return; }

    const timeoutWarningInSeconds = this.auth.currentUser
      ? (this.auth.currentUser.clientTimeoutWarningInMinutes * 60)
      : DEFAULT_WARNING_IN_SECONDS;
    const timeoutWarningDurationInSeconds = this.auth.currentUser
      ? (this.auth.currentUser.clientTimeoutWarningDurationInMinutes * 60)
      : DEFAULT_WARNING_DURATION_IN_SECONDS;

    // start a new session watchdog timer
    this.sessionWatchdogTimer = setTimeout(() => {
      this.sessionWatchdogTimer = null;
      this.openModal(timeoutWarningDurationInSeconds);
    }, timeoutWarningInSeconds * 1000);
  }, 1000);

  private clearWatchdog() {
    if (this.sessionWatchdogTimer) {
      clearTimeout(this.sessionWatchdogTimer);
      this.sessionWatchdogTimer = null;
    }
  }

  private openModal(durationInSeconds: number) {
    this.sessionExpiringModal = this.modals.open(SessionExpiringModalComponent,
      { backdrop: 'static', keyboard: false, centered: true });
    this.sessionExpiringModal.componentInstance.durationInSeconds = durationInSeconds;

    // handle result
    this.sessionExpiringModal.result.then(() => {
      // clear for next time
      this.sessionExpiringModal = null;

      // reload user to refresh the session and session watchdog timer
      this.auth.login(true).subscribe();
    }, () => {
      // clear for next time
      this.sessionExpiringModal = null;

      // perform auto-logout
      this.auth.logout(true).subscribe();

      // redirect to session expired page
      this.router.navigateByUrl('/session-expired');
    });
  }

}
