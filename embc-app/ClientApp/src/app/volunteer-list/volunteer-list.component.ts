import { Component, OnInit } from '@angular/core';
import { ListResult, Volunteer } from '../core/models';

import { Router, ActivatedRoute } from '@angular/router';

import { VolunteerService } from '../core/services/volunteer.service';

@Component({
  selector: 'app-volunteer-list',
  templateUrl: './volunteer-list.component.html',
  styleUrls: ['./volunteer-list.component.scss']
})
export class VolunteerListComponent implements OnInit {
  // simple server response
  metaVolunteers: ListResult<Volunteer>;
  notFoundMessage: string = '';
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
    // TODO: this seems like bad practive but fix when we have time
    this.router.navigate(['../volunteer-edit/fill/' + bceidAccountNumber], { relativeTo: this.route });
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
