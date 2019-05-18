import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormControl, FormGroup } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';

import { Evacuee, ReferralBase } from 'src/app/core/models';
import { clearFormArray, uuid } from 'src/app/shared/utils';

/**
 * This is the base class for `FoodReferralComponent`, `ClothingReferralComponent`, etc.
 *
 * It provides some of the shared behavior that all referral components have, like
 * calculating validation status and handling selection of evacuees from a list. It
 * also defines the properties that are shared between all sub-classes, like `form`
 * and `array` and `comments`.
 *
 * It shouldn't be instantiated directly.
 */
@Component({ template: '' })
export class AbstractReferralComponent<T extends ReferralBase> implements OnInit, OnDestroy {
  @Input() referral: T = null;
  @Input() showErrorsWhen = true;

  // List of all evacuees that we want to show in this component
  @Input() evacuees: Evacuee[] = [];
  @Input() readOnly = false;

  private subscription: Subscription;
  @Input() formChangeTrigger: Observable<void>;

  // @Input() emitChangesWhen = false; // formChange will NOT be fired until this flag is set to TRUE
  @Input() showErrorsWhen = false; // wait until the user click NEXT before showing any validation errors

  @Output() formReady = new EventEmitter<FormGroup>();
  @Output() formChange = new EventEmitter<T>();
  @Output() formValidationChange = new EventEmitter<boolean>();

  // The model for the form data collected
  form = this.fb.group({
    evacuees: this.fb.array([], Validators.required),
    comments: '',
  });

  // For the purpose of accessibility this number is likely unique.
  // If it breaks and isn't unique it won't break the form. (poor man's guid)
  uuid = uuid();

  // helper to format dollar amounts
  currency = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 2
  });

  constructor(public fb: FormBuilder) { }

  ngOnInit() {
    if (!this.readOnly) {
      // this allows the parent form (the list "maker") to trigger a form submission
      // we need this because we have no submit buttons on each individual referral form
      this.subscription = this.formChangeTrigger.subscribe(() => this.onSubmit());

      // inform the parent form about this sub-form validation status
      this.form.statusChanges.subscribe(status => this.onValidate(status));

      // let the parent form know we are ready
      this.formReady.emit(this.form);
    }
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  onValidate(status: any) {
    const formIsValid = status === 'VALID';
    this.formValidationChange.emit(formIsValid);
  }

  onSubmit() {
    const p = this.toModel(this.form.value);
    this.propagateChanges(p);
  }

  toModel(formValue: any): T {
    // Copy over all of the original referral properties
    // Then copy over the values from the form
    // This ensures values not on the form, such as the Id, are retained
    const p: T = { ...this.referral, ...this.form.value };
    return p;
  }

  propagateChanges(newValue: T) {
    this.formChange.emit(newValue);
  }

  /**
   * After a sub-form is initialized, we link it to our main form
   */
  formInitialized(name: string, childForm: FormGroup) {
    this.form.setControl(name, childForm);
  }

  get selected() {
    return this.form.get('evacuees') as FormArray;
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
