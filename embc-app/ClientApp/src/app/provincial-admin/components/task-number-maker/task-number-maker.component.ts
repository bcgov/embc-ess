import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IncidentTask } from '../../../core/models';
import { Router } from '@angular/router';
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
  // path is for routing based on the user's role
  path: string;

  // whatever is in the application state
  currentIncidentTask$ = this.store.select(i => i.incidentTasks.currentIncidentTask);
  componentActive = true;

  // fields to collect
  form: FormGroup;

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
    private fb: FormBuilder,
  ) { }

  get pageTitle(): string {
    return this.editMode ? 'Edit a Task Number' : 'Add a Task Number';
  }

  ngOnInit() {
    // keep the current path up to date
    this.authService.path.subscribe(p => this.path = p);

    // initialize form for collection
    this.initializeForm();

    const key = this.uniqueKeyService.getKey();
    if (key) {
      // there may be a user to edit because the route has an ID for an incident task
      this.incidentTaskService.getIncidentTask(key)
        .subscribe((i: IncidentTask) => {
          // save the incident task for filling in information later.
          this.displayTaskNumber(i);
          this.incidentTask = i;
          this.editMode = true;
        });
    } else {
      // this is a fresh form and will be a simple add user
      this.editMode = false;
    }
  }

  initializeForm(): void {
    this.form = this.fb.group({
      taskNumber: ['', Validators.required],
      community: ['', Validators.required],
      startDateTime: [new Date(), Validators.required],
      // startTime: [{ hour: 0, minute: 0 }, Validators.required],
      details: ['', Validators.required],
    });
  }

  validateForm(): void {
    // TODO: Implement validation
    // this.validationErrors = this.validationHelper.processMessages(this.form);
  }

  displayTaskNumber(task: IncidentTask): void {
    // Reset the form back to pristine
    this.form.reset();

    // flow data back into the form
    this.form.patchValue({
      taskNumber: task.taskNumber,
      community: task.community,
      details: task.details,
      // TODO: split into date and time components...
      // startDate: ,
      // startTime: ,
    });

    // TODO: split JS Date object into date and time components
    // this.startDateField.setValue(task.startDate);
  }

  next(): void {
    // only go next if all fields are non null
    this.validateForm();

    if (this.form.valid) {
      // navigate to the next page. AKA show the summary part of the form.
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

  cancel() {
    // navigate back home
    this.router.navigate([`/${this.path}/task-numbers`]);
  }

  onSave(): void {
    this.incidentTask.taskNumber = this.form.value.taskNumber;
    this.incidentTask.community = this.form.value.community;
    this.incidentTask.details = this.form.value.details;
    // TODO: date and time
  }
}
