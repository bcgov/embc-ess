import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { Router, ActivatedRoute } from '@angular/router';
import { IncidentTask } from 'src/app/core/models';
import { UpdateIncidentTask } from 'src/app/store/incident-tasks/incident-tasks.actions';
import { takeWhile } from 'rxjs/operators';
import { IncidentTaskService } from 'src/app/core/services/incident-task.service';

@Component({
  selector: 'app-admin-add-task-number-one',
  templateUrl: './admin-add-task-number-one.component.html',
  styleUrls: ['./admin-add-task-number-one.component.scss']
})
export class AdminAddTaskNumberOneComponent implements OnInit {
  editMode = false;

  communities$ = this.store.select(s => s.lookups.communities.communities);
  // whatever is in the application state
  currentIncidentTask$ = this.store.select(i => i.incidentTasks.currentIncidentTask);
  componentActive = true;

  // three fields to collect
  taskNumber: FormControl;
  community: FormControl;
  details: FormControl;
  incidentTask: IncidentTask = {
    id: '',
    taskNumber: '',
    details: '',
    community: null,
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private store: Store<AppState>,
    private incidentTaskService: IncidentTaskService,
  ) { }

  ngOnInit() {
    // initialize form for collection
    this.initForm();
    if (this.route.snapshot.params.id) {
      // there may be a user to edit because the route looks right
      this.incidentTaskService.getIncidentTask(this.route.snapshot.params.id).subscribe((i: IncidentTask) => {
        // save the volunteer for filling in information later.
        this.taskNumber.setValue(i.taskNumber);
        this.community.setValue(i.community);
        this.details.setValue(i.details);

        this.incidentTask = i;
        this.editMode = true;
      });
    } else {
      // this is a fresh form and will be a simple add user
      this.editMode = false;
    }
  }
  initForm() {
    // get the current incident task
    this.taskNumber = new FormControl('');
    this.details = new FormControl('');
    this.community = new FormControl('');
    // this.currentIncidentTask$.pipe(takeWhile(() => this.componentActive))
    //   .subscribe(i => {
    //     this.incidentTask = i;
    //     // initialize form
    //     this.taskNumber = new FormControl(i.taskNumber || null);
    //     this.community = new FormControl(i.community || null);
    //     this.details = new FormControl(i.details || null);
    //   });
  }
  next(): void {
    this.onSave();
    // only go next if all fields are non null
    if (this.taskNumber.value && this.community.value && this.details.value) {
      // when routing to the next page we save first into the application state.
      this.onSave();
      // information saved in state. Navigate to confirm page
      this.router.navigate(['task-number-edit/confirmation']);
    } else {
      alert("All fields are required to continue.");
    }
  }

  onSave(): void {
    const incidentTask: IncidentTask = this.incidentTask;
    incidentTask.id = this.incidentTask.id || null; // keep the id for updates
    incidentTask.taskNumber = this.taskNumber.value;
    incidentTask.community = this.community.value;
    incidentTask.details = this.details.value;
    // stuff the data into an incidentTask object
    this.store.dispatch(new UpdateIncidentTask({ incidentTask }));
  }
}
