import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppVersion } from 'src/app/core/models/app-version.model';
import { VolunteerTaskService } from 'src/app/core/services/volunteer-task.service';

@Component({
  templateUrl: './active-task.component.html',
  styleUrls: ['./active-task.component.scss']
})

export class ActiveTaskComponent {
  taskNumber: string;
  constructor(
    public activeModal: NgbActiveModal,
    private volunteerTaskService: VolunteerTaskService
  ) { }

  assignTaskNumber() {
    this.volunteerTaskService.setVolunteerTaskForCurrentUser(this.taskNumber)
      .subscribe((result) => {
        debugger;
      });
  }
}
