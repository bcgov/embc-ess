import { Component, OnChanges, OnDestroy, ViewChild, TemplateRef, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { ReferralService } from 'src/app/core/services/referral.service';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { Registration, ListResult, PaginationSummary, Referral } from 'src/app/core/models';
import { ReferralSearchResults } from 'src/app/core/models/search-interfaces';

@Component({
  selector: 'app-referral-table',
  templateUrl: './referral-table.component.html',
  styleUrls: ['./referral-table.component.scss']
})
export class ReferralTableComponent implements OnChanges, OnDestroy {

  @ViewChild('summaryAlert') summaryAlert: TemplateRef<any>;
  @Input() registration: Registration = null;

  // server response
  resultsAndPagination$: Observable<ListResult<Referral>>;

  // search related
  showActive = true;
  searchResults$: Observable<ReferralSearchResults>;
  pagination: PaginationSummary = null;
  referrals: Array<Referral> = [];

  summaryModal: NgbModalRef = null;
  includeSummary: boolean;

  constructor(
    private referralService: ReferralService,
    private registrationService: RegistrationService,
    private modals: NgbModal,
  ) { }

  ngOnChanges() {
    this.doSearch();
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.summaryModal) { this.summaryModal.dismiss(); }
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
              referralId: y.referralId,
              subType: y.subType,
              supplier: { name: y.supplier.name },
              type: y.type,
              validDates: {
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
    this.summaryModal = this.modals.open(this.summaryAlert, { centered: true, windowClass: 'modal-small' });

    // handle result
    this.summaryModal.result.then(
      (includeSummary: boolean) => {
        // modal was closed
        this.summaryModal = null;

        const referralIds = this.referrals.map(r => r.referralId);
        this.registrationService.printReferrals(this.registration.id, referralIds, includeSummary);
      },
      () => {
        // modal was dismissed
        this.summaryModal = null;
      }
    );
  }

}
