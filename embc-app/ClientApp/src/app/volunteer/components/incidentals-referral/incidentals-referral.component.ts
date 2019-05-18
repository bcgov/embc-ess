import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges, ViewChild, AfterViewInit } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { IncidentalsReferral, Supplier } from 'src/app/core/models';
import { IncidentalsRatesComponent } from 'src/app/shared/modals/incidentals-rates/incidentals-rates.component';
import { FormBuilder, Validators } from '@angular/forms';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

const MAXIMUM_PER_PERSON = 50.00;

@Component({
  selector: 'app-incidentals-referral',
  templateUrl: './incidentals-referral.component.html',
  styleUrls: ['./incidentals-referral.component.scss']
})
export class IncidentalsReferralComponent extends AbstractReferralComponent<IncidentalsReferral> implements OnInit, OnDestroy, OnChanges {
  private ratesModal: NgbModalRef = null;

  get maximumAmount() {
    const n = this.selected.length;
    return (n * MAXIMUM_PER_PERSON);
  }

  constructor(
    public fb: FormBuilder,
    public modals: NgbModal,
  ) {
    // call base form to setup shared form fields; e.g. evacuee list, comments
    super(fb);

    // add more fields that are specific to this form
    this.form.setControl('approvedItems', this.fb.control(''));
    this.form.setControl('totalAmount', this.fb.control('', [CustomValidators.number, Validators.required, Validators.min(0)]));
  }

  ngOnInit() {
    super.ngOnInit(); // this is IMPORTANT! - failure to call the base class will stop validation from working!

    // TODO: Review these below...
    // this.handleFormChange();
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
  // handleFormChange(): void {
  //   this.form.valueChanges.subscribe(() => this.saveChanges());
  // }

  displayReferral(referral: IncidentalsReferral) {
    if (referral && !this.readOnly) {
      this.form.reset();
      this.form.patchValue({
        evacuees: referral.evacuees,
        approvedItems: referral.approvedItems,
        totalAmount: referral.totalAmount,
        comments: referral.comments,
        // supplier: referral.supplier,
      });

      // populate the evacuee list with existing selection
      (referral.evacuees || []).forEach(x => this.selectEvacuee(x));

      // TODO: Dates FROM and TO
    }
  }

  // if all required information is in the form we emit
  // saveChanges() {
  //   if (!this.form.valid) {
  //     return;
  //   }
  //   // Copy over all of the original referral properties
  //   // Then copy over the values from the form
  //   // This ensures values not on the form, such as the Id, are retained
  //   const p: IncidentalsReferral = { ...this.referral, ...this.form.value };
  //   this.propagateChanges(p);
  // }

  updateSupplier(value: Supplier) {
    this.referral.supplier = value;
  }

  viewRates() {
    this.ratesModal = this.modals.open(IncidentalsRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }

}
