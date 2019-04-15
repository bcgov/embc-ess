import { Component, OnInit } from '@angular/core';
import { VolunteerService } from '../core/services/volunteer.service';
import { Volunteer, Registration, User } from '../core/models';
import { Store } from '@ngrx/store';
import { AppState } from '../store';
import { takeWhile } from 'rxjs/operators';
import { state } from '@angular/animations';
import { AuthService } from '../core/services/auth.service';
import { NotificationQueueService } from '../core/services/notification-queue.service';
// import { Store } from '@ngrx/store';

// import { IncidentTaskService } from '../core/services/incident-task.service';
// import { RegistrationService } from '../core/services/registration.service';
// import { UserDataService } from '../core/services/user-data.service';
// import { AppState } from '../store';
// import { Country, RegionalDistrict, Region, IncidentTask, Registration, User } from '../core/models';
// import { ControlledListService } from '../core/services/controlled-list.service';

@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {
  page = 1;

  constructor(
    private notifications: NotificationQueueService
  ) { }

  ngOnInit() { }
  notify(message: string) {
    this.notifications.addNotification(message);
  }
}
