import { Component, OnInit, TemplateRef, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { RegistrationService } from '../core/services/registration.service';
import { Registration } from '../core/models';
import { AuthService } from '../core/services/auth.service';
import { UniqueKeyService } from '../core/services/unique-key.service';

@Component({
  selector: 'app-registration-summary',
  templateUrl: './registration-summary.component.html',
  styleUrls: ['./registration-summary.component.scss']
})
export class RegistrationSummaryComponent implements OnInit, OnDestroy {

  private confirmModal: NgbModalRef = null;
  registration: Registration = null;
  path: string = null; // for relative routing
  selectedPurchaser = 'null';
  otherPurchaser: string = null;
  loading = true;
  reason = 'null';

  get purchaser() {
    const otherPurchaser = this.otherPurchaser ? this.otherPurchaser.trim() : null;
    return (this.selectedPurchaser === 'other') ? otherPurchaser : (this.selectedPurchaser !== 'null') ? this.selectedPurchaser : null;
  }

  constructor(
    private router: Router,
    private modals: NgbModal,
    private registrationService: RegistrationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // get path for routing
    this.authService.path.subscribe(p => this.path = p);

    // get lookup key and load registration data
    const key = this.uniqueKeyService.getKey();
    if (key) {
      this.registrationService.getRegistrationSummaryById(key)
        .subscribe(value => {
          this.loading = false;
          if (!value.id || !value.essFileNumber) {
            console.log('ERROR - invalid registration object = ', value);
            // done with the key. It was useless. Clear the reference key.
            this.uniqueKeyService.clearKey();
            this.goHome();
          } else {
            // store the registration object
            this.registration = value;
          }
        }, err => {
          this.loading = false;
          alert(`err = ${err}`);
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
    // send them back to their home page
    this.router.navigate([`/${this.path}`]);
  }

  showFullProfile(content: TemplateRef<any>) {
    this.confirmModal = this.modals.open(content);

    // handle result
    this.confirmModal.result.then(() => {
      // modal was closed

      this.confirmModal = null; // clear for next time

      // save the key for lookup
      this.uniqueKeyService.setKey(this.registration.id);
      this.router.navigate([`/${this.path}/registration/summary/full`]);
    }, () => {
      // modal was dismissed
      this.confirmModal = null; // clear for next time
    });
  }

  addReferrals() {
    this.router.navigate([`/${this.path}/referrals`, this.registration.id, this.purchaser]);
  }

}
