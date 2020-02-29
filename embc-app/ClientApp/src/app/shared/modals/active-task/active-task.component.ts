import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppVersion } from 'src/app/core/models/app-version.model';
import { VolunteerTaskService } from 'src/app/core/services/volunteer-task.service';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import * as VolunteerTaskActions from 'src/app/store/volunteer-task/volunteer-task.actions';
import { Volunteer } from 'src/app/core/models';
import { VolunteerTask } from 'src/app/core/models/volunteer-task.model';


@Component({
  templateUrl: './active-task.component.html',
  styleUrls: ['./active-task.component.scss']
})

export class ActiveTaskComponent {
  taskNumber: string;
  constructor(
    public activeModal: NgbActiveModal,
    private store: Store<AppState>,
    private volunteerTaskService: VolunteerTaskService
  ) { }

  assignTaskNumber() {
    this.volunteerTaskService.setVolunteerTaskForCurrentUser(this.taskNumber)
      .subscribe((result: VolunteerTask) => {
        this.store.dispatch(new VolunteerTaskActions.SetCurrentVolunteerTask({ task: result.incidentTask.taskNumber }));
        this.activeModal.close();
      });
  }
}
