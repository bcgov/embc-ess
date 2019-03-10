import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { takeWhile } from 'rxjs/operators';

import { Registration } from 'src/app/core/models';
import { AppState } from 'src/app/store';
import { UpdateRegistration } from 'src/app/store/registration/registration.actions';


@Component({
  selector: 'app-self-registration-three',
  templateUrl: './self-registration-three.component.html',
  styleUrls: ['./self-registration-three.component.scss']
})
export class SelfRegistrationThreeComponent implements OnInit, OnDestroy {

  // state needed by this FORM
  currentRegistration$ = this.store.select(state => state.registrations.currentRegistration);

  form: FormGroup;
  componentActive = true;

  registration: Registration | null;

  constructor(
    private store: Store<AppState>,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    // Create form controls
    this.initForm();
    this.onFormChanges();

    // Update form values based on the state
    this.currentRegistration$
      .pipe(takeWhile(() => this.componentActive))
      .subscribe(value => this.displayRegistration(value));
  }

  ngOnDestroy(): void {
    this.componentActive = false;
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.form.controls;
  }

  // Define the form group
  initForm() {
    this.form = this.fb.group({
      declarationAndConsent: [null, Validators.requiredTrue],
    });
  }

  onFormChanges() {
  }

  displayRegistration(registration: Registration | null): void {
    // Set the local registration property
    this.registration = registration;

    if (this.registration && this.form) {
      // Reset the form back to pristine
      this.form.reset();

      // Update the data on the form
      this.form.patchValue({
        declarationAndConsent: this.registration.declarationAndConsent,
      });
    }
  }

  next() {
    this.onSave();
    this.router.navigate(['../step-4'], { relativeTo: this.route });
  }

  back() {
    // clear the consent checkbox if we go back to edit the information provided so far
    this.reset();
    this.onSave();
    this.router.navigate(['../step-2'], { relativeTo: this.route });
  }

  reset() {
    this.f.declarationAndConsent.reset();
  }

  onSave() {
    const registration: Registration = {
      ...this.registration,
      ...this.form.value
    };
    this.store.dispatch(new UpdateRegistration({ registration }));
  }
}
