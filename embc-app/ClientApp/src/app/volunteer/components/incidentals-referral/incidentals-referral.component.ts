import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges, ViewChild, AfterViewInit } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { IncidentalsReferral } from 'src/app/core/models';
import { IncidentalsRatesComponent } from 'src/app/shared/modals/incidentals-rates/incidentals-rates.component';
import { numberOfDays, uuid } from 'src/app/shared/utils';
import { SupplierComponent } from '../supplier/supplier.component';
import { FormBuilder, Validators } from '@angular/forms';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

@Component({
  selector: 'app-incidentals-referral',
  templateUrl: './incidentals-referral.component.html',
  styleUrls: ['./incidentals-referral.component.scss']
})
export class IncidentalsReferralComponent extends AbstractReferralComponent implements OnInit, AfterViewInit, OnDestroy, OnChanges {
  @Input() showErrorsWhen = true;
  @Input() referral: IncidentalsReferral = null;
  @ViewChild(SupplierComponent) supplier: SupplierComponent;

  private ratesModal: NgbModalRef = null;

  // For the purpose of accessibility this number is likely unique.
  // If it breaks and isn't unique it won't break the form. (poor man's guid)
  uuid = uuid();

  constructor(
    public fb: FormBuilder,
    public modals: NgbModal,
  ) {
    super(fb);
    this.form.addControl('approvedItems', this.fb.control(''));
    this.form.addControl('totalAmount', this.fb.control('', [CustomValidators.number, Validators.required, Validators.min(0)]));
  }

  ngOnInit() {
    this.handleFormChange();
    this.displayReferral(this.referral);
  }

  ngAfterViewInit() {
    // connect child form(s) to parent
    this.form.addControl('supplier', this.supplier.form);
    this.supplier.form.setParent(this.form);
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.ratesModal) { this.ratesModal.dismiss(); }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.referral) {
      // console.log('referral =', changes.referral.currentValue);
    }
    if (changes.readOnly) {
      // console.log('readOnly =', changes.readOnly.currentValue);
    }
  }

  // validate the whole form as we capture data
  handleFormChange(): void {
    this.form.valueChanges.subscribe(() => this.validate());
  }

  displayReferral(referral: IncidentalsReferral) {
    if (referral) {
      this.form.reset();
      this.form.patchValue({
        evacuees: referral.evacuees,
        approvedItems: referral.approvedItems,
        totalAmount: referral.totalAmount,
      });
    }
  }

  // TODO: if all required information is in the form we emit
  validate() {
    if (this.form.valid) {
      // ...
    }
  }

  viewRates() {
    this.ratesModal = this.modals.open(IncidentalsRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }

  // --------------------HELPERS-----------------------------------------
  numDays(validFrom: Date, validTo: Date) { return numberOfDays(validFrom, validTo); }

}
