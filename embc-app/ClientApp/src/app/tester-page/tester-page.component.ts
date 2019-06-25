import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../core/services/registration.service';
import { Registration } from '../core/models';

@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {
  registrationSummary;
  registration: Registration;
  constructor(
    private registrationService: RegistrationService
  ) { }

  ngOnInit() {
    this.registrationService.getRegistrationSummaryById('100087').subscribe(r => this.registrationSummary = r);
    this.registrationService.getRegistrationById('100087').subscribe(r => this.registration = r);
  }

}
