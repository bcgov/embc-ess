import { Component, OnInit } from '@angular/core';
import { debounceTime } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-notification-banner',
  templateUrl: './notification-banner.component.html',
  styleUrls: ['./notification-banner.component.scss'],
})
export class NotificationBannerComponent implements OnInit {

  staticAlertClosed = false;
  successMessage: string;

  notifications = ['qwer', 'tyui', 'asdf'];
  ngOnInit(): void {
    // setTimeout(() => this.staticAlertClosed = true, 20000);

    // this._success.subscribe((message) => this.successMessage = message);
    // this._success.pipe(
    //   debounceTime(5000)
    // ).subscribe(() => this.successMessage = null);
  }

  addAThing() {
    this.notifications.push("Thing");
  }
  close(a: string) {
    if (this.notifications.indexOf(a) !== -1) {
      this.notifications.splice(this.notifications.indexOf(a), 1);
    }
  }
}
