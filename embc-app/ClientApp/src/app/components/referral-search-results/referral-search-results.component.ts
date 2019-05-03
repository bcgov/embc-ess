import { Component, Input, Output, EventEmitter, SimpleChanges, OnChanges, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Referral } from 'src/app/core/models';
import { ReferralSearchResults } from 'src/app/core/models/search-interfaces';
import { AuthService } from 'src/app/core/services/auth.service';
import startCase from 'lodash/startCase';
import toLower from 'lodash/toLower';

// TODO: Rename this
interface RowItem {
  registrationId: string;

  // The underlying data for any given row within the table view.
  data: Referral;

  // // metadata for this row
  // count: number;  // Length of the number of total rows.
  // even: boolean;  // True if this cell is contained in a row with an even-numbered index.
  // first: boolean; // True if this cell is contained in the first row.
  // index: number;  // Index of the data object in the provided data array.
  // last: boolean;  // True if this cell is contained in the last row.
  // odd: boolean;   // True if this cell is contained in a row with an odd-numbered index.

  // // These are convenience accessors to the underlying data represented by `rowData`
  // // They are populated in `processSearchResults()`
  // id?: string; // the guid to link them to their file
  // restrictedAccess: boolean; // should this file be shown or not?
  // headOfHousehold: boolean; // whether this rowItem is belongs to the head of household
  // essFileNumber: number; // what is the ESS file number
  // firstName: string;
  // lastName: string;
  // incidentTaskTaskNumber: string;
  // requiresIncidentals: boolean; // do they need vouchers
  // personType: string; // HOH || FMBR || VOLN
  // evacuatedFrom: string; // community name
  // evacuatedTo: string; // community name
  // hasReferrals: boolean;
  // registrationCompletionDate: string | null;
}

/**
 * A basic TableView component to display search results in groups
 */
@Component({
  selector: 'app-referral-search-results',
  templateUrl: './referral-search-results.component.html',
  styleUrls: ['./referral-search-results.component.scss']
})
export class ReferralSearchResultsComponent implements OnChanges, OnInit {

  /**
   * The results to display
   */
  @Input() searchResults: ReferralSearchResults;

  /**
   * Emitted when the user selects a search result
   */
  @Output() resultSelected = new EventEmitter<RowItem>();

  rows: RowItem[] = [];
  notFoundMessage = 'Searching ...';
  path: string; // the routing path

  ngOnChanges(changes: SimpleChanges) {
    this.rows = this.processSearchResults(this.searchResults);
  }

  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.authService.path.subscribe(p => this.path = p);
  }
  onResultSelected(rowItem: RowItem, event: MouseEvent) {
    this.resultSelected.emit(rowItem);
  }

  // map the search results (referrals) into row items suitable for display on a table
  private processSearchResults(search: ReferralSearchResults): RowItem[] {
    this.notFoundMessage = 'No referrals have been added yet';

    if (!search) {
      return [];
    }

    // convert search results into row items
    return search.results.map(result => {
      return ({ registrationId: search.registrationId, data: result } as RowItem);
    });
  }

  getType(r: RowItem): string {
    let result: string = null;

    if (r.data.type) {
      result = `<div class="font-weight-bold">${this.toTitleCase(r.data.type)}</div>`;
      if (r.data.subType) {
        result += `<div>${this.toTitleCase(r.data.subType)}</div>`;
      }
    }

    return result;
  }

  toTitleCase(str: string): string {
    return startCase(toLower(str));
  }

  // TODO: move this to HTML so user can open link in new tab
  view(r: RowItem) {
    this.router.navigate([`/${this.path}/referral/${r.registrationId}/${r.data.id}`]);
  }
}
