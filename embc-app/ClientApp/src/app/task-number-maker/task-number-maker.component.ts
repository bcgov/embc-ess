import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { IncidentTask } from '../core/models';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AppState } from '../store';
import { IncidentTaskService } from '../core/services/incident-task.service';
import { NotificationQueueService } from '../core/services/notification-queue.service';
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
    private notificationQueueService: NotificationQueueService,
  ) { }

  // convenience getters
  get taskNumbersRoute() {
    return this.editMode ? '../../task-numbers' : '../task-numbers';
  }

  ngOnInit() {
    // initialize form for collection
    this.taskNumber = new FormControl('');
    this.details = new FormControl('');
    this.community = new FormControl('');

    if (this.route.snapshot.params.id) {
      // there may be a user to edit because the route has an ID for an incident task
      this.incidentTaskService.getIncidentTask(this.route.snapshot.params.id)
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
            // go back to the volunteer team dashboard
            // this needs to route in two different ways because the ID added for edit acts as a route element
            this.router.navigate([this.taskNumbersRoute], { relativeTo: this.route });
          });
      } else {
        // if the volunteer has no id we need to create a new one
        this.incidentTaskService.createIncidentTask(this.incidentTask)
          .subscribe(i => {
            this.submitting = false;
            // add a message to the UI
            this.notificationQueueService.addNotification('Task number added successfully');
            // go back to the volunteer team dashboard
            this.router.navigate([this.taskNumbersRoute], { relativeTo: this.route });
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
