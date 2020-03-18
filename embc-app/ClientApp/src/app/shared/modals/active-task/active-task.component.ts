import { Component, Input, AfterViewInit, ChangeDetectorRef, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppVersion } from 'src/app/core/models/app-version.model';
import { VolunteerTaskService } from 'src/app/core/services/volunteer-task.service';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { Volunteer } from 'src/app/core/models';
import { VolunteerTask } from 'src/app/core/models/volunteer-task.model';
import { filter } from 'rxjs/operators';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';

@Component({
  templateUrl: './active-task.component.html',
  styleUrls: ['./active-task.component.scss']
})

export class ActiveTaskComponent implements OnInit {
  taskNumberControl: FormControl = new FormControl("", [Validators.required]);
  constructor(
    public activeModal: NgbActiveModal,
    private volunteerTaskService: VolunteerTaskService,
  ) { 

  }
  ngOnInit(): void {

  }



  assignTaskNumber() {
    this.volunteerTaskService.setVolunteerTask(this.taskNumberControl.value)
      .subscribe((result: VolunteerTask) => {
        this.activeModal.close();
      });
  }
}
