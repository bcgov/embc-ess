import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
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
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-task-number-maker',
  templateUrl: './task-number-maker.component.html',
  styleUrls: ['./task-number-maker.component.scss']
})
export class TaskNumberMakerComponent implements OnInit, AfterViewInit, OnDestroy {

  maker = true; // determines if the widget is in edit or confirmation mode
  editMode = false;
  submitting = false;
  path: string = null; // the base path for routing
  endDateOverride: boolean = false; 
  overrideDate: Date = null;
  // Subscription for the taskNumberStartDate control's value change event
  // Whenever the start date's value is changed we need to update the validator
  // on end date (end date can never be before the start date).
  private startDateSub: Subscription;

  // whatever is in the application state
  currentIncidentTask$ = this.store.select(i => i.incidentTasks.currentIncidentTask);
  componentActive = true;

  incidentTask: IncidentTask = {
    id: '',
    taskNumber: '',
    details: '',
    community: null,
    region: null,
    startDate: null, // datetime
    taskNumberStartDate: null,
    taskNumberEndDate: null
  };

  // fields to collect
  form: FormGroup = null;
  showErrorsWhen = false; // run validation *after* the user clicks the NEXT button, not before.
  overrideForm: FormGroup = null;

  constructor(
    private router: Router,
    private store: Store<AppState>,
    private incidentTaskService: IncidentTaskService,
    private notificationQueueService: NotificationQueueService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private fb: FormBuilder,
  ) { }

  // convenience getter for easy access to form fields
  get f(): any { return this.form.controls; }

  get pageTitle(): string { return this.editMode ? 'Edit a Task Number' : 'Add a Task Number'; }

  ngOnInit() {
    // keep the current path up to date
    this.authService.path.subscribe((path: string) => this.path = path);

    // initialize form for collection
    this.initializeForm();

    const taskId = this.uniqueKeyService.getKey();
    if (taskId) {
      // there may be a task to edit because the route has an ID for an incident task
      this.incidentTaskService.getIncidentTask(taskId)
        .subscribe((incidentTask: IncidentTask) => {
          // save the incident task for filling in information later.
          //this.displayTaskNumber(incidentTask);
          this.incidentTask = incidentTask;
          this.initFormFromIncidentTask();
          this.editMode = true;
        }, err => {
          this.notificationQueueService.addNotification('Failed to load task', 'danger');
          console.log('error getting task =', err);

          // go back to list of tasks
          this.router.navigate([`/${this.path}/task-numbers`]);
        });
    } else {
      // this is a fresh form and will be a simple add user
      this.editMode = false;
    }
  }

  ngAfterViewInit() {
    // focus the first input
    const elements = document.getElementsByClassName('form-control') as HTMLCollectionOf<HTMLElement>;
    if (elements.length > 0) {
      elements[0].focus();
    } else {
      // wait for elements to display and try again
      setTimeout(() => this.ngAfterViewInit(), 100);
    }
  }

  ngOnDestroy() {
    this.startDateSub.unsubscribe();
  }

  initializeForm() {
    this.form = this.fb.group({
      taskNumber         : ['', Validators.required],
      community          : [null, Validators.required],
      taskNumberStartDate: [moment(), [Validators.required, CustomValidators.maxDate(moment())]],
      taskNumberEndDate  : [moment().add(80, 'h'), [Validators.required, CustomValidators.minDate(moment())]],
      details            : ['', Validators.required],
      overrideDate       : [moment().add(80, 'h')]
    });
    this.setEndDateValidatorSubscription();
  }

  initFormFromIncidentTask() {
    let startDate: Date = new Date(this.incidentTask.taskNumberStartDate);
    let endDate: Date   = new Date(this.incidentTask.taskNumberEndDate);
    this.form = this.fb.group({
      taskNumber         : [this.incidentTask.taskNumber, Validators.required],
      community          : [this.incidentTask.community, Validators.required],
      taskNumberStartDate: [startDate, [Validators.required]], 
      taskNumberEndDate  : [endDate, [Validators.required, CustomValidators.minDate(startDate)]],
      details            : [this.incidentTask.details, Validators.required],
      overrideDate       : [endDate]
    });
    this.setEndDateValidatorSubscription();
  }

  private setEndDateValidatorSubscription() {
    const startDateControl = <FormControl>this.form.get('taskNumberStartDate');
    const endDateControl   = <FormControl>this.form.get('taskNumberEndDate');
    // subscribe to taskNumberStartDate value changes
    this.startDateSub = startDateControl.valueChanges.subscribe(value => {
      endDateControl.setValidators([Validators.required, CustomValidators.minDate(moment(value))]);
      endDateControl.updateValueAndValidity();
    });
  }

  invalid(field: string, parent: FormGroup = this.form): boolean {
    return invalidField(field, parent, this.showErrorsWhen);
  }

  private displayTaskNumber(task: IncidentTask) {
    // Reset the form back to pristine
    this.form.reset();
    let startDate: Date = new Date(this.incidentTask.taskNumberStartDate);
    let endDate: Date   = new Date(this.incidentTask.taskNumberEndDate);
    // flow data back into the form
    this.form.patchValue({
      taskNumber: task.taskNumber,
      community: task.community,
      details: task.details,
      //startDate: new Date(task.startDate),
      taskNumberStartDate: startDate,
      taskNumberEndDate: endDate,
      overrideDate: endDate
    });
  }

  next() {
    this.showErrorsWhen = true;

    // proceed if form is valid
    if (this.form.valid) {
      // show the review part of the form
      this.maker = false;
      this.onSave();
      window.scrollTo(0, 0); // scroll to top
    }
  }

  back() {
    // show the editing parts of the form
    this.maker = true;
    window.scrollTo(0, 0); // scroll to top
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
        // if the task has an ID then we need to update
        this.incidentTaskService.updateIncidentTask(this.incidentTask)
          .subscribe(() => {
            this.submitting = false;

            // add a message to the UI
            this.notificationQueueService.addNotification('Task updated successfully', 'success');

            // done editing the entry. Clear the reference key.
            this.uniqueKeyService.clearKey();

            // go back to the task number list page
            this.router.navigate([`/${this.path}/task-numbers`]);
          }, err => {
            this.notificationQueueService.addNotification('Failed to update task', 'danger');
            console.log('error updating task =', err);
          });
      } else {
        // if the task has no id then we need to create a new one
        this.incidentTaskService.createIncidentTask(this.incidentTask)
          .subscribe(() => {
            this.submitting = false;

            // add a message to the UI
            this.notificationQueueService.addNotification('Task added successfully', 'success');

            // NB - there is no key in this scenario
            // go back to the task number list page
            this.router.navigate([`/${this.path}/task-numbers`]);
          }, err => {
            this.notificationQueueService.addNotification('Failed to add task', 'danger');
            console.log('error creating task =', err);
          });
      }
    }
  }

  toggleOverride(): void {
    this.endDateOverride = !this.endDateOverride;
  }

  updateEndDate(): void {
    this.form.patchValue({
      taskNumberEndDate: this.form.controls.overrideDate.value
    });
    this.toggleOverride();
    const endDateControl = <FormControl>this.form.get("taskNumberEndDate");
    endDateControl.updateValueAndValidity();
  }

  startDateChange(date: Date): void {
    const minEndDate: moment.Moment = moment(date).add(80, 'h');
    const currentEndDate: moment.Moment = moment(this.form.controls.taskNumberEndDate.value);
    // ensure the end date is at least 80 hours after the start
    if (minEndDate.isAfter(currentEndDate)) {
      this.form.patchValue({
        taskNumberEndDate: minEndDate.toDate()
      });
      this.overrideDate = minEndDate.toDate();
    }
  }

  cancel() {
    // clear the loaded record if available
    this.uniqueKeyService.clearKey();
    // navigate back home
    this.router.navigate([`/${this.path}/task-numbers`]);
  }

  onSave() {
    const f = this.form.value;
    this.incidentTask.taskNumber = f.taskNumber;
    this.incidentTask.community = f.community;
    this.incidentTask.details = f.details;
    //this.incidentTask.startDate = (f.startDate as Date).toJSON(); // make sure JS dates are properly serialized
    this.incidentTask.taskNumberStartDate = (f.taskNumberStartDate as Date).toJSON();
    this.incidentTask.taskNumberEndDate = (f.taskNumberEndDate as Date).toJSON();
  }
}
