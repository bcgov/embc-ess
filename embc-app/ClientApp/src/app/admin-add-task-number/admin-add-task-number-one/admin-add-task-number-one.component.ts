import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { Router } from '@angular/router';
import { IncidentTask } from 'src/app/core/models';
import { UpdateIncidentTask } from 'src/app/store/incident-tasks/incident-tasks.actions';
import { takeWhile } from 'rxjs/operators';

@Component({
  selector: 'app-admin-add-task-number-one',
  templateUrl: './admin-add-task-number-one.component.html',
  styleUrls: ['./admin-add-task-number-one.component.scss']
})
export class AdminAddTaskNumberOneComponent implements OnInit {

  communities$ = this.store.select(s => s.lookups.communities.communities);
  // whatever is in the application state
  currentIncidentTask$ = this.store.select(i => i.incidentTasks.currentIncidentTask);
  componentActive = true;

  // three fields to collect
  taskNumber: FormControl;
  community: FormControl;
  details: FormControl;
  incidentTask: IncidentTask;

  constructor(
    private router: Router,
    private store: Store<AppState>, ) { }

  ngOnInit() {
    // initialize form for collection
    this.initForm();
  }
  initForm() {
    // get the current incident task
    this.currentIncidentTask$.pipe(takeWhile(() => this.componentActive))
      .subscribe(i => {
        this.incidentTask = i;
        // initialize form
        this.taskNumber = new FormControl(i.taskNumber || null);
        this.community = new FormControl(i.community || null);
        this.details = new FormControl(i.details || null);
      });
  }
  next(): void {
    // only go next if all fields are non null
    if (this.taskNumber.value && this.community.value && this.details.value) {
      const incidentTask: IncidentTask = {
        id: this.incidentTask.id || null, // keep the id for updates
        taskNumber: this.taskNumber.value,
        community: this.community.value,
        details: this.details.value,
      };
      // when routing to the next page we save first into the application state.
      this.onSave(incidentTask);
      // information saved in state. Navigate to confirm page
      this.router.navigate(['add-task-number/confirmation']);
    } else {
      alert("All fields are required to continue.");
    }
  }

  onSave(incidentTask: IncidentTask): void {
    // stuff the data into an incidentTask object
    this.store.dispatch(new UpdateIncidentTask({ incidentTask }));
  }
}
