import { Component, OnInit } from '@angular/core';
import { VolunteerService } from '../core/services/volunteer.service';
import { Volunteer, Registration } from '../core/models';
import { Store } from '@ngrx/store';
import { AppState } from '../store';
import { takeWhile } from 'rxjs/operators';
import { state } from '@angular/animations';
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
  currentRegistration$ = this.store.select(s => s.registrations.currentRegistration);
  componentActive = true;
  currentVolunteer$ = this.store.select(s => s.volunteers.currentVolunteer);
  // currentVolunteer$ = this.store.select(s => s.)
  // // state needed by this FORM
  // countries$ = this.store.select(s => s.lookups.countries.countries);
  // regionalDistricts$ = this.store.select(s => s.lookups.regionalDistricts);
  // regions$ = this.store.select(s => s.lookups.regions);
  // relationshipTypes$ = this.store.select(s => s.lookups.relationshipTypes.relationshipTypes);
  // incidentTask$ = this.store.select(s => s.incidentTasks.incidentTasks);
  // // communities$

  // regions: Region[];
  // incidentTasks: IncidentTask[];
  // registrations: Registration[];
  // user: User;
  registration: Registration;
  volunteer: Volunteer;

  constructor(
    private store: Store<AppState>,
    // private incidentTaskService: IncidentTaskService,
    // private registrationService: RegistrationService,
    // private userDataService: UserDataService,
    // private controlledListService: ControlledListService,
    // private volunteerService: VolunteerService
  ) { }

  ngOnInit() {
    // this.volunteerService.getAllVolunteers().subscribe(v => this.volunteers = v);
    // this.volunteerService.getVolunteerById('BCEIDACCOUNT').subscribe(v => this.volunteer = v);

    this.currentRegistration$.pipe(takeWhile(() => this.componentActive))
      .subscribe(value => this.registration = value);
    this.currentVolunteer$.pipe(takeWhile(() => this.componentActive))
      .subscribe(v => this.volunteer = v);
    // this.userDataService.getCurrentUser().subscribe(u => this.user = u);
    // this.incidentTaskService.getIncidentTasks().subscribe(i => this.incidentTasks = i);
    // this.registrationService.getRegistrations().subscribe(r => this.registrations = r.data);
  }

}
