import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Registration } from 'src/app/core/models';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/app-state';
import { Router, ActivatedRoute } from '@angular/router';
import { UpdateRegistration } from 'src/app/store/actions/registration.action';

@Component({
  selector: 'app-self-registration-three',
  templateUrl: './self-registration-three.component.html',
  styleUrls: ['./self-registration-three.component.scss']
})
export class SelfRegistrationThreeComponent implements OnInit {
  form: FormGroup;
  registration: Registration;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  // Shortcuts for this.form.get(...)
  // get mailingAddress() { return this.form.get('mailingAddress') as FormGroup; }

  // TODO: Form UI logic; i.e. show additional form fields when a checkbox is checked
  get ui() {
    return {};
  }

  ngOnInit() {
    this.getInitialState()
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

    });
  }

  handleFormChanges() {
    // TODO: Register any value change listeners here...
    // this.form.get('someField').valueChanges.subscribe(...)
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
    this.router.navigate(['../step-4'], { relativeTo: this.route });
  }

  back() {
    this.router.navigate(['../step-2'], { relativeTo: this.route });
  }
}
