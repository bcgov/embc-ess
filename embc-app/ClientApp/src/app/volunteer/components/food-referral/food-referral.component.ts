import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, ViewChild, AfterViewInit, SimpleChanges, OnChanges } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import range from 'lodash/range';

import { FoodReferral, Supplier } from 'src/app/core/models';
import { ReferralDate } from 'src/app/core/models/referral-date';
import { FoodRatesComponent } from 'src/app/shared/modals/food-rates/food-rates.component';
import { SupplierComponent } from '../supplier/supplier.component';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

const BREAKFAST = 10.00;
const LUNCH = 13.00;
const DINNER = 22.00;
const GROCERIES = 22.50;

@Component({
  selector: 'app-food-referral',
  templateUrl: './food-referral.component.html',
  styleUrls: ['./food-referral.component.scss']
})
export class FoodReferralComponent extends AbstractReferralComponent<FoodReferral> implements OnInit, OnDestroy, OnChanges {
  private ratesModal: NgbModalRef = null;

  days: Array<number> = null;

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  get subType() { return this.f.subType.value; }

  get maximumAmount(): number {
    if (this.subType === 'RESTAURANT') {
      const b = this.f.numBreakfasts.value * BREAKFAST;
      const l = this.f.numLunches.value * LUNCH;
      const d = this.f.numDinners.value * DINNER;
      const n = this.selected.length;
      return (b + l + d) * n;
    }
    if (this.subType === 'GROCERIES') {
      const d = this.f.numDaysMeals.value;
      const n = this.selected.length;
      return d * GROCERIES * n;
    }
    return 0;
  }

  constructor(
    public fb: FormBuilder,
    private modals: NgbModal,
  ) {
    super(fb);
    this.form.setControl('subType', this.fb.control(''));
    this.form.setControl('numBreakfasts', this.fb.control(''));
    this.form.setControl('numLunches', this.fb.control(''));
    this.form.setControl('numDinners', this.fb.control(''));
    this.form.setControl('numDaysMeals', this.fb.control(''));
    this.form.setControl('totalAmount', this.fb.control(''));
  }

  ngOnInit() {
    super.ngOnInit(); // this is IMPORTANT! - failure to call the base class will stop validation from working!
    // this.handleFormChange();
    // this.displayReferral(this.referral as FoodReferral);
  }

  ngOnDestroy() {
    super.ngOnDestroy();

    // close modal if it's open
    if (this.ratesModal) { this.ratesModal.dismiss(); }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.referral && this.referral) {
      this.displayReferral(this.referral);
    }
    if (changes.readOnly) {
      // console.log('readOnly =', changes.readOnly.currentValue);
    }
  }

  // validate the whole form as we capture data
  // private handleFormChange(): void {
  //   this.form.valueChanges.subscribe(() => this.saveChanges());
  // }

  private displayReferral(referral: FoodReferral) {
    if (referral && !this.readOnly) {
      this.form.reset();
      this.form.patchValue({
        subType: referral.subType || null,
        numBreakfasts: referral.numBreakfasts || 0,
        numLunches: referral.numLunches || 0,
        numDinners: referral.numDinners || 0,
        numDaysMeals: referral.numDaysMeals || 0,
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
    // if RESTAURANT then assign maximumAmount; otherwise leave whatever the user entered
    if (this.subType === 'RESTAURANT') { p.totalAmount = this.maximumAmount; }
    // FIXME: Fix!!!!
    // this.referralChange.emit(p);
  }

  // NB: this is called when date component is initialized and whenever its data changes
  updateReferralDate(rd: ReferralDate) {
    this.referral.validDates = rd;

    // update array for number dropdowns
    this.days = range(1, this.referral.validDates.days + 1); // [1..n]

    // update any dropdowns that exceed max
    if (this.f.numBreakfasts.value > this.days) { this.f.numBreakfasts.setValue(+this.days); }
    if (this.f.numLunches.value > this.days) { this.f.numLunches.setValue(+this.days); }
    if (this.f.numDinners.value > this.days) { this.f.numDinners.setValue(+this.days); }
    if (this.f.numDaysMeals.value > this.days) { this.f.numDaysMeals.setValue(+this.days); }
  }

  updateSupplier(value: Supplier) {
    this.referral.supplier = value;
  }

  viewRates() {
    this.ratesModal = this.modals.open(FoodRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }

}
