import { Component, OnInit } from '@angular/core';
import { ListResult, Evacuee } from '../../../core/models';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { EvacueeSearchQueryParameters } from 'src/app/core/models/search-interfaces';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
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
  defaultSearchQuery: EvacueeSearchQueryParameters = {
    offset: 0,
    limit: 20
  };
  queryString: string;
  // a place to save the last query parameters
  previousQuery: EvacueeSearchQueryParameters = {};
  sort = '-registrationId'; // how do we sort the list query param
  path: string = null; // the base path for routing

  advancedSearchMode = false;
  advancedSearchParams: EvacueeSearchQueryParameters = {};

  constructor(
    private evacueeService: EvacueeService,
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe((path: string) => this.path = path);
    this.getEvacuees().subscribe((listResult: ListResult<Evacuee>) => {
      this.resultsAndPagination = listResult;
    });
  }

  switchToAdvancedSearch() {
    this.advancedSearchMode = true;

    // TODO: clean-up & manage new search state
  }

  switchToBasicSearch() {
    this.advancedSearchMode = false;

    // TODO: clean-up & manage new search state
  }

  getEvacuees(query: EvacueeSearchQueryParameters = this.defaultSearchQuery): Observable<ListResult<Evacuee>> {
    // save the generic query for repeat searches
    this.previousQuery = query;
    // save the organization id into the query from the default
    return this.evacueeService.getEvacuees(query);
  }

  search() {
    // submit and collect search with a query string
    const query = this.getSearchParams();

    this.getEvacuees(query).subscribe((listResult: ListResult<Evacuee>) => {
      if (listResult.data.length <= 0) {
        this.notFoundMessage = 'No results found.';
      } else {
        this.notFoundMessage = 'Searching ...';
      }
      this.resultsAndPagination = listResult;
    });
  }

  getSearchParams(): EvacueeSearchQueryParameters {
    const query = { ...this.defaultSearchQuery };

    if (this.advancedSearchMode) {
      delete query.q;
      query.last_name = this.advancedSearchParams.last_name || null;
      query.first_name = this.advancedSearchParams.last_name || null;
      query.task_no = this.advancedSearchParams.task_no || null;
      query.ess_file_no = this.advancedSearchParams.ess_file_no || null;
      query.evacuated_from = this.advancedSearchParams.evacuated_from || null;
      query.evacuated_to = this.advancedSearchParams.evacuated_to || null;

      if (this.advancedSearchParams.registration_completed === null) {
        delete query.registration_completed;
      }
      if (this.advancedSearchParams.referrals_provided === null) {
        delete query.referrals_provided;
      }
    } else {
      // basic search
      query.q = this.queryString;
    }

    // store the sorting
    query.sort = this.sort;
    return query;
  }

  onPaginationEvent(event: EvacueeSearchQueryParameters) {
    // save the pagination into the previous query and execute the query again
    this.previousQuery.limit = event.limit;
    this.previousQuery.offset = event.offset;
    this.getEvacuees(this.previousQuery).subscribe((listResult: ListResult<Evacuee>) => {
      this.resultsAndPagination = listResult;
    });
  }

  edit(registrationId: string) {
    // save registration ID for lookup in the new component
    this.uniqueKeyService.setKey(registrationId);

    // go to registration maker
    this.router.navigate([`/${this.path}/registration`]);
  }

  view(registrationId: string) {
    // save registration ID for lookup in the new component
    this.uniqueKeyService.setKey(registrationId);

    // go to registration summary page
    this.router.navigate([`/${this.path}/registration/summary`]);
  }
}
