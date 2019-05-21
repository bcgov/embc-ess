import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { ClothingReferral } from 'src/app/core/models';
import { ReferralDate } from 'src/app/core/models/referral-date';
import { ClothingRatesComponent } from 'src/app/shared/modals/clothing-rates/clothing-rates.component';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

const MAXIMUM_PER = 150.00;
const MAXIMUM_EXTREME = 200.00;

@Component({
  selector: 'app-clothing-referral',
  templateUrl: './clothing-referral.component.html',
  styleUrls: ['./clothing-referral.component.scss']
})
export class ClothingReferralComponent extends AbstractReferralComponent<ClothingReferral> implements OnInit, OnDestroy {
  private ratesModal: NgbModalRef = null;

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  get maximumAmount(): number {
    const n = this.selected.length;
    return this.f.extremeWinterConditions.value
      ? (n * MAXIMUM_EXTREME)
      : (n * MAXIMUM_PER);
  }

  constructor(
    public fb: FormBuilder,
    private modals: NgbModal,
  ) {
    super(fb);
    this.form.setControl('extremeWinterConditions', this.fb.control(''));
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

  fromModel(referral: ClothingReferral) {
    // populate fields shared by all forms; e.g. selected evacuees, comments, supplier info...
    super.fromModel(referral);
    this.form.patchValue({
      subType: referral.subType || null,
      extremeWinterConditions: referral.extremeWinterConditions || false,
      totalAmount: referral.totalAmount || 0
    });
  }

  // NB: this is called when date component is initialized and whenever its data changes
  updateReferralDate(rd: ReferralDate) {
    this.referral.validDates = rd;
  }

  viewRates() {
    this.ratesModal = this.modals.open(ClothingRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
