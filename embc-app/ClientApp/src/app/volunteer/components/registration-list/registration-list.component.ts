import { Component, OnInit } from '@angular/core';
import { ListResult, Registration, PaginationSummary, isBcAddress, isOtherAddress } from '../../../core/models';
import { Observable } from 'rxjs';
import { RegistrationService } from '../../../core/services/registration.service';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { SearchQueryParameters } from '../../../core/models/search-interfaces';
import { UniqueKeyService } from '../../../core/services/unique-key.service';
import get from 'lodash/get';

interface RowItem {
  // The underlying data for any given row within the table view.
  rowData: Registration;

  // metadata for this row
  count: number;  // Length of the number of total rows.
  even: boolean;  // True if this cell is contained in a row with an even-numbered index.
  first: boolean; // True if this cell is contained in the first row.
  index: number;  // Index of the data object in the provided data array.
  last: boolean;  // True if this cell is contained in the last row.
  odd: boolean;   // True if this cell is contained in a row with an odd-numbered index.

  // These are convenience accessors to the underlying data represented by `rowData`
  // They are populated in `processSearchResults()`
  id?: string; // the guid to link them to their file
  restrictedAccess: boolean; // should this file be shown or not?
  headOfHousehold: boolean; // whether this rowItem is belongs to the head of household
  essFileNumber: number; // what is the ESS file number
  firstName: string;
  lastName: string;
  incidentTaskTaskNumber: string;
  requiresIncidentals: boolean; // do they need vouchers
  personType: string; // HOH || FMBR || VOLN
  evacuatedFrom: string; // community name
  evacuatedTo: string; // community name
  registrationCompletionDate: string | null;
}
@Component({
  selector: 'app-registration-list',
  templateUrl: './registration-list.component.html',
  styleUrls: ['./registration-list.component.scss']
})
export class RegistrationListComponent implements OnInit {
  // server response
  resultsAndPagination: ListResult<Registration>;
  rows: RowItem[] = [];
  pagination: PaginationSummary = null;
  notFoundMessage = 'Searching ...';
  defaultSearchQuery: SearchQueryParameters = {
    offset: 0,
    limit: 10
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
    this.getRegistrations().subscribe(r => {
      this.resultsAndPagination = r;
      this.rows = this.processSearchResults(r.data);
    });
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
      this.rows = this.processSearchResults(r.data);
    });
  }

  onPaginationEvent(event: SearchQueryParameters) {
    // save the pagination into the previous query and execute the query again
    this.getRegistrations(event).subscribe(r => {
      this.resultsAndPagination = r;
      this.rows = this.processSearchResults(r.data);
    });
  }

  modifyRegistration(id: string) {
    // no id means 'add user' -> clear unique key
    this.uniqueKeyService.setKey(id);
    this.router.navigate([`/${this.path}/registration`]);
  }

  //*******/
  // Map the search results (i.e. Registrations) into row items suitable for display on a table
  private processSearchResults(results: Registration[]): RowItem[] {
    if (!results) {
      return [];
    }

    this.notFoundMessage = 'No results found.';
    const listItems: RowItem[] = [];
    results.forEach((registration, index, array) => {
      if (!registration) {
        return;  // bad data; should fix
      }

      // push the head of household as a stub
      const hoh: RowItem = {
        // hold on to a copy of the source data
        rowData: { ...registration },

        // populate row metadata
        index,
        count: array.length,
        even: index % 2 === 0,
        odd: Math.abs(index % 2) === 1,
        first: index === 0,
        last: index === array.length - 1,

        id: registration.id, // the guid to link them to their file
        restrictedAccess: registration.restrictedAccess, // should this file be shown or not?
        essFileNumber: registration.essFileNumber, // what is the ESS file number
        firstName: registration.headOfHousehold.firstName,
        lastName: registration.headOfHousehold.lastName,
        requiresIncidentals: registration.requiresIncidentals, // do they need vouchers
        personType: registration.headOfHousehold.personType, // HOH || FMBR || VOLN
        headOfHousehold: true,
        incidentTaskTaskNumber: null,
        evacuatedFrom: null, // community name
        evacuatedTo: null, // community name
        registrationCompletionDate: registration.registrationCompletionDate
      };

      // get Incident Task Number
      hoh.incidentTaskTaskNumber = (registration.incidentTask && registration.incidentTask.taskNumber) || '';

      // get Evacuated From (depending on address type)
      if (registration.headOfHousehold && isBcAddress(registration.headOfHousehold.primaryResidence)) {
        hoh.evacuatedFrom = get(registration, 'headOfHousehold.primaryResidence.community.name', '');
      } else if (registration.headOfHousehold && isOtherAddress(registration.headOfHousehold.primaryResidence)) {
        const city = get(registration, 'headOfHousehold.primaryResidence.city');
        const province = get(registration, 'headOfHousehold.primaryResidence.province');
        const country = get(registration, 'headOfHousehold.primaryResidence.country.name');
        hoh.evacuatedFrom = [city, province, country].filter(x => x).join(', ') || '';
      } else {
        hoh.evacuatedFrom = '';
      }

      // get Evacuated To
      hoh.evacuatedTo = (registration.hostCommunity && registration.hostCommunity.name) || '';

      listItems.push(hoh);

      // push the family members of the HOH as stubs
      if (registration.headOfHousehold && registration.headOfHousehold.familyMembers) {
        for (const familyMember of registration.headOfHousehold.familyMembers) {
          const fmbr = {
            // hold on to a copy of the source data
            rowData: { ...registration },

            // populate row metadata
            index,
            count: array.length,
            even: index % 2 === 0,
            odd: Math.abs(index % 2) === 1,
            first: index === 0,
            last: index === array.length - 1,

            id: registration.id, // the guid to link them to their file
            restrictedAccess: registration.restrictedAccess, // should this file be shown or not?
            essFileNumber: registration.essFileNumber, // what is the ESS file number
            firstName: familyMember.firstName,
            lastName: familyMember.lastName,
            incidentTaskTaskNumber: null,
            requiresIncidentals: registration.requiresIncidentals, // do they need vouchers
            personType: familyMember.personType, // HOH || FMBR || VOLN
            headOfHousehold: false,
            evacuatedFrom: null, // community name
            evacuatedTo: null, // community name
            registrationCompletionDate: registration.registrationCompletionDate
          };

          if (registration.incidentTask && registration.incidentTask.taskNumber) {
            // check for nulls
            fmbr.incidentTaskTaskNumber = registration.incidentTask.taskNumber;
          } else {
            fmbr.incidentTaskTaskNumber = '';
          }
          if (registration.headOfHousehold.primaryResidence
            && registration.headOfHousehold.primaryResidence.community
            && registration.headOfHousehold.primaryResidence.community.name) {
            // check for nulls
            fmbr.evacuatedFrom = registration.headOfHousehold.primaryResidence.community.name;
          } else {
            fmbr.evacuatedFrom = '';
          }
          if (registration.hostCommunity && registration.hostCommunity.name) {
            // check for nulls
            fmbr.evacuatedTo = registration.hostCommunity.name;
          } else {
            fmbr.evacuatedTo = '';
          }
          listItems.push(fmbr);
        }
      }
    });
    return listItems;
  }
  finalize(r: RowItem) {
    this.uniqueKeyService.setKey(r.rowData.id);
    this.router.navigate([`/${this.path}/registration`]);
  }

  view(r: RowItem) {
    this.uniqueKeyService.setKey(r.rowData.id);
    this.router.navigate([`/${this.path}/registration/summary`]);
  }
  /*peopleCount(registration: ListResult<Registration>): ListResult<Registration> {
    // this returns the right count of users
    const count = registration.data
      .map((curr: Registration) => curr.headOfHousehold.familyMembers ? curr.headOfHousehold.familyMembers.length + 1 : 1
      ).reduce((acc, curr) => acc + curr);
    registration.metadata.totalCount = count;
    return registration;
  }*/
}
