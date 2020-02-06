import { Component, OnInit, OnDestroy, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { IncidentalsReferral } from 'src/app/core/models';
import { IncidentalsRatesComponent } from 'src/app/shared/modals/incidentals-rates/incidentals-rates.component';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';
import { ReferralDate } from 'src/app/core/models/referral-date';
import { AbstractReferralComponent } from '../abstract-referral/abstract-referral.component';

const MAXIMUM_PER_PERSON = 50.00;

@Component({
  selector: 'app-incidentals-referral',
  templateUrl: './incidentals-referral.component.html',
  styleUrls: ['./incidentals-referral.component.scss']
})
export class IncidentalsReferralComponent extends AbstractReferralComponent<IncidentalsReferral> implements OnInit, OnChanges, OnDestroy {

  @ViewChild('approvedItems') approvedItems: ElementRef;

  private ratesModal: NgbModalRef = null;

  constructor(
    public fb: FormBuilder,
    public modals: NgbModal,
  ) {
    // call base form to setup shared form fields; e.g. evacuee list, comments
    super(fb);

    // add more fields that are specific to this form
    this.form.setControl('approvedItems', this.fb.control(null, [this.approvedItemsValidator()]));
    this.form.setControl('totalAmount', this.fb.control(null, [CustomValidators.number, Validators.required, Validators.min(0)]));
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.showErrorsWhen) {
      // add 'required' validator only after user clicks NEXT
      this.f.approvedItems.setValidators([Validators.required, this.approvedItemsValidator()]);
      this.f.approvedItems.updateValueAndValidity({ emitEvent: false });
    }

    super.ngOnChanges(changes);
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

  private approvedItemsValidator(): ValidatorFn {
    return (c: AbstractControl): ValidationErrors | null => {
      const ne = this.approvedItems && this.approvedItems.nativeElement;

      // is text length OK?
      if (ne && ne.textLength > 250) { return { tooLong: true }; }

      // is text height OK?
      if (ne && ne.scrollHeight > ne.offsetHeight) { return { tooTall: true }; }

      return null;
    };
  }

  fromModel(referral: IncidentalsReferral) {
    // populate fields shared by all forms; e.g. selected evacuees, comments, supplier info...
    super.fromModel(referral);
    this.form.patchValue({
      approvedItems: referral.approvedItems,
      totalAmount: referral.totalAmount,
    });
  }

  // NB: this is called when date component is initialized and whenever its data changes
  updateReferralDate(rd: ReferralDate) {
    this.referral.validDates = rd;
  }

  maximumAmount(x: FormGroup | IncidentalsReferral): number {
    if (x) {
      if (x instanceof FormGroup) {
        // get data from form
        const n = x.value.evacuees.length;
        return (n * MAXIMUM_PER_PERSON);
      } else {
        // get data from referral
        const n = x.evacuees.length;
        return (n * MAXIMUM_PER_PERSON);
      }
    }
    return 0;
  }

  viewRates() {
    this.ratesModal = this.modals.open(IncidentalsRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }

}
