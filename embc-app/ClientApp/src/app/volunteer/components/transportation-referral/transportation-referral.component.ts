import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { TransportationReferral, Supplier } from 'src/app/core/models';
import { ReferralDate } from 'src/app/core/models/referral-date';
import { TransportationRatesComponent } from 'src/app/shared/modals/transportation-rates/transportation-rates.component';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

@Component({
  selector: 'app-transportation-referral',
  templateUrl: './transportation-referral.component.html',
  styleUrls: ['./transportation-referral.component.scss']
})
export class TransportationReferralComponent extends AbstractReferralComponent<TransportationReferral> implements OnInit, OnDestroy {
  private ratesModal: NgbModalRef = null;

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  get subType() { return this.f.subType.value; }

  constructor(
    public fb: FormBuilder,
    private modals: NgbModal,
  ) {
    super(fb);
    this.form.setControl('subType', this.fb.control(''));
    this.form.setControl('fromAddress', this.fb.control(''));
    this.form.setControl('toAddress', this.fb.control(''));
    this.form.setControl('otherTransportModeDetails', this.fb.control(''));
    this.form.setControl('totalAmount', this.fb.control(''));
  }

  ngOnInit() {
    // this is IMPORTANT! - failure to call the base class will stop validation from working!
    super.ngOnInit();
  }

  ngOnDestroy() {
    super.ngOnDestroy();
    // close modal if it's open
    if (this.ratesModal) { this.ratesModal.dismiss(); }
  }

  fromModel(referral: TransportationReferral) {
    // populate fields shared by all forms; e.g. selected evacuees, comments, supplier info...
    super.fromModel(referral);
    this.form.patchValue({
      subType: referral.subType || null,
      fromAddress: referral.fromAddress,
      toAddress: referral.toAddress,
      otherTransportModeDetails: referral.otherTransportModeDetails,
      totalAmount: referral.totalAmount || 0
    });
  }

  // NB: this is called when date component is initialized and whenever its data changes
  updateReferralDate(rd: ReferralDate) {
    this.referral.validDates = rd;
  }

  viewRates() {
    this.ratesModal = this.modals.open(TransportationRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
