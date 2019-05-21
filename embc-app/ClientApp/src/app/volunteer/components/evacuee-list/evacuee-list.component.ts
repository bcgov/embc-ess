import { Component, OnInit } from '@angular/core';
import { ListResult, Registration, PaginationSummary, Evacuee } from '../../../core/models';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { SearchQueryParameters } from '../../../core/models/search-interfaces';
import { UniqueKeyService } from '../../../core/services/unique-key.service';
import { EvacueeService } from 'src/app/core/services/evacuee.service';

@Component({
  selector: 'app-evacuee-list',
  templateUrl: './evacuee-list.component.html',
  styleUrls: ['./evacuee-list.component.scss']
})
export class EvacueeListComponent implements OnInit {
  evacuees: any;
  // server response
  resultsAndPagination: ListResult<Evacuee>;
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
    private evacueeService: EvacueeService,
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe(p => this.path = p);
    this.getEvacuees().subscribe(r => {
      this.resultsAndPagination = r;
    });
  }

  getEvacuees(query: SearchQueryParameters = this.defaultSearchQuery): Observable<ListResult<Evacuee>> {
    // store the sorting
    query.sort = this.sort;
    // save the generic query for repeat searches
    this.previousQuery = query;
    // save the organization id into the query from the default
    return this.evacueeService.getEvacuees(query);
  }

  search() {
    // submit and collect search with a query string
    const query = this.defaultSearchQuery;
    query.q = this.queryString;
    this.getEvacuees(query).subscribe(r => {
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
    this.getEvacuees(event).subscribe(r => {
      this.resultsAndPagination = r;
    });
  }

  edit(id: string) {
    this.uniqueKeyService.setKey(id);
    this.router.navigate([`/${this.path}/registration`]);
  }

  view(id: string) {
    this.uniqueKeyService.setKey(id);
    this.router.navigate([`/${this.path}/registration/summary`]);
  }
}
