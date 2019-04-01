import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../core/services/registration.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Registration } from '../core/models';

@Component({
  selector: 'app-evacuee-summary',
  templateUrl: './evacuee-summary.component.html',
  styleUrls: ['./evacuee-summary.component.scss']
})
export class EvacueeSummaryComponent implements OnInit {

  // collect ess file number from activated route
  registration: Registration;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private registrationService: RegistrationService,
  ) { }

  ngOnInit() {
    // collect ess file from activated route
    // try to collect the ess file number and load it
    // if there are route params we should grab them
    if (this.route.snapshot.params.id) {
      const snapshot = this.route.snapshot.params.id;
      // TODO: go get the evacuee from db eventually
      this.registrationService.getRegistrationById(snapshot)
        .subscribe(r => {
          // if there is nothing useful returned route somewhere else.
          if (!r.essFileNumber) {
            this.router.navigateByUrl('dashboard');
          } else {
            // Save the registration into the
            this.registration = r;
          }
        });
    }
  }

  routeTo(id: string) {
    // TODO: this seems like bad practive but fix when we have time
    if (confirm('By clicking continue you acknowledge that all changes to this information will be collected, audited, and your administrator may contact you about them.')) {
      this.router.navigate(['../../register-evacuee/fill/' + id], { relativeTo: this.route });
    }
  }
}
