import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Registration } from 'src/app/core/models';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { Router, ActivatedRoute } from '@angular/router';
import { LoadAllRegistrations } from 'src/app/store/registration/registration.actions';

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
    return this.store.select(state => state.registrations.currentRegistration);
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

    // this.store.dispatch(new LoadAllRegistrations(newState));
  }

  next() {
    this.onSave();
    this.router.navigate(['../step-4'], { relativeTo: this.route });
  }

  back() {
    this.router.navigate(['../step-2'], { relativeTo: this.route });
  }
}
