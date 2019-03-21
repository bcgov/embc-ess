import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

import { GeographicsService } from '../core/services/geographics.service';
import { IncidentTaskService } from '../core/services/incident-task.service';
import { RegistrationService } from '../core/services/registration.service';
import { UserDataService } from '../core/services/user-data.service';
import { AppState } from '../store';

@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {

  // state needed by this FORM
  countries$ = this.store.select(s => s.lookups.countries.countries);
  regionalDistrics$ = this.store.select(s => s.lookups.regionalDistricts);
  regions$ = this.store.select(s => s.lookups.regions);
  relationshipTypes$ = this.store.select(s => s.lookups.relationshipTypes.relationshipTypes);
  incidentTask$ = this.store.select(s => s.incidentTasks.incidentTasks);
  // communities$

  constructor(
    private store: Store<AppState>,
    private geographicsService: GeographicsService,
    private incidentTaskService: IncidentTaskService,
    private registrationService: RegistrationService,
    private userDataService: UserDataService,
  ) { }

  ngOnInit() {

  }

}
