import { Component, Input, Output, EventEmitter, SimpleChanges, OnChanges, OnInit } from '@angular/core';
import { Referral } from 'app/core/models';
import { ReferralSearchResults } from 'app/core/models/search-interfaces';
import { AuthService } from 'app/core/services/auth.service';
import startCase from 'lodash/startCase';
import toLower from 'lodash/toLower';

interface RowItem {
  registrationId: string;
  data: Referral;
  checked: boolean;
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
  @Output() referralsSelected = new EventEmitter<Referral[]>();

  rows: Array<RowItem> = [];
  notFoundMessage = 'Searching ...';
  path: string = null; // the base path for routing
  referrals: Array<Referral> = [];

  ngOnChanges(changes: SimpleChanges) {
    this.rows = this.processSearchResults(this.searchResults);
  }

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.authService.path.subscribe((path: string) => this.path = path);
  }

  onReferralChange(rowItem: RowItem) {
    const index = this.referrals.indexOf(rowItem.data);

    // if row is not checked then check it (and vice-versa)
    if (!rowItem.checked && index === -1) {
      rowItem.checked = true;
      // add item to array
      this.referrals.push(rowItem.data);
    } else if (rowItem.checked && index !== -1) {
      rowItem.checked = false;
      // remove item from array
      this.referrals.splice(index, 1);
    }

    this.referralsSelected.emit(this.referrals);
  }

  // map the search results (referrals) into row items suitable for display on a table
  private processSearchResults(search: ReferralSearchResults): RowItem[] {
    this.notFoundMessage = 'No referrals have been added yet';

    if (!search) {
      return [];
    }

    // convert search results into row items
    return search.results.map(result => {
      return { registrationId: search.registrationId, data: result, checked: false } as RowItem;
    });
  }

  getType(r: RowItem): string {
    let result: string = null;

    if (r.data && r.data.type) {
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

}
