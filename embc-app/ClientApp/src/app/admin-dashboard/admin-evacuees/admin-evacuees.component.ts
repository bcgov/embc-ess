import { Component, OnInit } from '@angular/core';
import { SearchQueryParameters, EvacueeSearchResults } from 'src/app/shared/components/search';
import { Observable } from 'rxjs';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { Router, ActivatedRoute } from '@angular/router';
import { map } from 'rxjs/operators';
import { ListResult, Registration } from 'src/app/core/models';

@Component({
  selector: 'app-admin-evacuees',
  templateUrl: './admin-evacuees.component.html',
  styleUrls: ['./admin-evacuees.component.scss']
})
export class AdminEvacueesComponent implements OnInit {

  // server response
  resultsAndPagination$: Observable<ListResult<Registration>>;

  // search related
  isLoadingResults = false;
  searchState = { offset: 0, limit: 100, sort: '', query: '' };
  searchResults$: Observable<EvacueeSearchResults>;

  constructor(
    private registrationService: RegistrationService,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
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
}
