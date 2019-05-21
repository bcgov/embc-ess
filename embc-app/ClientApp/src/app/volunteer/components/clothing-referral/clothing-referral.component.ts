import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { ClothingReferral, Supplier } from 'src/app/core/models';
import { ReferralDate } from 'src/app/core/models/referral-date';
import { ClothingRatesComponent } from 'src/app/shared/modals/clothing-rates/clothing-rates.component';
import { SupplierComponent } from '../supplier/supplier.component';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

const MAXIMUM_PER = 150.00;
const MAXIMUM_EXTREME = 200.00;

@Component({
  selector: 'app-clothing-referral',
  templateUrl: './clothing-referral.component.html',
  styleUrls: ['./clothing-referral.component.scss']
})
export class ClothingReferralComponent extends AbstractReferralComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() referral: ClothingReferral;
  @Output() referralChange = new EventEmitter<ClothingReferral>();

  // TODO: replace this with formReady event on supplier form
  @ViewChild(SupplierComponent) supplierRef: SupplierComponent;

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
    this.form.addControl('extremeWinterConditions', this.fb.control(''));
    this.form.addControl('totalAmount', this.fb.control(''));
  }

  ngOnInit() {
    this.handleFormChange();
    this.displayReferral(this.referral as ClothingReferral);
  }

  ngAfterViewInit() {
    // connect child form to parent
    if (this.supplierRef && !this.form.get('supplier')) {
      this.form.addControl('supplier', this.supplierRef.form);
      this.supplierRef.form.setParent(this.form);
    }
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.ratesModal) { this.ratesModal.dismiss(); }
  }

  // validate the whole form as we capture data
  private handleFormChange(): void {
    this.form.valueChanges.subscribe(() => this.saveChanges());
  }

  private displayReferral(referral: ClothingReferral) {
    if (referral) {
      this.form.reset();
      this.form.patchValue({
        subType: referral.subType || null,
        extremeWinterConditions: referral.extremeWinterConditions || false,
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
    this.referralChange.emit(p);
  }

  // NB: this is called when date component is initialized and whenever its data changes
  updateReferralDate(rd: ReferralDate) {
    this.referral.dates = rd;
  }

  updateSupplier(value: Supplier) {
    this.referral.supplier = value;
  }

  viewRates() {
    this.ratesModal = this.modals.open(ClothingRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
