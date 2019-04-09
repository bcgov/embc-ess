import { Component, OnInit } from '@angular/core';
import { User, ListResult, Registration } from '../core/models';
import { Observable } from 'rxjs';
import { EvacueeSearchResults, SearchQueryParameters } from '../shared/components/search';
import { RegistrationService } from '../core/services/registration.service';
import { Router } from '@angular/router';
import { AuthService } from '../core/services/auth.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-evacuee-list',
  templateUrl: './evacuee-list.component.html',
  styleUrls: ['./evacuee-list.component.scss']
})
export class EvacueeListComponent implements OnInit {

  isLoggedIn = false;


  // server response
  resultsAndPagination$: Observable<ListResult<Registration>>;

  // search related
  isLoadingResults = false;
  searchResults$: Observable<EvacueeSearchResults>;

  // collection of pagination parameters for UI pagination
  // doesn't need to be an object besides it provides a visual seper
  page: number; // the current displayed page
  totalPages: number; // how many pages are returned?
  pageSize: number; // how many entries are on the page
  collectionSize: number; // how large is the collection?
  previousQuery: string; // a place to save the last query parameters
  sort: string; // how do we sort the list
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

  onPageChange(page: number) {
    // change the page that we want
    this.page = page - 1;
    // search again on whatever the last query was
    this.doSearch(this.previousQuery);
  }

  doSearch(query: string = '') {
    // how do we know when this is a new query?

    // perform a search. This is triggered from an event in the searchBar
    // or it is triggered by an event in the pagination.
    // update form state
    this.isLoadingResults = true;

    // go get a fresh list of registrations from the service
    const queryParams: SearchQueryParameters = {
      offset: this.page * this.pageSize,
      limit: this.maxSize,
      sort: this.sort || '',
      q: query
    };

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
        this.pageSize = x.metadata.pageSize;
        this.previousQuery = query;
        return { results: x.data, query } as EvacueeSearchResults;
      })
    );

    // TODO: store the pagination metadata + links somewhere
  }


  routeTo(essFileNumber: string) {
    // TODO: this seems like bad practice but fix when we have time
    this.router.navigate(['register-evacuee/fill/' + essFileNumber]);
  }

}
