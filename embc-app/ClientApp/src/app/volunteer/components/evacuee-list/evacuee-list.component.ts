import { Component, OnInit } from '@angular/core';
import { ListResult, EvacueeListItem } from 'src/app/core/models';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { EvacueeSearchQueryParameters } from 'src/app/core/models/search-interfaces';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { EvacueeService } from 'src/app/core/services/evacuee.service';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-evacuee-list',
  templateUrl: './evacuee-list.component.html',
  styleUrls: ['./evacuee-list.component.scss']
})
export class EvacueeListComponent implements OnInit {
  evacuees: any;
  // server response
  resultsAndPagination: ListResult<EvacueeListItem>;
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
  advancedSearchForm = this.fb.group({
    last_name: null,
    first_name: null,
    task_no: null,
    ess_file_no: null,
    evacuated_from: null,
    evacuated_to: null,
    registration_completed: null,
    referrals_provided: null,
  });

  constructor(
    private evacueeService: EvacueeService,
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private fb: FormBuilder,
  ) { }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe((path: string) => this.path = path);
    this.getEvacuees().subscribe((listResult: ListResult<EvacueeListItem>) => {
      this.resultsAndPagination = listResult;
    });
  }

  switchToAdvancedSearch() {
    this.advancedSearchMode = true;
    // when you switch back reset the search
    this.search();
  }

  switchToBasicSearch() {
    this.advancedSearchMode = false;
    // when you switch back reset the search
    this.search();
  }

  getEvacuees(query: EvacueeSearchQueryParameters = this.defaultSearchQuery): Observable<ListResult<EvacueeListItem>> {
    // save the generic query for repeat searches
    this.previousQuery = { ...query };
    // save the organization id into the query from the default
    return this.evacueeService.getEvacuees(query);
  }

  search() {
    // submit and collect search with a query string
    const query = this.createSearchQuery();

    this.getEvacuees(query).subscribe((listResult: ListResult<EvacueeListItem>) => {
      if (listResult.data.length <= 0) {
        this.notFoundMessage = 'No results found.';
      } else {
        this.notFoundMessage = 'Searching ...';
      }
      this.resultsAndPagination = listResult;
    });
  }

  createSearchQuery(): EvacueeSearchQueryParameters {
    // store the sorting and pagination
    let query = { ...this.defaultSearchQuery, sort: this.sort };
    if (this.advancedSearchMode) {
      const form = this.advancedSearchForm.value;

      // the community auto-complete returns an object. we only want the ID (string)
      const fromCommunityId = form.evacuated_from ? form.evacuated_from.id : null;
      const toCommunityId = form.evacuated_to ? form.evacuated_to.id : null;

      query = {
        ...query,
        ...form,
        evacuated_from: fromCommunityId,
        evacuated_to: toCommunityId
      };

      // delete unneeded values
      delete query.q;
      if (query.registration_completed === null) {
        delete query.registration_completed;
      }
      if (query.referrals_provided === null) {
        delete query.referrals_provided;
      }
    } else {
      // basic search
      query.q = this.queryString;
    }
    return query;
  }

  onPaginationEvent(event: EvacueeSearchQueryParameters) {
    // save the pagination into the previous query and execute the query again
    this.previousQuery.limit = event.limit;
    this.previousQuery.offset = event.offset;
    this.getEvacuees(this.previousQuery).subscribe((listResult: ListResult<EvacueeListItem>) => {
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

  onNullQueryString() {
    // when a user in IE11 clicks the x to clear the field we need to be sure that we reset the search
    if (!this.queryString) {
      this.search();
    }
  }
}
