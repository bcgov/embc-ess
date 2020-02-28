import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModalComponent } from 'src/app/shared/modals/confirm/confirm.component';
import { ActiveTaskComponent } from 'src/app/shared/modals/active-task/active-task.component';

@Component({
  selector: 'app-volunteer-registrations-page',
  templateUrl: './volunteer-registrations-page.component.html',
  styleUrls: ['./volunteer-registrations-page.component.scss']
})
export class VolunteerRegistrationsPageComponent {

 
  constructor(private modals: NgbModal) { }

  ngOnInit() {
    this.showVersion();
  }

  
  showVersion(){
    const modalRef =this.modals.open(ActiveTaskComponent, {backdrop:true, beforeDismiss: () => false});
    // modalRef.componentInstance.version = this.appVersion;
  }

}
