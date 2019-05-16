import { Component, OnInit } from '@angular/core';
import { ListResult, Registration, PaginationSummary, Volunteer } from '../core/models';
import { Observable } from 'rxjs';
import { RegistrationService } from '../core/services/registration.service';
import { Router } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { map } from 'rxjs/operators';
import { EvacueeSearchResults, SearchQueryParameters } from '../core/models/search-interfaces';
import { UniqueKeyService } from '../core/services/unique-key.service';

@Component({
  selector: 'app-registration-list',
  templateUrl: './registration-list.component.html',
  styleUrls: ['./registration-list.component.scss']
})
export class RegistrationListComponent implements OnInit {

  // server response
  resultsAndPagination: ListResult<Registration>;
  pagination: PaginationSummary = null;
  notFoundMessage = 'Searching ...';
  defaultSearchQuery: SearchQueryParameters = {
    offset: 0,
    limit: 20
  };
  queryString: string;
  // a place to save the last query parameters
  previousQuery: SearchQueryParameters = {};
  sort = '-registrationCompletionDate'; // how do we sort the list query param

  // this is the correct path prefix for the user routing
  path: string;

  constructor(
    private registrationService: RegistrationService,
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe(p => this.path = p);
  }

  getRegistrations(query: SearchQueryParameters = this.defaultSearchQuery): Observable<ListResult<Registration>> {
    // store the sorting
    query.sort = this.sort;
    // save the generic query for repeat searches
    this.previousQuery = query;
    // save the organization id into the query from the default
    return this.registrationService.getRegistrations(query);
  }

  search() {
    // submit and collect search with a query string
    const query = this.defaultSearchQuery;
    query.q = this.queryString;
    this.getRegistrations(query).subscribe(r => {
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
    this.getRegistrations(event).subscribe(r => {
      this.resultsAndPagination = r;
    });
  }

  modifyRegistration(id: string) {
    // no id means 'add user' -> clear unique key
    this.uniqueKeyService.setKey(id);
    this.router.navigate([`/${this.path}/registration`]);
  }
}
