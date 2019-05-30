import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface Notification {
  // this is the notification that is used in this component
  message: string;
  identifier: string; // whatever the user's browser stamps is fine
  type: string; // 'success' || 'warning' || 'danger'
}

@Injectable({
  providedIn: 'root'
})
export class NotificationQueueService {

  // the time it takes a message to expire
  defaultTimeout = 5000;

  // this is the data structure that holds a collection of notification objects
  public notificationQueue: BehaviorSubject<Notification[]> = new BehaviorSubject<Notification[]>([]);

  // the default notification is warning
  addNotification(message: string, type: string = 'warning', timeoutMs: number = this.defaultTimeout) {
    // Get old value of behaviour subject to update it.
    const currentNotifications: Notification[] = this.notificationQueue.getValue();

    // Create a new notification object to display and give it a uniqueish ID.
    const notification: Notification = { message, identifier: new Date().toString(), type };

    // add the notification into the notification array
    currentNotifications.push(notification);

    // replace the behavior subject with the updated one
    this.notificationQueue.next(currentNotifications);

    // set a timeout callback to expire the notification in a set amout of time.
    setTimeout(() => {
      // expire the notification by its timestamp
      this.expireNotification(notification.identifier);
    }, timeoutMs);

  }

  expireNotification(identifier: string) {
    // set the next notification queue to the updated filtered collection
    this.notificationQueue.next(this.notificationQueue.getValue().filter((m) => m.identifier !== identifier));
  }

}
