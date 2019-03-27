import { Component, OnInit } from '@angular/core';
import { AppState } from '../store';
import { Store } from '@ngrx/store';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { takeWhile } from 'rxjs/operators';
import { IncidentTask } from '../core/models';

@Component({
  selector: 'app-admin-add-task-number',
  templateUrl: './admin-add-task-number.component.html',
  styleUrls: ['./admin-add-task-number.component.scss']
})
export class AdminAddTaskNumberComponent implements OnInit {
  // current community list in the application state
  communities$ = this.store.select(s => s.lookups.communities.communities);
  // whatever is in the application state
  currentIncidentTask$ = this.store.select(i => i.incidentTasks.currentIncidentTask);

  // Keep the component alive
  componentActive: boolean = true;

  // three fields to collect
  taskNumber: FormControl;
  community: FormControl;
  details: FormControl;

  constructor(
    private store: Store<AppState>, // ngrx app state
    private router: Router,
  ) { }

  ngOnInit() {
    this.initForm();
    this.currentIncidentTask$
      .pipe(takeWhile(() => this.componentActive))
      .subscribe(i => {
        alert(JSON.stringify(i));
        // this.initForm(i)
      });
    // Update form values based on the state
    // this.currentRegistration$
    //   .pipe(takeWhile(() => this.componentActive))
    //   .subscribe(value => this.displayRegistration(value));
  }
  initForm(incidentTask?: IncidentTask) {
    // initialize form
    this.taskNumber = new FormControl(null);
    this.community = new FormControl(null);
    this.details = new FormControl(null);
  }
  next() {
    // this will eventually go to the next page instead of a simple submit
    this.submit();
  }
  submit() {
    alert(this.taskNumber.value);
  }

}
