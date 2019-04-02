import { Component, OnInit } from '@angular/core';
import { Registration, ListResult, User } from '../core/models';
import { RegistrationService } from '../core/services/registration.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { EvacueeSearchResults, SearchQueryParameters } from '../shared/components/search';
import { AuthService } from '../core/services/auth.service';

interface Stub {
  id?: string; // the guid to link them to their file
  restrictedAccess: boolean; // should this file be shown or not?
  essFileNumber: number; // what is the ESS file number
  firstName: string;
  lastName: string;
  incidentTaskTaskNumber: string;
  requiresIncidentals: boolean; // do they need vouchers
  personType: string; // HOH || FMBR || VOLN
  evacuatedFrom: string; // community name
  evacuatedTo: string; // community name
  registrationCompletionDate: Date;

}
@Component({
  selector: 'app-volunteer-dashboard',
  templateUrl: './volunteer-dashboard.component.html',
  styleUrls: ['./volunteer-dashboard.component.scss']
})
export class VolunteerDashboardComponent implements OnInit {

  currentUser: User;
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
    private route: ActivatedRoute,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.authService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
      this.isLoggedIn = !!user;
    });

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

}
