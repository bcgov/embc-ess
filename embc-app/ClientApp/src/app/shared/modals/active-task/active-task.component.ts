import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppVersion } from 'src/app/core/models/app-version.model';
import { VolunteerTaskService } from 'src/app/core/services/volunteer-task.service';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { Volunteer } from 'src/app/core/models';
import { VolunteerTask } from 'src/app/core/models/volunteer-task.model';
import { filter } from 'rxjs/operators';


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
  ) { 
    this.store.select(state => state.volunterTask)
    .pipe(filter(task => !!task))
    .subscribe(task => {
        this.activeModal.close();
    });
  }

  assignTaskNumber() {
    this.volunteerTaskService.setVolunteerTask(this.taskNumber)
      .subscribe((result: VolunteerTask) => {
        this.activeModal.close();
      });
  }
}
