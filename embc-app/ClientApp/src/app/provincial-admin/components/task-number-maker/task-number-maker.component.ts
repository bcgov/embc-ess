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
import { invalidField } from 'src/app/shared/utils';

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
  shouldValidateForm = false; // run validation *after* the user clicks the NEXT button, not before.
  errorSummary = ''; // error summary to display; i.e. 'Some required fields have not been completed.'

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
      startDate: [new Date(), Validators.required],
      details: ['', Validators.required],
    });
  }

  invalid(field: string, parent: FormGroup = this.form): boolean {
    return invalidField(field, parent, this.shouldValidateForm);
  }

  validateForm(): void {
    this.shouldValidateForm = true;
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
      startDate: new Date(task.startDate),
    });
  }

  next(): void {
    // only go next if all fields are non null
    this.validateForm();

    if (this.form.valid) {
      // navigate to the next page. AKA show the summary part of the form.
      this.maker = false;
      this.errorSummary = null;
      this.onSave();
    } else {
      this.errorSummary = 'Some required fields have not been completed.';
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
    const f = this.form.value;
    this.incidentTask.taskNumber = f.taskNumber;
    this.incidentTask.community = f.community;
    this.incidentTask.details = f.details;
    this.incidentTask.startDate = f.startDate;
  }
}
