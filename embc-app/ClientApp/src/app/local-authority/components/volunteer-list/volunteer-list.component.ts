import { Component, OnInit, OnDestroy, ViewChild, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { AuthService } from 'src/app/core/services/auth.service';
import { VolunteerService, VolunteerSearchQueryParameters } from 'src/app/core/services/volunteer.service';
import { ListResult, Volunteer, User } from 'src/app/core/models';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { SearchQueryParameters } from 'src/app/core/models/search-interfaces';
import { ReadOnlyService } from '../../../core/services/read-only.service';

//
// this is the component used by Local Authority users
//

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

  path: string = null; // the base path for routing

  confirmModal: NgbModalRef = null;

  constructor(
    private router: Router,
    private volunteerService: VolunteerService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private modals: NgbModal,
    public readOnlyService: ReadOnlyService
  ) { }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe((path: string) => this.path = path);

    // NB: ignore any query param -- always use the current Local Authority's org

    // initialize the global variables
    this.authService.getCurrentUser()
      .subscribe((user: User) => {
        // collect the current user
        this.volunteerService.getVolunteerById(user.contactid)
          .subscribe((volunteer: Volunteer) => {
            // save the organization ID for future search queries
            this.defaultSearchQuery.org_id = volunteer.organization.id;
            // get the results
            this.getVolunteers(this.defaultSearchQuery)
              .subscribe((listResult: ListResult<Volunteer>) => {
                this.resultsAndPagination = listResult;
              }, err => {
                this.notFoundMessage = err;
                console.log('error getting volunteers =', err);
              });
          }, err => {
            this.notFoundMessage = err;
            console.log('error getting volunteer =', err);
          });
      }, err => {
        this.notFoundMessage = err;
        console.log('error getting current user =', err);
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
    this.getVolunteers(query).subscribe((listResult: ListResult<Volunteer>) => {
      if (listResult.data.length <= 0) {
        this.notFoundMessage = 'No results found.';
      } else {
        this.notFoundMessage = 'Searching ...';
      }
      this.resultsAndPagination = listResult;
    }, err => {
      console.log('error getting volunteers ', err);
    });
  }

  onPaginationEvent(event: SearchQueryParameters) {
    // save the pagination into the previous query and execute the query again
    this.getVolunteers(event).subscribe((listResult: ListResult<Volunteer>) => {
      this.resultsAndPagination = listResult;
    }, err => {
      console.log('error getting volunteers ', err);
    });
  }

  modifyVolunteer(volunteerId?: string) {
    if (!volunteerId) {
      // no id means 'add user' -> clear unique key
      this.uniqueKeyService.clearKey();
      this.router.navigate([`/${this.path}/volunteer`]);
      return;
    }

    this.confirmModal = this.modals.open(this.viewAlert, { centered: true, windowClass: 'modal-small' });
    this.confirmModal.result.then(
      () => {
        // modal was closed
        this.confirmModal = null;

        // save volunteer ID for lookup in the new component
        this.uniqueKeyService.setKey(volunteerId);
        this.router.navigate([`/${this.path}/volunteer`]);
      },
      () => {
        // modal was dismissed
        this.confirmModal = null;
      }
    );
  }

}
