import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';

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
    private store: Store<AppState>, // ngrx app state
  ) { }

  ngOnInit() {
    this.initForm();
  }
  initForm() {
    // initialize form
    this.taskNumber = new FormControl(null);
    this.community = new FormControl(null);
    this.details = new FormControl(null);
  }
  next() {
    // this will eventually go to the next page instead of a simple submit
    alert("next please")
  }
}
