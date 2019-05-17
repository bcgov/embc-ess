import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges, ViewChild, AfterViewInit } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { IncidentalsReferral, Supplier } from 'src/app/core/models';
import { IncidentalsRatesComponent } from 'src/app/shared/modals/incidentals-rates/incidentals-rates.component';
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
  @Output() referralChange = new EventEmitter<IncidentalsReferral>();

  // TODO: replace this with formReady event on supplier form
  @ViewChild(SupplierComponent) supplierRef: SupplierComponent;

  private ratesModal: NgbModalRef = null;

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
    this.form.valueChanges.subscribe(() => this.saveChanges());
  }

  displayReferral(referral: IncidentalsReferral) {
    if (referral) {
      this.form.reset();
      this.form.patchValue({
        evacuees: referral.evacuees,
        approvedItems: referral.approvedItems,
        totalAmount: referral.totalAmount,
        comments: referral.comments,
      });

      // populate the evacuee list with existing selection
      (referral.evacuees || []).forEach(x => this.selectEvacuee(x));

      // TODO: Dates FROM and TO
    }
  }

  // if all required information is in the form we emit
  saveChanges() {
    if (!this.form.valid) {
      return;
    }
    // Copy over all of the original referral properties
    // Then copy over the values from the form
    // This ensures values not on the form, such as the Id, are retained
    const p = { ...this.referral, ...this.form.value };
    this.referralChange.emit(p);
  }

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
