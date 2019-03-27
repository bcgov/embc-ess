import { Component, OnInit } from '@angular/core';
import { IncidentTask } from 'src/app/core/models';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { VolunteerService } from 'src/app/core/services/volunteer.service';
import { takeWhile } from 'rxjs/operators';

@Component({
  selector: 'app-admin-add-task-number-confirmation',
  templateUrl: './admin-add-task-number-confirmation.component.html',
  styleUrls: ['./admin-add-task-number-confirmation.component.scss']
})
export class AdminAddTaskNumberConfirmationComponent implements OnInit {
  componentActive = true;
  currentIncidentTask$ = this.store.select(s => s.incidentTasks.currentIncidentTask);
  incidentTask: IncidentTask;

  constructor(
    private store: Store<AppState>,
  ) { }

  ngOnInit() {
    this.currentIncidentTask$.pipe(takeWhile(() => this.componentActive))
      .subscribe(i => {
        this.incidentTask = i;
      });
  }

}
