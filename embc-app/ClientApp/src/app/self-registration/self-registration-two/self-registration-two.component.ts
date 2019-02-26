import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { first } from "rxjs/operators";

import { AppState } from 'src/app/store/app-state';
import { Registration } from 'src/app/core/models';
import { UpdateRegistration } from 'src/app/store/actions/registration.action';

@Component({
  selector: 'app-self-registration-two',
  templateUrl: './self-registration-two.component.html',
  styleUrls: ['./self-registration-two.component.scss']
})
export class SelfRegistrationTwoComponent implements OnInit {
  form: FormGroup;
  registration: Registration;

  // TODO: Fetch this from backend API (when available). This is a controlled list (lookup table)
  servicesLookup = [
    { id: 1, name: 'Food' },
    { id: 2, name: 'Clothing' },
    { id: 3, name: 'Accommodation' },
    { id: 4, name: 'Incidentals' },
    { id: 5, name: 'Transportation' },
  ];

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  // Shortcuts for this.form.get(...)
  get isSupportRequired() { return this.form.get('isSupportRequired') }
  get requestedSupportServices() { return this.form.get('requestedSupportServices') as FormArray; }

  // TODO: Form UI logic; i.e. show additional form fields when a checkbox is checked
  get ui() {
    return {
      showAvailableServices: () => { return this.form.get('isSupportRequired').value === true; },
    };
  }

  ngOnInit() {
    this.getInitialState()
      .pipe(first())
      .subscribe(registration => {
        this.initForm(registration);
        this.handleFormChanges();
      });
  }

  getInitialState() {
    return this.store.select(state => state.registration);
  }

  initForm(state: Registration) {
    this.registration = state;

    this.form = this.fb.group({
      hasDietaryRequirements: [],
      isTakingMedication: [],
      hasPets: [],
      hasInsurance: [],
      isSupportRequired: [],
      requestedSupportServices: this.buildSupportServices(),
    });
  }

  handleFormChanges() {
    // TODO: Register any value change listeners here...
    this.isSupportRequired.valueChanges.subscribe((value: boolean) => {
      if (!value) {
        this.resetSupportServices();
      }
    });
  }

  // TODO: refactor form-array into sub-component <support-services [parent]="form" .../>
  buildSupportServices(): FormArray {
    // all checkboxes are unchecked by default...
    const arr = this.servicesLookup.map(x => this.fb.control(false));
    return this.fb.array(arr);
  }

  resetSupportServices(): void {
    const checkboxes = this.requestedSupportServices.controls;
    checkboxes.forEach(cb => cb.setValue(false));
  }

  onSave() {
    const form = this.form.value;
    const state = this.registration;

    const newState: Registration = {
      ...state,
      ...{}
    };

    this.store.dispatch(new UpdateRegistration(newState));
  }

  next() {
    this.onSave();
    this.router.navigate(['../step-3'], { relativeTo: this.route });
  }

  back() {
    this.router.navigate(['../step-1'], { relativeTo: this.route });
  }
}
