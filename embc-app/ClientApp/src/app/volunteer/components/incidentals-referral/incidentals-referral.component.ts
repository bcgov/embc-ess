import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges, ViewChild, AfterViewInit } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { IncidentalsReferral, Evacuee } from 'src/app/core/models';
import { IncidentalsRatesComponent } from 'src/app/shared/modals/incidentals-rates/incidentals-rates.component';
import { numberOfDays, uuid, clearFormArray } from 'src/app/shared/utils';
import { SupplierComponent } from '../supplier/supplier.component';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';

@Component({
  selector: 'app-incidentals-referral',
  templateUrl: './incidentals-referral.component.html',
  styleUrls: ['./incidentals-referral.component.scss']
})
export class IncidentalsReferralComponent implements OnInit, AfterViewInit, OnDestroy, OnChanges {
  // List of all evacuees that we want to show in this component
  @Input() evacuees: Evacuee[];
  @Input() referral: IncidentalsReferral = null;
  @Input() readOnly = false;

  @Output() validityChange = new EventEmitter<boolean>();

  @ViewChild(SupplierComponent) supplier: SupplierComponent;

  private ratesModal: NgbModalRef = null;

  // For the purpose of accessibility this number is likely unique.
  // If it breaks and isn't unique it won't break the form. (poor man's guid)
  uuid = uuid();

  // The model for the form data collected
  form = this.fb.group({
    selectedEvacuees: this.fb.array([], Validators.required),
    approvedItems: '',
    totalAmount: ['', [CustomValidators.number, Validators.required, Validators.min(0)]],
  });

  constructor(
    private modals: NgbModal,
    private fb: FormBuilder,
  ) { }

  get array() {
    return this.form.get('selectedEvacuees') as FormArray;
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
    this.form.statusChanges.subscribe(status => this.validityChange.emit(status === 'VALID'));
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

  addEvacuee(evacuee: Evacuee) {
    this.array.push(new FormControl(evacuee));
  }

  removeEvacuee(index: number) {
    this.array.removeAt(index);
  }

  selectEvacuee(evacuee: Evacuee) {
    const index = this.indexOfEvacuee(this.array.value, evacuee);
    if (index >= 0) {
      this.removeEvacuee(index);
    } else {
      this.addEvacuee(evacuee);
    }
  }

  selectAllEvacuees() {
    clearFormArray(this.array);
    (this.evacuees || []).forEach(x => this.addEvacuee(x));
  }

  // --------------------HELPERS-----------------------------------------
  numDays(validFrom: Date, validTo: Date) { return numberOfDays(validFrom, validTo); }

  // search an Evacuee by ID
  indexOfEvacuee(arr: Array<Evacuee>, value: Evacuee): number {
    return (arr || []).map(o => o.id).indexOf(value.id);
  }
}
