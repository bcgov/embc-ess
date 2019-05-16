import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormControl, FormGroup } from '@angular/forms';

import { Evacuee } from 'src/app/core/models';
import { clearFormArray } from 'src/app/shared/utils';

/**
 * This is the base class for `FoodReferralComponent`, `ClothingReferralComponent`, etc.
 *
 * It provides some of the shared behavior that all referral components have, like
 * calculating validation status and handling selection of evacuees from a list. It
 * also defines the properties that are shared between all sub-classes, like `form`
 * and `array`.
 *
 * It shouldn't be instantiated directly.
 */
@Component({ template: '' })
export class AbstractReferralComponent {
  // List of all evacuees that we want to show in this component
  @Input() evacuees: Evacuee[] = [];
  @Input() readOnly = false;
  @Output() validationStatusChange = new EventEmitter<boolean>();

  // The model for the form data collected
  form: FormGroup;

  constructor(public fb: FormBuilder) {
    this.form = this.fb.group({
      selectedEvacuees: this.fb.array([], Validators.required),
    });
    this.form.statusChanges.subscribe(status => this.validationStatusChange.emit(status === 'VALID'));
  }

  get selected() {
    return this.form.get('selectedEvacuees') as FormArray;
  }

  addEvacuee(evacuee: Evacuee) {
    this.selected.push(new FormControl(evacuee));
  }

  removeEvacuee(index: number) {
    this.selected.removeAt(index);
  }

  selectEvacuee(evacuee: Evacuee) {
    const index = this.indexOfEvacuee(this.selected.value, evacuee);
    if (index >= 0) {
      this.removeEvacuee(index);
    } else {
      this.addEvacuee(evacuee);
    }
  }

  selectAllEvacuees() {
    clearFormArray(this.selected);
    (this.evacuees || []).forEach(x => this.addEvacuee(x));
  }

  // search an Evacuee by ID
  protected indexOfEvacuee(arr: Array<Evacuee>, value: Evacuee): number {
    return (arr || []).map(o => o.id).indexOf(value.id);
  }
}
