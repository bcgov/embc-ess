import { Component, OnInit } from '@angular/core';
import { VolunteerService } from '../core/services/volunteer.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { SearchQueryParameters } from '../shared/components/search';
import { ListResult, Volunteer } from '../core/models';
// import { }
@Component({
  selector: 'app-volunteer-team-dashboard',
  templateUrl: './volunteer-team-dashboard.component.html',
  styleUrls: ['./volunteer-team-dashboard.component.scss']
})
export class VolunteerTeamDashboardComponent implements OnInit {
  // simple server response
  metaVolunteers: ListResult<Volunteer>;
  notFoundMessage: string = '';
  constructor(
    private volunteerService: VolunteerService,
    private router: Router
  ) { }

  ngOnInit() {
    // collect all volunteers
    this.getVolunteers();
  }
  routeTo(bceidAccountNumber: string) {
    // TODO: this seems like bad practive but fix when we have time
    this.router.navigate(['volunteer-edit/fill/' + bceidAccountNumber]);
  }
  getVolunteers(limit?: number, offset?: number, query?: string, sort?: string) {
    // get volunteers with supplied params defaults defined in
    this.volunteerService.getVolunteers(limit, offset, query, sort).subscribe((v: ListResult<Volunteer>) => {
      // save the metaVolunteers
      this.metaVolunteers = v;
    });
  }
  search(searchTerm: string) {
    // submit and collect search
    this.getVolunteers(null, null, searchTerm);
  }
}
