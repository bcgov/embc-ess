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
  referrals: Array<Referral> = [];

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

          // convert data[] to Referral[]
          const data: Array<Referral> = x.referrals.data.map((y: any) => {
            return {
              active: y.active,
              id: y.referralId,
              subType: y.subType,
              supplier: { name: y.supplier.name },
              type: y.type,
              dates: {
                to: y.validTo,
                from: y.validFrom,
              }
              // validFrom: y.validFrom,
              // validTo: y.validTo
            } as Referral;
          });

          // the search results need to be in this special format
          return { registrationId: x.registrationId, results: data } as ReferralSearchResults;
        })
      );
    }
  }

  printReferrals() {
    console.log('referrals to print =', this.referrals);
    // TODO: call BE to print referrals and return PDF (automatically open/save)
  }
}
