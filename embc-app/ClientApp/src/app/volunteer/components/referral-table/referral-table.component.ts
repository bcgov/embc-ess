import { Component, OnChanges, Input, SimpleChanges } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ReferralService } from 'src/app/core/services/referral.service';
import { Registration, ListResult, PaginationSummary, Referral } from 'src/app/core/models';
import { ReferralSearchResults, SearchQueryParameters } from 'src/app/core/models/search-interfaces';

@Component({
  selector: 'app-referral-table',
  templateUrl: './referral-table.component.html',
  styleUrls: ['./referral-table.component.scss']
})
export class ReferralTableComponent implements OnChanges {
  @Input() registration: Registration = null;

  // server response
  resultsAndPagination$: Observable<ListResult<Referral>>;
  pagination: PaginationSummary = null;

  // search related
  isLoadingResults = false;
  searchResults$: Observable<ReferralSearchResults>;
  numReferrals = 0;

  constructor(
    private referralService: ReferralService,
  ) { }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.registration) {
      this.registration = changes.registration.currentValue;
      this.doSearch();
    }
  }

  private doSearch() {
    if (this.registration.id) {
      this.isLoadingResults = true;

      // go get a fresh list of registrations from the service
      const queryParams: SearchQueryParameters = {
        offset: 0,
        limit: 100, // TODO: how to make this 'infinity'?
        sort: '-startDate',
        q: `${this.registration.id}`
      };

      // get the collection of meta and data
      this.resultsAndPagination$ = this.referralService.getReferrals(queryParams);

      // process server response into something we can display in the UI
      this.searchResults$ = this.resultsAndPagination$.pipe(
        map(meta => {
          this.isLoadingResults = false;
          this.numReferrals = meta.metadata.totalCount;
          this.pagination = meta.metadata;

          // the search results need to be in this special format
          return { results: meta.data, query: '' } as ReferralSearchResults;
        })
      );
    }
  }
}
