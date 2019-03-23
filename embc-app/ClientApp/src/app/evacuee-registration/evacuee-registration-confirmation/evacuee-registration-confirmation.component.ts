import { Component, OnInit } from '@angular/core';
import { Evacuee, Registration } from 'src/app/core/models';
import { AppState } from 'src/app/store';
import { Store } from '@ngrx/store';
import { FormGroup } from '@angular/forms';
import { takeWhile } from 'rxjs/operators';

@Component({
  selector: 'app-evacuee-registration-confirmation',
  templateUrl: './evacuee-registration-confirmation.component.html',
  styleUrls: ['./evacuee-registration-confirmation.component.scss']
})
export class EvacueeRegistrationConfirmationComponent implements OnInit {

  // current application state
  currentRegistration$ = this.store.select(state => state.registrations.currentRegistration);
  registration: Registration | null;
  form: FormGroup;
  componentActive = true;


  constructor(
    private store: Store<AppState>, // ngrx app state
  ) { }

  ngOnInit() {
    // Update form values based on the state
    this.currentRegistration$
      .pipe(takeWhile(() => this.componentActive))
      .subscribe(registration => {
        this.registration = registration;
      });
  }

}
