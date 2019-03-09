import { Component, OnInit } from '@angular/core';
import { Registration, Evacuee } from '../core/models';
import { RegistrationService } from '../core/services/registration.service';
@Component({
  selector: 'app-volunteer-dashboard',
  templateUrl: './volunteer-dashboard.component.html',
  styleUrls: ['./volunteer-dashboard.component.scss']
})
export class VolunteerDashboardComponent implements OnInit {

  registrations;

  constructor(
    private registrationService: RegistrationService
  ) { }

  ngOnInit() {
    // go get the data
    this.registrationService.getRegistries()
      .subscribe(r=>{
        //
        this.registrations=r;
      });
  }

}
