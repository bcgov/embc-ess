import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import range from 'lodash/range';

import { LodgingReferral, Supplier } from 'src/app/core/models';
import { ReferralDate } from 'src/app/core/models/referral-date';
import { LodgingRatesComponent } from 'src/app/shared/modals/lodging-rates/lodging-rates.component';
import { SupplierComponent } from '../supplier/supplier.component';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

@Component({
  selector: 'app-lodging-referral',
  templateUrl: './lodging-referral.component.html',
  styleUrls: ['./lodging-referral.component.scss']
})
export class LodgingReferralComponent extends AbstractReferralComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() referral: LodgingReferral;
  @Output() referralChange = new EventEmitter<LodgingReferral>();

  // TODO: replace this with formReady event on supplier form
  @ViewChild(SupplierComponent) supplierRef: SupplierComponent;

  private ratesModal: NgbModalRef = null;

  nights: Array<number> = null;
  rooms: Array<number> = range(1, 11); // [1..10]

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  get subType() { return this.f.subType.value; }

  constructor(
    public fb: FormBuilder,
    private modals: NgbModal,
  ) {
    super(fb);
    this.form.addControl('subType', this.fb.control(''));
    this.form.addControl('numNights', this.fb.control(''));
    this.form.addControl('numRooms', this.fb.control(''));
  }

  ngOnInit() {
    this.handleFormChange();
    this.displayReferral(this.referral as LodgingReferral);
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

  private displayReferral(referral: LodgingReferral) {
    if (referral) {
      this.form.reset();
      this.form.patchValue({
        subType: referral.subType || null,
        numNights: referral.numNights || 0,
        numRooms: referral.numRooms || 0,
        comments: referral.comments,
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

    // update array for number dropdowns
    this.nights = range(1, this.referral.dates.days + 1); // [1..n]

    // update any dropdowns that exceed max
    if (this.f.numNights.value > this.nights) { this.f.numNights.setValue(+this.nights); }
  }

  updateSupplier(value: Supplier) {
    this.referral.supplier = value;
  }

  viewRates() {
    this.ratesModal = this.modals.open(LodgingRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
