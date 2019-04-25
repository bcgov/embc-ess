import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../core/services/registration.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Registration } from '../core/models';
import { AuthService } from '../core/services/auth.service';
import { UniqueKeyService } from '../core/services/unique-key.service';

@Component({
  selector: 'app-registration-summary',
  templateUrl: './registration-summary.component.html',
  styleUrls: ['./registration-summary.component.scss']
})
export class RegistrationSummaryComponent implements OnInit {

  // collect ess file number from activated route
  registration: Registration;
  // all routing should be handled through the current path
  path: string;

  constructor(
    private router: Router,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe(p => this.path = p);

    // try to collect the ess file number and load it
    const key = this.uniqueKeyService.getKey();
    // if the value stored as the lookup key is falsy then we should reroute home
    if (key) {
      this.registrationService.getRegistrationById(key)
        .subscribe(r => {
          // if there is nothing useful returned route somewhere else.
          if (!r.essFileNumber) {
            // send them back to their home page
            this.router.navigate([`/${this.path}`]);
          } else {
            // Save the registration into the
            this.registration = r;
          }
        });
    } else {
      // send them back to their home page
      this.router.navigate([`/${this.path}`]);
    }
  }

  routeTo() {
    // TODO: this seems like bad practice but fix when we have time
    if (confirm('By clicking continue you acknowledge that all changes to this information will be collected, audited, and your administrator may contact you about them.')) {
      // save the key for lookup
      this.uniqueKeyService.setKey(this.registration.id);
      this.router.navigate([`/${this.path}/registration/summary/full`]);
    }
  }
}
