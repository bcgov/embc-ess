import { Component, OnInit } from '@angular/core';
import { VolunteerService } from '../core/services/volunteer.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ListResult, Volunteer } from '../core/models';

@Component({
  selector: 'app-volunteer-team-dashboard',
  templateUrl: './volunteer-team-dashboard.component.html',
  styleUrls: ['./volunteer-team-dashboard.component.scss']
})
export class VolunteerTeamDashboardComponent implements OnInit {
  // simple server response
  metaVolunteers: ListResult<Volunteer>;
  notFoundMessage = '';

  constructor(
    private volunteerService: VolunteerService,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    // collect all volunteers
    this.getVolunteers();
  }

  routeTo(bceidAccountNumber: string) {
    // TODO: this seems like bad practice but fix when we have time
    this.router.navigate(['../volunteer/' + bceidAccountNumber], { relativeTo: this.route });
  }

  getVolunteers(limit?: number, offset?: number, query?: string, sort?: string) {
    // get volunteers with supplied params defaults defined in
    this.volunteerService.getVolunteers({ limit, offset, q: query, sort }).subscribe((v: ListResult<Volunteer>) => {
      // save the metaVolunteers
      this.metaVolunteers = v;
    });
  }

  search(searchTerm: string) {
    // submit and collect search
    this.getVolunteers(null, null, searchTerm);
  }
}
