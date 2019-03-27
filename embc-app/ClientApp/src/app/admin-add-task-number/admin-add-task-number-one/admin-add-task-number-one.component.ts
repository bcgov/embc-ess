import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-admin-add-task-number-one',
  templateUrl: './admin-add-task-number-one.component.html',
  styleUrls: ['./admin-add-task-number-one.component.scss']
})
export class AdminAddTaskNumberOneComponent implements OnInit {

  communities$ = this.store.select(s => s.lookups.communities.communities);

  // three fields to collect
  taskNumber: FormControl;
  community: FormControl;
  details: FormControl;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private store: Store<AppState>, ) { }

  ngOnInit() {
    this.initForm();
  }
  initForm() {
    // initialize form
    this.taskNumber = new FormControl(null);
    this.community = new FormControl(null);
    this.details = new FormControl(null);
  }
  next(): void {
    // when routing to the next page we save first into the application state.
    this.onSave();
    // if (this.volunteer.lastName && this.volunteer.firstName && this.volunteer.bceidAccountNumber && this.volunteer.canAccessRestrictedFiles != null) {
    //   // simple check to be sure that all fields are included.
    //   this.router.navigate(['volunteer-edit/confirmation']);
    // } else {
    //   alert("All fields are required.");
    // }
  }

  onSave(): void {
    // stuff the data back into the volunteer object
    //   const volunteer: Volunteer = this.volunteer;
    //   volunteer.lastName = this.lastName.value;
    //   volunteer.firstName = this.firstName.value;
    //   volunteer.bceidAccountNumber = this.bceid.value;
    //   volunteer.canAccessRestrictedFiles = this.restrictedAccess.value;
    //   this.store.dispatch(new UpdateVolunteer({ volunteer }))
  }
}
