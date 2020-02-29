import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModalComponent } from 'src/app/shared/modals/confirm/confirm.component';
import { ActiveTaskComponent } from 'src/app/shared/modals/active-task/active-task.component';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';

@Component({
  selector: 'app-volunteer-registrations-page',
  templateUrl: './volunteer-registrations-page.component.html',
  styleUrls: ['./volunteer-registrations-page.component.scss']
})
export class VolunteerRegistrationsPageComponent {
  taskNumber: string;

 
  constructor(private modals: NgbModal,
    private store: Store<AppState>) { }

  ngOnInit() {
    this.store.select(state => state.volunterTask.taskNumber)
    .subscribe(taskNumber => {
      this.taskNumber = taskNumber;
      if(!taskNumber){
        this.showVersion();
      }
    });
  }

  
  showVersion(){
    const modalRef =this.modals.open(ActiveTaskComponent, {backdrop:true, beforeDismiss: () => false});
    // modalRef.componentInstance.version = this.appVersion;
  }

}
