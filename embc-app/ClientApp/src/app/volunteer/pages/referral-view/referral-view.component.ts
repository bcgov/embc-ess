import { Component, OnInit, OnDestroy, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { ReferralService } from 'src/app/core/services/referral.service';
import { NotificationQueueService } from 'src/app/core/services/notification-queue.service';
import {
  Referral, isLodgingReferral, isClothingReferral,
  isFoodReferral, isIncidentalsReferral, isTransportationReferral
} from 'src/app/core/models';

@Component({
  selector: 'app-referral-view',
  templateUrl: './referral-view.component.html',
  styleUrls: ['./referral-view.component.scss']
})
export class ReferralViewComponent implements OnInit, OnDestroy {

  registrationId: string = null;
  referralId: string = null;
  referral: Referral = null;
  loading = true;
  deactivating = false;
  reason = null;

  private path: string = null; // for relative routing
  private confirmModal: NgbModalRef = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private modals: NgbModal,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private referralService: ReferralService,
    private notifications: NotificationQueueService,
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe(p => this.path = p);

    // get and verify URL params
    this.registrationId = this.route.snapshot.paramMap.get('regId');
    this.referralId = this.route.snapshot.paramMap.get('refId');

    if (!this.registrationId || !this.referralId) {
      alert(`err = invalid registration ID or referral ID`);
      this.back();
    }

    // get referral data
    this.referralService.getReferralById(this.registrationId, this.referralId)
      .subscribe((x: any) => {
        this.loading = false;
        if (!x.registrationId || !x.referral) {
          console.log('ERROR - invalid referral object = ', x);
          this.goHome();
        } else {
          this.referral = x.referral;
          this.referral.dates = { from: x.referral.validFrom, to: x.referral.validTo };
        }
      }, err => {
        this.loading = false;
        alert(`err = ${err}`);
        this.goHome();
      });
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.confirmModal) { this.confirmModal.dismiss(); }
  }

  private goHome() {
    // go back to their home page
    this.router.navigate([`/${this.path}`]);
  }

  back() {
    // go back to summary page
    this.uniqueKeyService.setKey(this.registrationId);
    this.router.navigate([`/${this.path}/registration/summary`]);
  }

  onDeactivateCancel(): void {
    this.confirmModal.dismiss();
  }

  onDeactivateConfirm(reason: string): void {
    // TODO: Send reason code to server - once API becomes available
    this.confirmModal.close();
  }

  deactivate(content: TemplateRef<any>) {
    this.deactivating = true;

    this.confirmModal = this.modals.open(content, { centered: true });

    // handle result
    this.confirmModal.result.then(() => {
      // modal was closed

      this.confirmModal = null; // clear for next time

      this.referralService.deactivateReferral(this.registrationId, this.referralId)
        .subscribe(() => {
          // deactivate succeeded
          this.deactivating = false;
          this.notifications.addNotification('Referral deactivated successfully');
          // return to summary page
          this.back();
        }, err => {
          // deactivate failed
          this.deactivating = false;
          alert(`err = ${err}`);
          this.goHome();
        });
    }, () => {
      // modal was dismissed
      this.deactivating = false;
      this.confirmModal = null; // clear for next time
    });
  }

  // --------------------HELPERS-----------------------------------------
  isLodgingReferral(referral: Referral): boolean {
    return isLodgingReferral(referral);
  }

  isClothingReferral(referral: Referral): boolean {
    return isClothingReferral(referral);
  }

  isFoodReferral(referral: Referral): boolean {
    return isFoodReferral(referral);
  }

  isIncidentalsReferral(referral: Referral): boolean {
    return isIncidentalsReferral(referral);
  }

  isTransportationReferral(referral: Referral): boolean {
    return isTransportationReferral(referral);
  }

}
