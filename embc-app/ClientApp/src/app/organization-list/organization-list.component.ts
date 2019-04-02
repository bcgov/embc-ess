import { Component, OnInit } from '@angular/core';
import { ListResult, Volunteer } from '../core/models';
import { VolunteerService } from '../core/services/volunteer.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-organization-list',
  templateUrl: './organization-list.component.html',
  styleUrls: ['./organization-list.component.scss']
})
export class OrganizationListComponent implements OnInit {
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
