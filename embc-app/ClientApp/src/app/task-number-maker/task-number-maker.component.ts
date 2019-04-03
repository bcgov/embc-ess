import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { IncidentTask } from '../core/models';
import { compareById } from '../shared/utils';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AppState } from '../store';
import { IncidentTaskService } from '../core/services/incident-task.service';
import { UpdateIncidentTask } from '../store/incident-tasks/incident-tasks.actions';

@Component({
  selector: 'app-task-number-maker',
  templateUrl: './task-number-maker.component.html',
  styleUrls: ['./task-number-maker.component.scss']
})
export class TaskNumberMakerComponent implements OnInit {

  maker = true; // determines if the widget is in edit or confirmation mode
  editMode = false;
  submitting = false;

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

  // convenience getters so we can use helper functions in Angular templates
  compareById = compareById;

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
    // this.onSave();
    // only go next if all fields are non null
    if (this.taskNumber.value && this.community.value && this.details.value) {
      this.maker = false;
      // when routing to the next page we save first into the application state.
      // this.onSave();
      // information saved in state. Navigate to confirm page
      // const nextRoute = this.editMode ? '../../confirmation' : '../confirmation';
      // this.router.navigate([nextRoute], { relativeTo: this.route });
    } else {
      alert("All fields are required to continue.");
    }
  }



  back() {
    // go back
    // this.onSave();
    // this.router.navigate(['../fill'], { relativeTo: this.route });
  }
  submit() {
    this.submitting = true;
    if (!(this.incidentTask.community && this.incidentTask.details && this.incidentTask.taskNumber)) {
      // todo go somewhere useful for this provincial user after routing is fixed.
      this.submitting = false;
      this.router.navigate(['../fill'], { relativeTo: this.route });
    } else {
      // check if this is an update
      if (this.incidentTask.id) {
        // if the volunteer has an ID we need to update
        this.incidentTaskService.updateIncidentTask(this.incidentTask)
          .subscribe(() => {
            this.submitting = false;
            // go back to the volunteer team dashboard
            this.router.navigate(['../../task-numbers'], { relativeTo: this.route });
          });
      } else {
        // if the volunteer has no id we need to create a new one
        this.incidentTaskService.createIncidentTask(this.incidentTask)
          .subscribe(i => {
            this.submitting = false;
            // go back to the volunteer team dashboard
            this.router.navigate(['../../task-numbers'], { relativeTo: this.route });
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
    // stuff the data into an incidentTask object
    this.store.dispatch(new UpdateIncidentTask({ incidentTask }));
  }
}
