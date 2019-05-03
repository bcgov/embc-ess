import { Component, OnInit } from '@angular/core';
import { NotificationQueueService } from '../core/services/notification-queue.service';
import { Registration } from 'src/app/core/models';
import { ReferralDate } from '../core/models/referral-date';

@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {
  registration: Registration = null;
  page = 1;
  dateStub: any;

  // inputDate: ReferralDate = { identifier: 'banana' };
  inputDate: ReferralDate = {
    uuid: 'banana',
    from: new Date(),
    days: 1
  };

  constructor(
    private notifications: NotificationQueueService
  ) { }

  ngOnInit() { }

  notify(message?: string, type?: string) {
    this.notifications.addNotification(message, 6000, type);
  }

  doRemove() {
    console.log('remove event');
  }

  doAdd() {
    console.log('add event');
  }

  onDate(event: string) {
    // for checking the object
    if (event) {
      this.dateStub = event;
    }
  }
}
