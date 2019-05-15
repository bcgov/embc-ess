import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';


import { AuthService } from '../core/services/auth.service';
import { VolunteerService, VolunteerSearchQueryParameters } from '../core/services/volunteer.service';
import { ListResult, Volunteer } from '../core/models';
import { UniqueKeyService } from '../core/services/unique-key.service';
import { SearchQueryParameters } from '../core/models/search-interfaces';

@Component({
  selector: 'app-volunteer-list',
  templateUrl: './volunteer-list.component.html',
  styleUrls: ['./volunteer-list.component.scss']
})
export class VolunteerListComponent implements OnInit {
  // simple server response
  resultsAndPagination: ListResult<Volunteer>;
  notFoundMessage = 'Searching ...';
  defaultSearchQuery: VolunteerSearchQueryParameters = {
    offset: 0,
    limit: 20
  };
  queryString: string;
  previousQuery: SearchQueryParameters = {}; // a place to save the last query parameters
  sort = ''; // how do we sort the list

  // this is the correct path prefix for the user routing
  path: string;

  constructor(
    private router: Router,
    private volunteerService: VolunteerService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService
  ) { }

  ngOnInit() {
    // save the path for routing later
    this.authService.path.subscribe(p => this.path = p);
    // initialize the global variables. When done get volunteers associated with the organization
    this.authService.getCurrentUser()
      .subscribe(u => {
        // collect the current user
        this.volunteerService.getVolunteerById(u.contactid)
          .subscribe(
            v => {
              // save the organization ID for future search queries
              this.defaultSearchQuery.org_id = v.organization.id;
              // get the results
              this.getVolunteers(this.defaultSearchQuery)
                .subscribe(r => {
                  this.resultsAndPagination = r;
                });
            },
            err => {
              this.notFoundMessage = err;
            }
          );
      });
  }

  getVolunteers(query: SearchQueryParameters = this.defaultSearchQuery): Observable<ListResult<Volunteer>> {
    // store the sorting
    query.sort = this.sort;
    // save the generic query for repeat searches
    this.previousQuery = query;

    // cast the search parameters into the new interface
    const q: VolunteerSearchQueryParameters = query;
    // save the organization id into the query from the default
    q.org_id = this.defaultSearchQuery.org_id;
    return this.volunteerService.getVolunteers(q);
  }

  search() {
    // submit and collect search with a query string
    const query = this.defaultSearchQuery;
    query.q = this.queryString;
    this.getVolunteers(query).subscribe(r => {
      if (r.data.length <= 0) {
        this.notFoundMessage = 'No results found.';
      } else {
        this.notFoundMessage = 'Searching ...';
      }
      this.resultsAndPagination = r;
    });
  }
  onPaginationEvent(event: SearchQueryParameters) {
    // save the pagination into the previous query and execute the query again
    this.getVolunteers(event).subscribe(r => {
      this.resultsAndPagination = r;
    });
  }
  modifyVolunteer(id?: string) {
    if (id) {
      // save the unique ID for lookup in the new component
      this.uniqueKeyService.setKey(id);
    }
    // save the volunteer
    this.router.navigate([`/${this.path}/volunteer`]);
  }
}
