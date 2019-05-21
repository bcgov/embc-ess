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
    this.form.addControl('subType', this.fb.control(''));
    this.form.addControl('fromAddress', this.fb.control(''));
    this.form.addControl('toAddress', this.fb.control(''));
    this.form.addControl('modeTransport', this.fb.control(''));
    this.form.addControl('totalAmount', this.fb.control(''));
  }

  ngOnInit() {
    this.handleFormChange();
    this.displayReferral(this.referral as TransportationReferral);
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.ratesModal) { this.ratesModal.dismiss(); }
  }

  // validate the whole form as we capture data
  private handleFormChange(): void {
    this.form.valueChanges.subscribe(() => this.saveChanges());
  }

  private displayReferral(referral: TransportationReferral) {
    if (referral) {
      this.form.reset();
      this.form.patchValue({
        subType: referral.subType || null,
        fromAddress: referral.fromAddress,
        toAddress: referral.toAddress,
        modeTransport: referral.modeTransport,
        comments: referral.comments,
        totalAmount: referral.totalAmount || 0
      });

      // populate the evacuee list with existing selection
      (referral.evacuees || []).forEach(x => this.selectEvacuee(x));
    }
  }

  // if all required information is in the form we emit
  private saveChanges() {
    if (!this.form.valid) {
      console.log('form is invalid'); // TODO: fix
      // return;
    }

    // Copy over all of the original referral properties.
    // Then copy over the values from the form.
    // This ensures values not on the form, such as the Id, are retained.
    const p = { ...this.referral, ...this.form.value };
    // FIXME:
    // this.referralChange.emit(p);
  }

  // NB: this is called when date component is initialized and whenever its data changes
  updateReferralDate(rd: ReferralDate) {
    this.referral.dates = rd;
  }

  updateSupplier(value: Supplier) {
    this.referral.supplier = value;
  }

  viewRates() {
    this.ratesModal = this.modals.open(TransportationRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
