import { Component, OnInit } from '@angular/core';
import { Notification, NotificationQueueService } from 'src/app/core/services/notification-queue.service';

@Component({
  selector: 'app-notification-banner',
  templateUrl: './notification-banner.component.html',
  styleUrls: ['./notification-banner.component.scss'],
})
export class NotificationBannerComponent implements OnInit {

  staticAlertClosed = false;
  successMessage: string;
  currentNotifications: Notification[];

  constructor(
    private notificationQueueService: NotificationQueueService,
  ) { }

  ngOnInit(): void {
    this.notificationQueueService.notificationQueue.subscribe((notifications: Notification[]) => {
      this.currentNotifications = notifications;
    });
  }

  close(notification: Notification) {
    // expire the notification
    this.notificationQueueService.expireNotification(notification.identifier);
  }

}
