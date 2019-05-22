import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import range from 'lodash/range';

import { FoodReferral } from 'src/app/core/models';
import { ReferralDate } from 'src/app/core/models/referral-date';
import { FoodRatesComponent } from 'src/app/shared/modals/food-rates/food-rates.component';
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
export class FoodReferralComponent extends AbstractReferralComponent<FoodReferral> implements OnInit, OnDestroy {
  private ratesModal: NgbModalRef = null;

  days: Array<number> = null;

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  get subType() { return this.f.subType.value; }

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
    // this is IMPORTANT! - failure to call the base class will stop validation from working!
    super.ngOnInit();
  }

  ngOnDestroy() {
    super.ngOnDestroy();
    // close modal if it's open
    if (this.ratesModal) { this.ratesModal.dismiss(); }
  }

  fromModel(referral: FoodReferral) {
    super.fromModel(referral);
    this.form.patchValue({
      subType: referral.subType || null,
      numBreakfasts: referral.numBreakfasts || 0,
      numLunches: referral.numLunches || 0,
      numDinners: referral.numDinners || 0,
      numDaysMeals: referral.numDaysMeals || 0,
      totalAmount: referral.totalAmount || 0
    });
  }

  toModel(formValue: any): FoodReferral {
    const p = super.toModel(formValue);
    // if RESTAURANT then assign maximumAmount; otherwise leave whatever the user entered
    if (this.subType === 'RESTAURANT') { p.totalAmount = this.maximumAmount(this.form); }
    return p;
  }

  // NB: this is called when date component is initialized and whenever its data changes
  updateReferralDate(rd: ReferralDate) {
    this.referral.validDates = rd;

    // update array for number dropdowns
    this.days = range(1, this.referral.validDates.days + 1); // [1..n]

    // update any dropdowns that exceed max
    if (this.f.numBreakfasts.value > this.days) { this.f.numBreakfasts.setValue(+this.referral.validDates.days); }
    if (this.f.numLunches.value > this.days) { this.f.numLunches.setValue(+this.referral.validDates.days); }
    if (this.f.numDinners.value > this.days) { this.f.numDinners.setValue(+this.referral.validDates.days); }
    if (this.f.numDaysMeals.value > this.days) { this.f.numDaysMeals.setValue(+this.referral.validDates.days); }
  }

  maximumAmount(x: FormGroup | FoodReferral): number {
    if (x) {
      if (x instanceof FormGroup) {
        // get data from form
        if (x.value.subType === 'RESTAURANT') {
          const b = x.value.numBreakfasts * BREAKFAST;
          const l = x.value.numLunches * LUNCH;
          const d = x.value.numDinners * DINNER;
          const n = x.value.evacuees.length;
          return (b + l + d) * n;
        }
        if (x.value.subType === 'GROCERIES') {
          const d = x.value.numDaysMeals;
          const n = x.value.evacuees.length;
          return d * GROCERIES * n;
        }
      } else {
        // get data from referral
        if (x.subType === 'RESTAURANT') {
          const b = x.numBreakfasts * BREAKFAST;
          const l = x.numLunches * LUNCH;
          const d = x.numDinners * DINNER;
          const n = x.evacuees.length;
          return (b + l + d) * n;
        }
        if (x.subType === 'GROCERIES') {
          const d = x.numDaysMeals;
          const n = x.evacuees.length;
          return d * GROCERIES * n;
        }
      }
    }
    return 0;
  }

  viewRates() {
    this.ratesModal = this.modals.open(FoodRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }

}
