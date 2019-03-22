import { Component, Input, Output, EventEmitter, SimpleChanges, OnChanges } from '@angular/core';

import { EvacueeSearchResults } from '../../utils';

// TODO: Rename this
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

/**
 * A component to display search results in groups
 */
@Component({
  selector: 'app-evacuee-search-results',
  templateUrl: './evacuee-search-results.component.html',
  styleUrls: ['./evacuee-search-results.component.scss']
})
export class EvacueeSearchResultsComponent implements OnChanges {

  /**
   * The results to display
   */
  @Input() searchResults: EvacueeSearchResults;

  /**
   * Emitted when the user selects a search result
   */
  @Output() resultSelected = new EventEmitter<Stub>();

  notFoundMessage = 'Searching ...';
  rowItems: Stub[] = [];

  ngOnChanges(changes: SimpleChanges) {
    this.rowItems = this.processSearchResults(this.searchResults);
  }

  onResultSelected(rowItem: Stub, event: MouseEvent) {
    this.resultSelected.emit(rowItem);
  }

  // Map the search results (i.e. Registrations) into row items suitable for display on a table
  private processSearchResults(search: EvacueeSearchResults): Stub[] {
    if (!search) {
      return [];
    }

    this.notFoundMessage = 'No results found.';
    const stubCollector: Stub[] = [];
    search.results.forEach(registration => {
      if (!registration) {
        return;  // bad data; should fix
      }
      // push the head of household as a stub
      const hoh: Stub = {
        id: registration.id, // the guid to link them to their file
        restrictedAccess: registration.restrictedAccess, // should this file be shown or not?
        essFileNumber: registration.essFileNumber, // what is the ESS file number
        firstName: registration.headOfHousehold.firstName,
        lastName: registration.headOfHousehold.lastName,
        requiresIncidentals: registration.requiresIncidentals, // do they need vouchers
        personType: 'HOH', // HOH || FMBR || VOLN
        incidentTaskTaskNumber: null,
        evacuatedFrom: null, // community name
        evacuatedTo: null, // community name
        registrationCompletionDate: registration.registrationCompletionDate
      };

      if (registration.incidentTask && registration.incidentTask.taskNumber) {
        // check for nulls
        hoh.incidentTaskTaskNumber = registration.incidentTask.taskNumber;
      } else {
        hoh.incidentTaskTaskNumber = '';
      }
      if (registration.incidentTask && registration.incidentTask.community && registration.incidentTask.community.name) {
        // check for nulls
        hoh.evacuatedFrom = registration.incidentTask.community.name;
      } else {
        hoh.evacuatedFrom = '';
      }
      if (registration.hostCommunity && registration.hostCommunity.name) {
        // check for nulls
        hoh.evacuatedTo = registration.hostCommunity.name;
      } else {
        hoh.evacuatedTo = '';
      }
      stubCollector.push(hoh);

      // push the family members of the HOH as stubs
      for (const familyMember of registration.headOfHousehold.familyMembers) {
        const fmbr = {
          id: registration.id, // the guid to link them to their file
          restrictedAccess: registration.restrictedAccess, // should this file be shown or not?
          essFileNumber: registration.essFileNumber, // what is the ESS file number
          firstName: familyMember.firstName,
          lastName: familyMember.lastName,
          incidentTaskTaskNumber: null,
          requiresIncidentals: registration.requiresIncidentals, // do they need vouchers
          personType: familyMember.personType, // HOH || FMBR || VOLN
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
        if (registration.incidentTask && registration.incidentTask.community && registration.incidentTask.community.name) {
          // check for nulls
          fmbr.evacuatedFrom = registration.incidentTask.community.name;
        } else {
          fmbr.evacuatedFrom = '';
        }
        if (registration.hostCommunity && registration.hostCommunity.name) {
          // check for nulls
          fmbr.evacuatedTo = registration.hostCommunity.name;
        } else {
          fmbr.evacuatedTo = '';
        }
        stubCollector.push(fmbr);
      }
    });
    return stubCollector;
  }
}
