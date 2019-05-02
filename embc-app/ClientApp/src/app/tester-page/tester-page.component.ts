import { Component, OnInit } from '@angular/core';
import { NotificationQueueService } from '../core/services/notification-queue.service';
import { Referral } from 'src/app/core/models';

@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {
  referral: Referral = null;
  page = 1;

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
}
