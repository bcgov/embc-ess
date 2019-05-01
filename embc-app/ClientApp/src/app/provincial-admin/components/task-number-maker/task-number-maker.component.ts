import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { IncidentTask } from '../../../core/models';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AppState } from '../../../store';
import { IncidentTaskService } from '../../../core/services/incident-task.service';
import { NotificationQueueService } from '../../../core/services/notification-queue.service';
import { UniqueKeyService } from '../../../core/services/unique-key.service';
import { AuthService } from '../../../core/services/auth.service';
// import { UpdateIncidentTask } from '../store/incident-tasks/incident-tasks.actions';

@Component({
  selector: 'app-task-number-maker',
  templateUrl: './task-number-maker.component.html',
  styleUrls: ['./task-number-maker.component.scss']
})
export class TaskNumberMakerComponent implements OnInit {

  maker = true; // determines if the widget is in edit or confirmation mode
  editMode = false;
  submitting = false;
  //path is for routing based on the user's role
  path: string;

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
    region: null,
    startDate: null // datetime
  };

  constructor(
    private router: Router,
    private store: Store<AppState>,
    private incidentTaskService: IncidentTaskService,
    private notificationQueueService: NotificationQueueService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // keep the current path up to date
    this.authService.path.subscribe(p => this.path = p);

    // initialize form for collection
    this.taskNumber = new FormControl('');
    this.details = new FormControl('');
    this.community = new FormControl('');

    const key = this.uniqueKeyService.getKey();
    if (key) {
      // there may be a user to edit because the route has an ID for an incident task
      this.incidentTaskService.getIncidentTask(key)
        .subscribe((i: IncidentTask) => {
          // save the incident task for filling in information later.
          this.taskNumber.setValue(i.taskNumber);
          // alert(JSON.stringify(i.community));
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

  next(): void {
    // only go next if all fields are non null
    if (this.taskNumber.value && this.community.value && this.details.value) {
      this.maker = false;
      this.onSave();
    } else {
      alert('All fields are required to continue.');
    }
  }

  back() {
    // show the editing parts of the form.
    this.maker = true;
  }

  submit() {
    this.submitting = true;
    if (!(this.incidentTask.community && this.incidentTask.details && this.incidentTask.taskNumber)) {
      // todo go somewhere useful for this provincial user after routing is fixed.
      this.submitting = false;
      this.maker = true; // switch back into maker mode because information is somehow missed.
    } else {
      // check if this is an update
      if (this.incidentTask.id) {
        // if the volunteer has an ID we need to update
        this.incidentTaskService.updateIncidentTask(this.incidentTask)
          .subscribe(() => {
            this.submitting = false;
            // add a message to the UI
            this.notificationQueueService.addNotification('Task number updated successfully');
            // done editing the entry. Clear the reference key.
            this.uniqueKeyService.clearKey();
            // go back to the task number list page
            this.router.navigate([`/${this.path}/task-numbers`], { preserveQueryParams: true });
          });
      } else {
        // if the volunteer has no id we need to create a new one
        this.incidentTaskService.createIncidentTask(this.incidentTask)
          .subscribe(i => {
            this.submitting = false;
            // add a message to the UI
            this.notificationQueueService.addNotification('Task number added successfully');
            // NB - there is no key in this scenario
            // go back to the task number list page
            this.router.navigate([`/${this.path}/task-numbers`], { preserveQueryParams: true });
          });
      }
    }
  }

  onSave(): void {
    const incidentTask: IncidentTask = this.incidentTask;
    incidentTask.id = this.incidentTask.id || null; // keep the id for updates
    incidentTask.taskNumber = this.taskNumber.value;
    incidentTask.community = this.community.value;
    incidentTask.details = this.details.value;
    this.incidentTask = incidentTask;
    // stuff the data into an incidentTask object
    // this.store.dispatch(new UpdateIncidentTask({ incidentTask }));
  }
}
