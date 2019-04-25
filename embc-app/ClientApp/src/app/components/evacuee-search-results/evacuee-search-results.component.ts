import { Component, Input, Output, EventEmitter, SimpleChanges, OnChanges, OnInit } from '@angular/core';
import { Registration, isBcAddress, isOtherAddress } from 'src/app/core/models';
import { Router, ActivatedRoute } from '@angular/router';
import { EvacueeSearchResults } from 'src/app/core/models/search-interfaces';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { AuthService } from 'src/app/core/services/auth.service';
import get from 'lodash/get';

// TODO: Rename this
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
  hasReferrals: boolean;
  registrationCompletionDate: string | null;
}

/**
 * A basic TableView component to display search results in groups
 */
@Component({
  selector: 'app-evacuee-search-results',
  templateUrl: './evacuee-search-results.component.html',
  styleUrls: ['./evacuee-search-results.component.scss']
})
export class EvacueeSearchResultsComponent implements OnChanges, OnInit {

  /**
   * The results to display
   */
  @Input() searchResults: EvacueeSearchResults;

  /**
   * Emitted when the user selects a search result
   */
  @Output() resultSelected = new EventEmitter<RowItem>();

  rows: RowItem[] = [];
  notFoundMessage = 'Searching ...';
  // the routing path
  path: string;

  ngOnChanges(changes: SimpleChanges) {
    this.rows = this.processSearchResults(this.searchResults);
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private uniqueKeyService: UniqueKeyService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.authService.path.subscribe(p => this.path = p);
  }
  onResultSelected(rowItem: RowItem, event: MouseEvent) {
    this.resultSelected.emit(rowItem);
  }

  // Map the search results (i.e. Registrations) into row items suitable for display on a table
  private processSearchResults(search: EvacueeSearchResults): RowItem[] {
    if (!search) {
      return [];
    }

    this.notFoundMessage = 'No results found.';
    const listItems: RowItem[] = [];
    search.results.forEach((registration, index, array) => {
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
        hasReferrals: this.hasReferrals(registration),
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
          hasReferrals: this.hasReferrals(registration),
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
    });
    return listItems;
  }

  hasReferrals(r: Registration): boolean {
    // TODO we need to check business logic for this because there is deeper discussion with the client about
    // how this becomes a meaningful flag. This also should probably be handled server-side instead of here.
    return false;
  }

  finalize(r: RowItem) {
    this.uniqueKeyService.setKey(r.rowData.id);
    this.router.navigate([`/${this.path}/registration`]);
  }

  view(r: RowItem) {
    this.uniqueKeyService.setKey(r.rowData.id);
    this.router.navigate([`/${this.path}/registration/summary`]
      // , { relativeTo: this.route }
    );
  }
}
