import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import * as moment from 'moment';

import { AppState } from 'src/app/store';
import { IncidentTask } from 'src/app/core/models';
import { IncidentTaskService } from 'src/app/core/services/incident-task.service';
import { NotificationQueueService } from 'src/app/core/services/notification-queue.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { invalidField } from 'src/app/shared/utils';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';
import { ValidationHelper } from 'src/app/shared/validation/validation.helper';

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

  incidentTask: IncidentTask = {
    id: '',
    taskNumber: '',
    details: '',
    community: null,
    region: null,
    startDate: null // datetime
  };

  // fields to collect
  form: FormGroup;
  shouldValidateForm = false; // run validation *after* the user clicks the NEXT button, not before.
  errorSummary = ''; // error summary to display; i.e. 'Some required fields have not been completed.'
  validationErrors: { [key: string]: any } = {}; // holds field-level validation errors to display in the form

  // generic validation helper
  private constraints: { [key: string]: { [key: string]: string | { [key: string]: string } } };
  private validationHelper: ValidationHelper;

  constructor(
    private router: Router,
    private store: Store<AppState>,
    private incidentTaskService: IncidentTaskService,
    private notificationQueueService: NotificationQueueService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private fb: FormBuilder,
  ) {
    // Define all of the validation messages for the form.
    this.constraints = {
      taskNumber: {
        required: 'Please enter a task number',
      },
      community: {
        required: 'Please select a community/region from the dropdown list where the incident took place',
      },
      startDate: {
        required: 'Please enter a valid date and time for the incident',
        maxDate: 'Date for the incident must be today or in the past',
      },
      details: {
        required: 'Please enter incident details',
      },
    };

    // Define an instance of the validation helper for this form,
    // passing in this form's set of validation messages.
    this.validationHelper = new ValidationHelper(this.constraints);
  }

  // convenience getter for easy access to validation errors within the HTML template
  get e(): any { return this.validationErrors; }

  get pageTitle(): string { return this.editMode ? 'Edit a Task Number' : 'Add a Task Number'; }

  ngOnInit() {
    // keep the current path up to date
    this.authService.path.subscribe((path: string) => this.path = path);

    // initialize form for collection
    this.initializeForm();

    const key = this.uniqueKeyService.getKey();
    if (key) {
      // there may be a user to edit because the route has an ID for an incident task
      this.incidentTaskService.getIncidentTask(key)
        .subscribe((incidentTask: IncidentTask) => {
          // save the incident task for filling in information later.
          this.displayTaskNumber(incidentTask);
          this.incidentTask = incidentTask;
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
      startDate: [null, [Validators.required, CustomValidators.maxDate(moment())]],
      details: ['', Validators.required],
    });
  }

  invalid(field: string, parent: FormGroup = this.form): boolean {
    return invalidField(field, parent, this.shouldValidateForm);
  }

  private validateForm(): boolean {
    this.shouldValidateForm = true;
    this.validationErrors = this.validationHelper.processMessages(this.form);
    return this.form.valid;
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
    if (this.validateForm()) {
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
      // TODO: go somewhere useful for this provincial user after routing is fixed.
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
            this.notificationQueueService.addNotification('Task number updated successfully', 'success');
            // done editing the entry. Clear the reference key.
            this.uniqueKeyService.clearKey();
            // go back to the task number list page
            // TODO: preserveQueryParams is deprecated, use queryParamsHandling instead
            this.router.navigate([`/${this.path}/task-numbers`], { preserveQueryParams: true });
          });
      } else {
        // if the volunteer has no id we need to create a new one
        this.incidentTaskService.createIncidentTask(this.incidentTask)
          .subscribe((incidentTask: IncidentTask) => {
            this.submitting = false;
            // add a message to the UI
            this.notificationQueueService.addNotification('Task number added successfully', 'success');
            // NB - there is no key in this scenario
            // go back to the task number list page
            // TODO: preserveQueryParams is deprecated, use queryParamsHandling instead
            this.router.navigate([`/${this.path}/task-numbers`], { preserveQueryParams: true });
          });
      }
    }
  }

  cancel() {
    // clear the loaded record if available
    this.uniqueKeyService.clearKey();
    // navigate back home
    this.router.navigate([`/${this.path}/task-numbers`]);
  }

  onSave(): void {
    const f = this.form.value;
    this.incidentTask.taskNumber = f.taskNumber;
    this.incidentTask.community = f.community;
    this.incidentTask.details = f.details;
    this.incidentTask.startDate = (f.startDate as Date).toJSON(); // make sure JS dates are properly serialized
  }
}
