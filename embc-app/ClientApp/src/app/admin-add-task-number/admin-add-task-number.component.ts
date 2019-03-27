import { Component, OnInit } from '@angular/core';
import { AppState } from '../store';
import { Store } from '@ngrx/store';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-add-task-number',
  templateUrl: './admin-add-task-number.component.html',
  styleUrls: ['./admin-add-task-number.component.scss']
})
export class AdminAddTaskNumberComponent implements OnInit {
  communities$ = this.store.select(s => s.lookups.communities.communities);

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
  }
  initForm() {
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
