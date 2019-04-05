import { Component, OnInit } from '@angular/core';
import { IncidentTask } from 'src/app/core/models';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { takeWhile } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { IncidentTaskService } from 'src/app/core/services/incident-task.service';
import { UpdateIncidentTask } from 'src/app/store/incident-tasks/incident-tasks.actions';

@Component({
  selector: 'app-admin-add-task-number-confirmation',
  templateUrl: './admin-add-task-number-confirmation.component.html',
  styleUrls: ['./admin-add-task-number-confirmation.component.scss']
})
export class AdminAddTaskNumberConfirmationComponent implements OnInit {
  componentActive = true;
  currentIncidentTask$ = this.store.select(s => s.incidentTasks.currentIncidentTask);
  incidentTask: IncidentTask;
  submitting = false;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private route: ActivatedRoute,
    private incidentTaskService: IncidentTaskService,
  ) { }

  ngOnInit() {
    this.currentIncidentTask$.pipe(takeWhile(() => this.componentActive))
      .subscribe(i => {
        // if there are any mandatory fields that are null route back to the main component
        if (!i.taskNumber || !i.community || !i.details) {
          this.router.navigate(['../fill'], { relativeTo: this.route });
        } else {
          // we have somethings to show so we show it.
          this.incidentTask = i;
        }
      });
  }
  back() {
    // go back
    this.onSave();
    this.router.navigate(['../fill'], { relativeTo: this.route });
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

  onSave() {
    // save the data into ngrx state
    const incidentTask = this.incidentTask;
    this.store.dispatch(new UpdateIncidentTask({ incidentTask }))
  }

}
