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
    this.loadFromRoute();
  }
  loadFromRoute() {
    // try to collect the ess file number and load it
    // if there are route params we should grab them
    if (this.route.snapshot.params.essFileNumber) {
      // TODO: go get the evacuee from db eventually
      this.registrationService.getRegistrationByEssFileNumber(this.route.snapshot.params.essFileNumber)
        .subscribe(r => {
          // Save the registration into the
          this.registration = r;
        });
    } else {
      // there is nothing to show? Route back somewhere else. (Volunteer dashboard?)
    }
  }
}
