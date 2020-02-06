import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import range from 'lodash/range';

import { LodgingReferral } from 'app/core/models';
import { ReferralDate } from 'app/core/models/referral-date';
import { LodgingRatesComponent } from 'app/shared/modals/lodging-rates/lodging-rates.component';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

@Component({
  selector: 'app-lodging-referral',
  templateUrl: './lodging-referral.component.html',
  styleUrls: ['./lodging-referral.component.scss']
})
export class LodgingReferralComponent extends AbstractReferralComponent<LodgingReferral> implements OnInit, OnDestroy {

  private ratesModal: NgbModalRef = null;

  nights: Array<number> = null;
  rooms: Array<number> = range(1, 11); // [1..10]

  get subType() { return this.f.subType.value; }

  constructor(
    public fb: FormBuilder,
    private modals: NgbModal,
  ) {
    super(fb);
    this.form.setControl('subType', this.fb.control(null));
    this.form.setControl('numNights', this.fb.control(null));
    this.form.setControl('numRooms', this.fb.control(null));

    // ensure a subType is selected
    this.f.subType.setValidators([Validators.required]); // ie, not null

    // set other validators according to 'subType' value
    this.f.subType.valueChanges.subscribe((value: string) => {
      if (value === 'HOTEL') {
        // remove other validators
        this.f.numNights.clearValidators();
        // set Hotel validators
        this.f.numNights.setValidators([Validators.required]);
        this.f.numRooms.setValidators([Validators.required]);
      }
      if (value === 'BILLETING') {
        // remove other validators
        this.f.numNights.clearValidators();
        this.f.numRooms.clearValidators();
        // set Billeting validators
        this.f.numNights.setValidators([Validators.required]);
      }
      if (value === 'GROUP') {
        // remove other validators
        this.f.numNights.clearValidators();
        this.f.numRooms.clearValidators();
        // set Billeting validators
        this.f.numNights.setValidators([Validators.required]);
      }
      this.f.numNights.updateValueAndValidity({ emitEvent: false });
      this.f.numRooms.updateValueAndValidity({ emitEvent: false });
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

  fromModel(referral: LodgingReferral) {
    // populate fields shared by all forms; e.g. selected evacuees, comments, supplier info...
    super.fromModel(referral);
    this.form.patchValue({
      subType: referral.subType || null,
      numNights: this.numberOrNull(referral.numNights),
      numRooms: this.numberOrNull(referral.numRooms)
    });
  }

  // NB: this is called when date component is initialized and whenever its data changes
  updateReferralDate(rd: ReferralDate) {
    this.referral.validDates = rd;

    // update array for number dropdowns
    const max = this.referral.validDates.days;
    this.nights = range(1, max + 1); // [1..max]

    // update any dropdowns that exceed max
    if (this.f.numNights.value > max) { this.f.numNights.setValue(max); }
  }

  viewRates() {
    this.ratesModal = this.modals.open(LodgingRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }

}
