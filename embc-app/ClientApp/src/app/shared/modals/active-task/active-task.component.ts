import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { VolunteerTaskService } from 'src/app/core/services/volunteer-task.service';
import { ListResult, IncidentTask } from 'src/app/core/models';
import { VolunteerTask } from 'src/app/core/models/volunteer-task.model';
import { Validators, FormControl } from '@angular/forms';
import { IncidentTaskService } from 'src/app/core/services/incident-task.service';

@Component({
  templateUrl: './active-task.component.html',
  styleUrls: ['./active-task.component.scss']
})

export class ActiveTaskComponent implements OnInit {
  taskNumberControl: FormControl = new FormControl("", [Validators.required]);
  openTasks: ListResult<IncidentTask>;
  selectedTask: IncidentTask;
  // Flags for UI
  enterTaskNumView: boolean = true;
  displayErrorText: boolean = false;
  
  constructor(
    public activeModal: NgbActiveModal,
    private volunteerTaskService: VolunteerTaskService,
    private taskService: IncidentTaskService
  ) {  }

  public get errorText() {
    return this.taskNumberControl.invalid
          ? "Please enter a valid task number to proceed"
          : "The task number you entered is not valid"
  }

  ngOnInit(): void {
    this.taskService.getOpenIncidentTasks()
      .subscribe(result => this.openTasks = result);
  }

  submitTaskNumber() {
    console.log(this.openTasks);
    // Validate that they've selected an open task
    this.selectedTask = this.openTasks.data.find(task => task.taskNumber === this.taskNumberControl.value);
    console.log("selected task:", this.selectedTask);
    const isValid: boolean = this.selectedTask != null && this.taskNumberControl.valid;
    this.displayErrorText = !isValid;
    if (isValid) {
      this.enterTaskNumView = false;
    }
  }

    // Go back to enter task number
    back() {
      this.enterTaskNumView = true;
      this.displayErrorText = false;
    }


  assignTaskNumber() {
    this.volunteerTaskService.setVolunteerTask(this.taskNumberControl.value)
      .subscribe((result: VolunteerTask) => {
        //this.activeModal.close();
      });
      this.activeModal.close();
  }
}
