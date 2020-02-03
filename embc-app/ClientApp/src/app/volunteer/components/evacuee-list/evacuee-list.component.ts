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
  isVolunteer: boolean; 
  // for R1, advanced search mode is the only mode
  advancedSearchMode = true;
  advancedSearchForm = this.fb.group({
    last_name: null,
    first_name: null,
    dob: null,
    task_no: null,
    ess_file_no: null,
    evacuated_from: null,
    evacuated_to: null,
    registration_completed: null,
    referrals_provided: null,
  });

  advancedSearchValid = {
    hasLastName: true,
    hasFirstName: true,
    hasDob: true
  }

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
    //this.getEvacuees().subscribe((listResult: ListResult<EvacueeListItem>) => {
    //  this.resultsAndPagination = listResult;
    //});
    this.authService.isVolunteer$.subscribe(vol => this.isVolunteer = vol);
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

  advancedSearch() {
    const dob = this.advancedSearchForm.get('dob').value;
    const fName = this.advancedSearchForm.get('first_name').value;
    const lName = this.advancedSearchForm.get('last_name').value;
    // Ensure required fields are not null or empty strings
    this.advancedSearchValid.hasDob = dob != null && dob != '';
    this.advancedSearchValid.hasFirstName = fName != null && fName !== '';
    this.advancedSearchValid.hasLastName = lName != null && lName !== '';

    if (this.advancedSearchValid.hasDob && this.advancedSearchValid.hasFirstName && this.advancedSearchValid.hasLastName) {
      this.search();
    }
    else {
      this.notFoundMessage = 'Please fill out all fields.';
    }
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
    // if the user is a volunteer we will route them to the results page
    if (this.isVolunteer) {
      // Navigate to results
      this.router.navigate([`/${this.path}/evacuee/results`]);
    }
  }

  createSearchQuery(): EvacueeSearchQueryParameters {
    // store the sorting and pagination
    let query = { ...this.defaultSearchQuery, sort: this.sort };
    if (this.advancedSearchMode) {
      const form = this.advancedSearchForm.value;

      // the community auto-complete returns an object. we only want the ID (string)
      const fromCommunity = form.evacuated_from ? form.evacuated_from.name : null;
      const toCommunity = form.evacuated_to ? form.evacuated_to.name : null;

      query = {
        ...query,
        ...form,
        evacuated_from: fromCommunity,
        evacuated_to: toCommunity
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
