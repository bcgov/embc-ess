import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'confirm-component',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.scss']
})

export class ConfirmModalComponent {
  title = 'Confirm';
  message = 'Are you sure?';
  okOnly = false;

  constructor(public activeModal: NgbActiveModal) { }

  confirm() {
    this.activeModal.close(); // promise returns True
  }

  cancel() {
    this.activeModal.dismiss(); // promise returns False
  }
}
