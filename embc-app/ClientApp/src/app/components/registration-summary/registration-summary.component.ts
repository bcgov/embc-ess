import { Component, OnInit, TemplateRef, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { RegistrationService } from 'src/app/core/services/registration.service';
import { Registration, RegistrationSummary } from 'src/app/core/models';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { NotificationQueueService } from 'src/app/core/services/notification-queue.service';
import * as moment from 'moment';
import { ReadOnlyService } from '../../core/services/read-only.service';

@Component({
  templateUrl: './registration-summary.component.html',
  styleUrls: ['./registration-summary.component.scss']
})
export class RegistrationSummaryComponent implements OnInit, OnDestroy {

  confirmModal: NgbModalRef = null;
  registrationSummary: RegistrationSummary = null;
  path: string = null; // the base path for routing
  selectedPurchaser: string = null;
  otherPurchaser: string = null;
  loading = true;
  reason: string = null;
  canAddReferrals = true;
  // This is here for nicer boolean logic in the template (e.g. disabled = referralsDisabled, rather than !canAddReferrals).
  get referralsDisabled() { return !this.canAddReferrals; }

  constructor(
    private router: Router,
    private modals: NgbModal,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private notificationQueueService: NotificationQueueService,
    public readOnlyService: ReadOnlyService
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe((path: string) => this.path = path);

    // get lookup key and load registration data
    const key = this.uniqueKeyService.getKey();
    if (key) {
      this.registrationService.getRegistrationSummaryById(key)
        .subscribe((registration: RegistrationSummary) => {
          this.loading = false;
          if (registration.incidentTask && registration.incidentTask.taskNumberEndDate) {
            // Cannot add tasks if the task number end date has passed
            this.canAddReferrals = moment.utc().isBefore(moment.utc(registration.incidentTask.taskNumberEndDate));
          }
          if (!registration.id || !registration.essFileNumber) {
            console.log('ERROR - invalid registration object =', registration);
            // Done with the key. It was useless. Clear it.
            this.uniqueKeyService.clearKey();

            this.goHome();
          } else {
            // store the registration object
            this.registrationSummary = registration;
          }
        }, err => {
          this.loading = false;

          this.notificationQueueService.addNotification('Failed to load evacuee summary', 'danger');
          console.log('error getting registration summary =', err);
          this.goHome();
        });
    } else {
      // key was not found
      this.loading = false;
      this.goHome();
    }
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.confirmModal) { this.confirmModal.dismiss(); }
  }

  private goHome() {
    // go to home page
    this.router.navigate([`/${this.path}`]);
  }

  showFullProfile(content: TemplateRef<any>) {
    this.confirmModal = this.modals.open(content, { centered: true });

    // handle result
    this.confirmModal.result.then(() => {
      // modal was closed
      this.confirmModal = null; // clear for next time

      // save registration ID for lookup in the new component
      this.uniqueKeyService.setKey(this.registrationSummary.id);

      // go to registration view page
      this.router.navigate([`/${this.path}/registration/summary/full`, { reason: this.reason }]);
    }, () => {
      // modal was dismissed
      this.confirmModal = null; // clear for next time
    });
  }

  getPurchaser() {
    const otherPurchaser = this.otherPurchaser ? this.otherPurchaser.trim() : null;
    return (this.selectedPurchaser === 'other') ? otherPurchaser : this.selectedPurchaser;
  }


  addReferrals() {
    if (this.canAddReferrals) {
      this.router.navigate([`/${this.path}/referrals`, this.registrationSummary.id, this.getPurchaser()]);
    }
    else {
      this.notificationQueueService.addNotification('Cannot add referrals against a closed task!', 'danger');
    }
  }

}
