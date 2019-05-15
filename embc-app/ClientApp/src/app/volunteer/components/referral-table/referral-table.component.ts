import { Component, OnChanges, Input } from '@angular/core';
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
  showActive = true;
  searchResults$: Observable<ReferralSearchResults>;
  pagination: PaginationSummary = null;
  referrals: Array<Referral> = [];

  constructor(
    private referralService: ReferralService,
  ) { }

  ngOnChanges() {
    this.doSearch();
  }

  doSearch() {
    // empty the previous array
    this.referrals.length = 0;

    if (this.registration && this.registration.id) {
      // get the collection of meta and data
      this.resultsAndPagination$ = this.referralService.getReferrals(this.registration.id, this.showActive);

      // process server response into something we can display in the UI
      this.searchResults$ = this.resultsAndPagination$.pipe(
        map((x: any) => {
          if (!x.registrationId || !x.referrals) {
            console.log('ERROR - invalid referral list result = ', x);
            return null;
          }

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
                from: y.validFrom,
                to: y.validTo
              }
            } as Referral;
          });

          // the search results need to be in this special format
          return { registrationId: x.registrationId, results: data } as ReferralSearchResults;
        })
      );
    }
  }

  toggleShow() {
    this.showActive = !this.showActive;
    this.doSearch();
  }

  printReferrals() {
    console.log('referrals to print =', this.referrals);
    // TODO: call BE to print referrals and return PDF (automatically open/save)
  }
}
