import { Component, OnInit } from '@angular/core';
import { VolunteerService } from '../core/services/volunteer.service';
import { Volunteer } from '../core/models';
import { Observable } from 'rxjs';
// import { }
@Component({
  selector: 'app-volunteer-team-dashboard',
  templateUrl: './volunteer-team-dashboard.component.html',
  styleUrls: ['./volunteer-team-dashboard.component.scss']
})
export class VolunteerTeamDashboardComponent implements OnInit {
  // server response
  resultsAndPagination: Observable<Volunteer[]>;

  // search related
  isLoadingResults = false;
  searchState = { offset: 0, limit: 100, sort: '', query: '' };
  // searchResults$: Observable<EvacueeSearchResults>;

  constructor(
    private volunteerService: VolunteerService,
  ) { }

  ngOnInit() {
  }

}
