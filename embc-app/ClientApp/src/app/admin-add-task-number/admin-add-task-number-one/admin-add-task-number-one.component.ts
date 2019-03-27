import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { ActivatedRoute, Router } from '@angular/router';
import { IncidentTask } from 'src/app/core/models';
import { UpdateIncidentTask } from 'src/app/store/incident-tasks/incident-tasks.actions';

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
  incidentTask: IncidentTask;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private store: Store<AppState>, ) { }

  ngOnInit() {
    // initialize form for collection
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
    // information saved in state. Navigate to confirm page
    this.router.navigate(['add-task-number/confirmation']);
  }

  onSave(): void {
    // stuff the data into an incidentTask object
    const incidentTask: IncidentTask = {
      id: null,
      taskNumber: this.taskNumber.value,
      community: this.community.value,
      details: this.details.value,
    };
    this.incidentTask = incidentTask;
    this.store.dispatch(new UpdateIncidentTask({ incidentTask }));
  }
}
