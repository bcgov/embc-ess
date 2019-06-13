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

  private path: string = null; // the base path for routing
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
    this.authService.path.subscribe((path: string) => this.path = path);

    // get and verify URL params
    this.registrationId = this.route.snapshot.paramMap.get('regId');
    this.referralId = this.route.snapshot.paramMap.get('refId');

    if (!this.registrationId || !this.referralId) {
      alert(`err = invalid registration ID or referral ID`);
      this.back();
    }

    // get referral data
    this.referralService.getReferralById(this.registrationId, this.referralId)
      .subscribe((referral: any) => {
        this.loading = false;
        if (!referral.registrationId || !referral.referral) {
          console.log('ERROR - invalid referral object = ', referral);
          this.goHome();
        } else {
          // HACK for BE returning id instead of referralId
          const { id, ...other } = referral.referral;
          this.referral = { referralId: id, ...other };
        }
      }, err => {
        this.loading = false;
        this.notifications.addNotification('Failed to load referral', 'danger');
        console.log('error getting referral =', err);
        this.goHome();
      });
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.confirmModal) { this.confirmModal.dismiss(); }
  }

  private goHome() {
    // go to home page
    this.router.navigate([`/${this.path}`]);
  }

  back() {
    // save registration ID for lookup in the new component
    this.uniqueKeyService.setKey(this.registrationId);

    // go to registration summary page
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
          this.deactivating = false;
          this.notifications.addNotification('Referral voided successfully', 'success');
          // return to summary page
          this.back();
        }, err => {
          this.deactivating = false;
          this.notifications.addNotification('Failed to void referral', 'danger');
          console.log('error deactivating referral =', err);
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
