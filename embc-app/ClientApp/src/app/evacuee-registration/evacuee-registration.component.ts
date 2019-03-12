import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Form } from '@angular/forms';
import { Store } from '@ngrx/store';
import { AppState } from '../store';
import { Registration } from '../core/models';

@Component({
  selector: 'app-evacuee-registration',
  templateUrl: './evacuee-registration.component.html',
  styleUrls: ['./evacuee-registration.component.scss']
})
export class EvacueeRegistrationComponent implements OnInit {

  // form variables
  form: FormGroup;
  componentActive = true;

  // The model for the form data collected
  registration: Registration | null;

  constructor(
    private store: Store<AppState>, // ngrx app state
    private fb: FormBuilder, // reactive forms
  ) { }

  ngOnInit() {
  }

}
