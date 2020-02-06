import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { TransportationReferral } from 'app/core/models';
import { ReferralDate } from 'app/core/models/referral-date';
import { TransportationRatesComponent } from 'app/shared/modals/transportation-rates/transportation-rates.component';
import { CustomValidators } from 'app/shared/validation/custom.validators';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

@Component({
  selector: 'app-transportation-referral',
  templateUrl: './transportation-referral.component.html',
  styleUrls: ['./transportation-referral.component.scss']
})
export class TransportationReferralComponent extends AbstractReferralComponent<TransportationReferral> implements OnInit, OnDestroy {

  private ratesModal: NgbModalRef = null;

  get subType() { return this.f.subType.value; }

  constructor(
    public fb: FormBuilder,
    private modals: NgbModal,
  ) {
    super(fb);
    this.form.setControl('subType', this.fb.control(null));
    this.form.setControl('fromAddress', this.fb.control(null));
    this.form.setControl('toAddress', this.fb.control(null));
    this.form.setControl('otherTransportModeDetails', this.fb.control(null));
    this.form.setControl('totalAmount', this.fb.control(null));

    // ensure a subType is selected
    this.f.subType.setValidators([Validators.required]); // ie, not null

    // set other validators according to 'subType' value
    this.f.subType.valueChanges.subscribe((value: string) => {
      if (value === 'TAXI') {
        // remove Other validators
        this.f.otherTransportModeDetails.clearValidators();
        this.f.totalAmount.clearValidators();
        // set Taxi validators
        this.f.fromAddress.setValidators([Validators.required]);
        this.f.toAddress.setValidators([Validators.required]);
      }
      if (value === 'OTHER') {
        // remove Taxi validators
        this.f.fromAddress.clearValidators();
        this.f.toAddress.clearValidators();
        // set Other validators
        this.f.otherTransportModeDetails.setValidators([Validators.required]);
        this.f.totalAmount.setValidators([CustomValidators.number, Validators.required, Validators.min(0)]);
      }
      this.f.fromAddress.updateValueAndValidity({ emitEvent: false });
      this.f.toAddress.updateValueAndValidity({ emitEvent: false });
      this.f.otherTransportModeDetails.updateValueAndValidity({ emitEvent: false });
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

  fromModel(referral: TransportationReferral) {
    // populate fields shared by all forms; e.g. selected evacuees, comments, supplier info...
    super.fromModel(referral);
    this.form.patchValue({
      subType: referral.subType || null,
      fromAddress: referral.fromAddress,
      toAddress: referral.toAddress,
      otherTransportModeDetails: referral.otherTransportModeDetails,
      totalAmount: this.numberOrNull(referral.totalAmount)
    });
  }

  toModel(formValue: any): TransportationReferral {
    const p = super.toModel(formValue);
    // if TAXI then don't send totalAmount to BE
    if (this.subType === 'TAXI') { delete p.totalAmount; }
    return p;
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
