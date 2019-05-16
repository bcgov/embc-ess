import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormControl, FormGroup } from '@angular/forms';

import { Evacuee } from 'src/app/core/models';
import { SupplierComponent } from '../supplier/supplier.component';
import { clearFormArray } from 'src/app/shared/utils';

@Component({ template: '' })
export class AbstractReferralComponent implements OnInit {
  // List of all evacuees that we want to show in this component
  @Input() evacuees: Evacuee[] = [];
  @Input() readOnly = false;
  @Output() validityChange = new EventEmitter<boolean>();

  // The model for the form data collected
  form: FormGroup;

  constructor(public fb: FormBuilder) {
    this.form = this.fb.group({
      selectedEvacuees: this.fb.array([], Validators.required),
    });
    this.form.statusChanges.subscribe(status => this.validityChange.emit(status === 'VALID'));
  }

  get array() {
    return this.form.get('selectedEvacuees') as FormArray;
  }

  ngOnInit() {
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

  // search an Evacuee by ID
  protected indexOfEvacuee(arr: Array<Evacuee>, value: Evacuee): number {
    return (arr || []).map(o => o.id).indexOf(value.id);
  }
}
