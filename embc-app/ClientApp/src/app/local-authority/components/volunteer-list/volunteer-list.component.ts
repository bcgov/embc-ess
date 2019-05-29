import { Component, OnInit, OnDestroy, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { AuthService } from '../../../core/services/auth.service';
import { VolunteerService, VolunteerSearchQueryParameters } from '../../../core/services/volunteer.service';
import { ListResult, Volunteer } from '../../../core/models';
import { UniqueKeyService } from '../../../core/services/unique-key.service';
import { SearchQueryParameters } from '../../../core/models/search-interfaces';

@Component({
  selector: 'app-volunteer-list',
  templateUrl: './volunteer-list.component.html',
  styleUrls: ['./volunteer-list.component.scss']
})
export class VolunteerListComponent implements OnInit, OnDestroy {

  @ViewChild('viewAlert') viewAlert: TemplateRef<any>;

  // simple server response
  resultsAndPagination: ListResult<Volunteer>;
  notFoundMessage = 'Searching ...';
  defaultSearchQuery: VolunteerSearchQueryParameters = {
    offset: 0,
    limit: 20
  };
  queryString: string;
  // a place to save the last query parameters
  previousQuery: SearchQueryParameters = {};
  sort = ''; // how do we sort the list

  // this is the correct path prefix for the user routing
  path: string;

  confirmModal: NgbModalRef = null;

  constructor(
    private router: Router,
    private volunteerService: VolunteerService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private modals: NgbModal,
  ) { }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe(p => this.path = p);

    // initialize the global variables
    this.authService.getCurrentUser()
      .subscribe(u => {
        // collect the current user
        this.volunteerService.getVolunteerById(u.contactid)
          .subscribe(volunteer => {
            // save the organization ID for future search queries
            this.defaultSearchQuery.org_id = volunteer.organization.id;
            // get the results
            this.getVolunteers(this.defaultSearchQuery)
              .subscribe(listResult => {
                this.resultsAndPagination = listResult;
              });
          }, err => {
            this.notFoundMessage = err;
          }
          );
      }, err => {
        this.notFoundMessage = err;
      });
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.confirmModal) { this.confirmModal.dismiss(); }
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
    this.getVolunteers(query).subscribe(listResult => {
      if (listResult.data.length <= 0) {
        this.notFoundMessage = 'No results found.';
      } else {
        this.notFoundMessage = 'Searching ...';
      }
      this.resultsAndPagination = listResult;
    });
  }
  onPaginationEvent(event: SearchQueryParameters) {
    // save the pagination into the previous query and execute the query again
    this.getVolunteers(event).subscribe(r => {
      this.resultsAndPagination = r;
    });
  }

  modifyVolunteer(id?: string) {
    if (!id) {
      // no id means 'add user' -> clear unique key
      this.uniqueKeyService.clearKey();
      this.router.navigate([`/${this.path}/volunteer`, { orgId: this.defaultSearchQuery.org_id }]);
      return;
    }

    this.confirmModal = this.modals.open(this.viewAlert, { centered: true, windowClass: 'modal-small' });
    this.confirmModal.result.then(
      () => {
        // modal was closed
        this.confirmModal = null;

        // save the volunteer ID for lookup in the new component
        this.uniqueKeyService.setKey(id);
        this.router.navigate([`/${this.path}/volunteer`, { orgId: this.defaultSearchQuery.org_id }]);
      },
      () => {
        // modal was dismissed
        this.confirmModal = null;
      }
    );
  }

}
