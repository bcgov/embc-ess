import { Component, Input, Output, EventEmitter, OnInit, OnDestroy, SimpleChanges, OnChanges, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormControl, FormGroup, ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';

import { Evacuee, ReferralBase } from 'app/core/models';
import { clearFormArray, uuid } from 'app/shared/utils';

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
export class AbstractReferralComponent<T extends ReferralBase> implements OnInit, OnDestroy, OnChanges {

  @ViewChild('comments') comments: ElementRef;

  @Input() readOnly = false;
  @Input() showErrorsWhen = false; // wait until the user click NEXT before showing any validation errors

  // List of all evacuees that we want to show in this component
  @Input() evacuees: Evacuee[] = [];

  // the referral to show
  @Input() referral: T = null;

  private subscription: Subscription;
  @Input() formChangeTrigger: Observable<void>;

  @Output() formReady = new EventEmitter<FormGroup>();
  @Output() formChange = new EventEmitter<T>();
  @Output() formValidationChange = new EventEmitter<boolean>();

  // For the purpose of accessibility this number is likely unique.
  // If it breaks and isn't unique it won't break the form. (poor man's guid)
  uuid = uuid();

  // the model for the form data collected
  form = this.fb.group({
    evacuees: this.fb.array([], Validators.required),
    comments: this.fb.control(null, this.commentsValidator()),
  });

  // to run validation after user clicks the Submit button
  shouldValidateForm = false;

  // helper to format dollar amounts
  currency = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 2
  });

  // convenience getter for easy access to form fields
  get f(): any { return this.form.controls; }

  constructor(public fb: FormBuilder) { }

  ngOnInit() {
    if (!this.readOnly) {
      // this allows the parent form (the list "maker") to trigger a form submission
      // we need this because we don't have submit buttons on individual referral forms
      if (this.formChangeTrigger) {
        this.subscription = this.formChangeTrigger.subscribe(() => this.onSubmit());
      }

      // inform the parent form about this sub-form validation status
      this.form.statusChanges.subscribe(status => {
        const formIsValid = (status === 'VALID');
        this.formValidationChange.emit(formIsValid);
      });

      // let the parent form know we are ready
      this.formReady.emit(this.form);
    }
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.referral && this.referral && !this.readOnly) {
      // display referral
      this.form.reset();
      clearFormArray(this.selected);
      this.fromModel(this.referral);
    }
  }

  fromModel(model: T): void {
    this.form.patchValue({
      comments: model.comments,
    });

    // populate the evacuee list with existing selection
    (model.evacuees || []).forEach(x => this.selectEvacuee(x));
  }

  toModel(formValue: any): T {
    // Copy over all of the original referral properties.
    // Then copy over the values from the form.
    // This ensures values not on the form, such as the Id, are retained.
    const p: T = { ...this.referral, ...formValue };
    return p;
  }

  private commentsValidator(): ValidatorFn {
    return (c: AbstractControl): ValidationErrors | null => {
      const ne = this.comments && this.comments.nativeElement;

      // is text length OK?
      if (ne && ne.textLength > 250) { return { tooLong: true }; }

      // is text height OK?
      if (ne && ne.scrollHeight > ne.offsetHeight) { return { tooTall: true }; }

      return null;
    };
  }

  onSubmit() {
    this.shouldValidateForm = true;
    if (!this.form.valid) {
      // console.log('REFERRAL-FORM: form is invalid');
      return;
    }
    const p = this.toModel(this.form.value);
    // propagate changes
    this.formChange.emit(p);
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

  private addEvacuee(evacuee: Evacuee) {
    this.selected.push(new FormControl(evacuee));
  }

  removeEvacuee(index: number) {
    this.selected.removeAt(index);
  }

  private selectEvacuee(evacuee: Evacuee) {
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

  // returns null if there is no value, otherwise returns number
  // this passes through '0'
  protected numberOrNull(value: number): number | null {
    return (value !== undefined ? value : null);
  }

}
