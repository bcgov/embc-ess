import { Component, OnChanges, Input, SimpleChanges } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ReferralService } from 'src/app/core/services/referral.service';
import { Registration, ListResult, PaginationSummary, Referral } from 'src/app/core/models';
import { ReferralSearchResults } from 'src/app/core/models/search-interfaces';

@Component({
  selector: 'app-referral-table',
  templateUrl: './referral-table.component.html',
  styleUrls: ['./referral-table.component.scss']
})
export class ReferralTableComponent implements OnChanges {
  @Input() registration: Registration = null;

  // server response
  resultsAndPagination$: Observable<ListResult<Referral>>;

  // search related
  isLoadingResults = false;
  searchResults$: Observable<ReferralSearchResults>;
  pagination: PaginationSummary = null;

  constructor(
    private referralService: ReferralService,
  ) { }

  ngOnChanges(changes: SimpleChanges) {
    if (this.registration && this.registration.id) {
      this.isLoadingResults = true;

      // get the collection of meta and data
      this.resultsAndPagination$ = this.referralService.getReferrals(this.registration.id);

      // process server response into something we can display in the UI
      this.searchResults$ = this.resultsAndPagination$.pipe(
        map((x: any) => {
          this.isLoadingResults = false;
          this.pagination = x.referrals.metadata;

          // the search results need to be in this special format
          return { results: x.referrals.items, registrationId: x.registrationId } as ReferralSearchResults;
        })
      );
    }
  }
}
