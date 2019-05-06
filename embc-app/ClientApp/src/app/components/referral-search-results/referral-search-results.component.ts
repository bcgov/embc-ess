import { Component, Input, Output, EventEmitter, SimpleChanges, OnChanges, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Referral } from 'src/app/core/models';
import { ReferralSearchResults } from 'src/app/core/models/search-interfaces';
import { AuthService } from 'src/app/core/services/auth.service';
import startCase from 'lodash/startCase';
import toLower from 'lodash/toLower';

interface RowItem {
  registrationId: string;
  data: Referral;
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
