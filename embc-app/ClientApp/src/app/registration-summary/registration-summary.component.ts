import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../core/services/registration.service';
import { Router } from '@angular/router';
import { Registration } from '../core/models';
import { AuthService } from '../core/services/auth.service';
import { UniqueKeyService } from '../core/services/unique-key.service';

@Component({
  selector: 'app-registration-summary',
  templateUrl: './registration-summary.component.html',
  styleUrls: ['./registration-summary.component.scss']
})
export class RegistrationSummaryComponent implements OnInit {

  registration: Registration;
  path: string; // for relative routing

  constructor(
    private router: Router,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe(p => this.path = p);

    // get lookup key and load registration data
    const key = this.uniqueKeyService.getKey();
    // ensure we have a lookup key
    if (key) {
      this.registrationService.getRegistrationById(key)
        .subscribe(r => {
          // ensure we have an ESS File Number
          if (!r.essFileNumber) {
            // send them back to their home page
            this.router.navigate([`/${this.path}`]);
          } else {
            // save the registration object
            this.registration = r;
          }
        });
    } else {
      // send them back to their home page
      this.router.navigate([`/${this.path}`]);
    }
  }

  showFullProfile() {
    // TODO: replace confirm with a better popup
    if (confirm('By clicking continue you acknowledge that all changes to this information will be collected, audited, and your administrator may contact you about them.')) {
      // save the key for lookup
      this.uniqueKeyService.setKey(this.registration.id);
      this.router.navigate([`/${this.path}/registration/summary/full`]);
    }
  }

}
