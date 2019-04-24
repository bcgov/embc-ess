import { Component, OnInit } from '@angular/core';
import { EvacueeSearchResults, SearchQueryParameters } from 'src/app/core/models/search-interfaces';
import { AuthService } from 'src/app/core/services/auth.service';
import { Router } from '@angular/router';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { Observable } from 'rxjs';
import { ListResult, Registration, PaginationSummary } from 'src/app/core/models';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-volunteer-registrations-page',
  templateUrl: './volunteer-registrations-page.component.html',
  styleUrls: ['./volunteer-registrations-page.component.scss']
})
export class VolunteerRegistrationsPageComponent implements OnInit {

  constructor(
    private registrationService: RegistrationService,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() { }

}
