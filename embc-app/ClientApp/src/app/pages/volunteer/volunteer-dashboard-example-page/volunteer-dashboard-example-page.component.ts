import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { AuthService } from 'src/app/core/services/auth.service';
import { Router } from '@angular/router';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { ListResult, Registration, PaginationSummary } from 'src/app/core/models';
import { Observable } from 'rxjs';
import { EvacueeSearchResults, SearchQueryParameters } from 'src/app/core/models/search-interfaces';

@Component({
  selector: 'app-volunteer-dashboard-example-page',
  templateUrl: './volunteer-dashboard-example-page.component.html',
  styleUrls: ['./volunteer-dashboard-example-page.component.scss']
})
export class VolunteerDashboardExamplePageComponent implements OnInit {

  isLoggedIn = false;
  // server response
  resultsAndPagination$: Observable<ListResult<Registration>>;
  pagination: PaginationSummary = null;

  // search related
  isLoadingResults = false;
  searchResults$: Observable<EvacueeSearchResults>;
  increments: number[] = [5, 10, 25, 50, 100, 1000];
  // collection of pagination parameters for UI pagination
  // doesn't need to be an object besides it provides a visual seper
  page: number; // the current displayed page
  totalPages: number; // how many pages are returned?
  pageSize: number; // how many entries are on the page
  previousQuery: string; // a place to save the last query parameters
  sort: string = '-registrationCompletionDate'; // how do we sort the list query param
  collectionSize: number = 0; // how large is the collection?
  maxSize = 10; // how many pages of results shoudl the UI show before collapsing?
  boundaryLinks = true; // do we show the jump to first and last page links?

  constructor(
    private registrationService: RegistrationService,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
    // go get the data
    this.doSearch();
  }
  search(query: string = '', page: number = 1, maxSize: number = this.maxSize) {
    // update form state
    this.isLoadingResults = true;

    // go get a fresh list of registrations from the service
    const queryParams: SearchQueryParameters = {
      offset: (page * maxSize) - maxSize,
      limit: maxSize,
      sort: this.sort,
      q: query
    };

    // go get the collection of meta and data
    this.resultsAndPagination$ = this.registrationService.getRegistrations(queryParams);

    // process server response into something we can display in the UI
    this.searchResults$ = this.resultsAndPagination$.pipe(
      map(x => {
        // Flip flag to show that loading has finished.
        this.isLoadingResults = false;

        // collect all of the meta into variables
        this.page = x.metadata.page;
        this.totalPages = x.metadata.totalPages;
        this.collectionSize = x.metadata.totalCount;
        this.maxSize = x.metadata.pageSize;

        this.pagination = x.metadata;

        //save the last query performed
        this.previousQuery = query;

        // the search results need to be in this special format
        return { results: x.data, query } as EvacueeSearchResults;
      })
    );
  }

  onPageChange(page: number = this.page) {
    // change the page that we want
    // search again on whatever the last query was (or blank)
    this.search(this.previousQuery || '', page)
  }
  onIncrementChange() {
    // this is a placeholder for handling a change to the maxSize
  }

  doSearch(query: string = this.previousQuery) {
    // only search
    this.search(query);
  }

  routeTo(essFileNumber: string) {
    // TODO: this seems like bad practice but fix when we have time
    this.router.navigate(['register-evacuee/fill/' + essFileNumber]);
  }

}
