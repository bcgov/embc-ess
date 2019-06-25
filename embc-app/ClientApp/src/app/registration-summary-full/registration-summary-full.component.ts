import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { Registration, Address, isBcAddress } from 'src/app/core/models';
import { GENDER_OPTIONS, INSURANCE_OPTIONS } from '../constants';
import { UniqueKeyService } from '../core/services/unique-key.service';
import { AuthService } from '../core/services/auth.service';
import { NotificationQueueService } from '../core/services/notification-queue.service';

@Component({
  templateUrl: './registration-summary-full.component.html',
  styleUrls: ['./registration-summary-full.component.scss']
})
export class RegistrationSummaryFullComponent implements OnInit {

  registration: Registration = null; // local copy of the application state
  path: string = null; // the base path for routing
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private notificationQueueService: NotificationQueueService,
  ) { }

  ngOnInit() {
    // get the path for routing
    this.authService.path.subscribe((path: string) => this.path = path);

    // get URL param
    const reason = this.route.snapshot.paramMap.get('reason');

    // get lookup key and load registration data
    const key = this.uniqueKeyService.getKey();
    if (key) {
      this.registrationService.getRegistrationById(key, reason)
        .subscribe((registration: Registration) => {
          this.loading = false;

          if (!registration.id || !registration.essFileNumber) {
            console.log('ERROR - invalid registration object = ', registration);
            // Done with the key. It was useless. Clear it.
            this.uniqueKeyService.clearKey();

            this.goHome();
          } else {
            // reverse the flags patch (Client didn't prioritize system-wide data fix.)
            // registration.requiresAccommodation = !registration.requiresAccommodation;
            // registration.requiresClothing = !registration.requiresClothing;
            // registration.requiresFood = !registration.requiresFood;
            // registration.requiresIncidentals = !registration.requiresIncidentals;
            // registration.requiresTransportation = !registration.requiresTransportation;
            // store the registration object
            this.registration = registration;
          }
        }, err => {
          this.loading = false;

          this.notificationQueueService.addNotification('Failed to load evacuee', 'danger');
          console.log('error getting registration =', err);
          this.goHome();
        });
    } else {
      // key was not found
      this.loading = false;
      this.goHome();
    }
  }

  private goHome() {
    // go to home page
    this.router.navigate([`/${this.path}`]);
  }



  routeTo() {
    // save registration ID for lookup in the new component
    this.uniqueKeyService.setKey(this.registration.id);

    // go to registration maker
    this.router.navigate([`/${this.path}/registration`]);
  }

}
