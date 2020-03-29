import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import range from 'lodash/range';

import { FoodReferral } from 'src/app/core/models';
import { ReferralDate } from 'src/app/core/models/referral-date';
import { FoodRatesComponent } from 'src/app/shared/modals/food-rates/food-rates.component';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';
import { RESTAURANT_BREAKFAST, RESTAURANT_LUNCH, RESTAURANT_DINNER, GROCERIES } from 'src/app/constants/rates';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

@Component({
  selector: 'app-food-referral',
  templateUrl: './food-referral.component.html',
  styleUrls: ['./food-referral.component.scss']
})
export class FoodReferralComponent extends AbstractReferralComponent<FoodReferral> implements OnInit, OnDestroy {

  private ratesModal: NgbModalRef = null;

  days: Array<number> = null;

  get subType() { return this.f.subType.value; }

  constructor(
    public fb: FormBuilder,
    private modals: NgbModal,
  ) {
    super(fb);
    this.form.setControl('subType', this.fb.control(null));
    this.form.setControl('numBreakfasts', this.fb.control(null));
    this.form.setControl('numLunches', this.fb.control(null));
    this.form.setControl('numDinners', this.fb.control(null));
    this.form.setControl('numDaysMeals', this.fb.control(null));
    this.form.setControl('totalAmount', this.fb.control(null));

    // ensure a subType is selected
    this.f.subType.setValidators([Validators.required]); // ie, not null

    // set other validators according to 'subType' value
    this.f.subType.valueChanges.subscribe((value: string) => {
      if (value === 'RESTAURANT') {
        // remove Groceries validators
        this.f.numDaysMeals.clearValidators();
        this.f.totalAmount.clearValidators();
        // set Restaurant validators
        this.f.numBreakfasts.setValidators([Validators.required]);
        this.f.numLunches.setValidators([Validators.required]);
        this.f.numDinners.setValidators([Validators.required]);
      }
      if (value === 'GROCERIES') {
        // remove Restaurant validators
        this.f.numBreakfasts.clearValidators();
        this.f.numLunches.clearValidators();
        this.f.numDinners.clearValidators();
        // set Groceries validators
        this.f.numDaysMeals.setValidators([Validators.required]);
        this.f.totalAmount.setValidators([CustomValidators.number, Validators.required, Validators.min(0)]);
      }
      this.f.numBreakfasts.updateValueAndValidity({ emitEvent: false });
      this.f.numLunches.updateValueAndValidity({ emitEvent: false });
      this.f.numDinners.updateValueAndValidity({ emitEvent: false });
      this.f.numDaysMeals.updateValueAndValidity({ emitEvent: false });
      this.f.totalAmount.updateValueAndValidity({ emitEvent: false });
    });
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
      numBreakfasts: this.numberOrNull(referral.numBreakfasts),
      numLunches: this.numberOrNull(referral.numLunches),
      numDinners: this.numberOrNull(referral.numDinners),
      numDaysMeals: this.numberOrNull(referral.numDaysMeals),
      totalAmount: this.numberOrNull(referral.totalAmount)
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
    const max = this.referral.validDates.days;
    this.days = range(0, max + 2); // [0..max]

    // update any dropdowns that exceed max
    if (this.f.numBreakfasts.value > max) { this.f.numBreakfasts.setValue(max); }
    if (this.f.numLunches.value > max) { this.f.numLunches.setValue(max); }
    if (this.f.numDinners.value > max) { this.f.numDinners.setValue(max); }
    if (this.f.numDaysMeals.value > max) { this.f.numDaysMeals.setValue(max); }
  }

  maximumAmount(x: FormGroup | FoodReferral): number {
    if (x) {
      if (x instanceof FormGroup) {
        // get data from form
        if (x.value.subType === 'RESTAURANT') {
          const b = x.value.numBreakfasts * RESTAURANT_BREAKFAST;
          const l = x.value.numLunches * RESTAURANT_LUNCH;
          const d = x.value.numDinners * RESTAURANT_DINNER;
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
          const b = x.numBreakfasts * RESTAURANT_BREAKFAST;
          const l = x.numLunches * RESTAURANT_LUNCH;
          const d = x.numDinners * RESTAURANT_DINNER;
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
