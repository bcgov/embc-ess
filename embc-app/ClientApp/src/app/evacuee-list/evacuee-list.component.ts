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
  searchState = { offset: 0, limit: 100, sort: '', query: '' };
  searchResults$: Observable<EvacueeSearchResults>;

  constructor(
    private registrationService: RegistrationService,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
    // go get the data
    this.doSearch();
  }

  doSearch(query: string = '') {
    // update form state
    this.searchState.query = query || '';
    this.isLoadingResults = true;

    // go get a fresh list of registrations from the service
    const queryParams: SearchQueryParameters = {
      offset: this.searchState.offset,
      limit: this.searchState.limit,
      sort: this.searchState.sort || '',
      q: this.searchState.query
    };
    this.resultsAndPagination$ = this.registrationService.getRegistrations(queryParams);

    // process server response into something we can display in the UI
    this.searchResults$ = this.resultsAndPagination$.pipe(
      map(x => {
        // Flip flag to show that loading has finished.
        this.isLoadingResults = false;
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
