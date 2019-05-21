import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import range from 'lodash/range';

import { LodgingReferral, Supplier } from 'src/app/core/models';
import { ReferralDate } from 'src/app/core/models/referral-date';
import { LodgingRatesComponent } from 'src/app/shared/modals/lodging-rates/lodging-rates.component';
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

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  get subType() { return this.f.subType.value; }

  constructor(
    public fb: FormBuilder,
    private modals: NgbModal,
  ) {
    super(fb);
    this.form.setControl('subType', this.fb.control(''));
    this.form.setControl('numNights', this.fb.control(''));
    this.form.setControl('numRooms', this.fb.control(''));
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
      numNights: referral.numNights || 0,
      numRooms: referral.numRooms || 0,
    });
  }

  // NB: this is called when date component is initialized and whenever its data changes
  updateReferralDate(rd: ReferralDate) {
    this.referral.validDates = rd;

    // update array for number dropdowns
    this.nights = range(1, this.referral.validDates.days + 1); // [1..n]

    // update any dropdowns that exceed max
    if (this.f.numNights.value > this.nights) { this.f.numNights.setValue(+this.nights); }
  }

  viewRates() {
    this.ratesModal = this.modals.open(LodgingRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
