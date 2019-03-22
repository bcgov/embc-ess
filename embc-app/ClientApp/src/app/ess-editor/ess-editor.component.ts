import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-ess-editor',
  templateUrl: './ess-editor.component.html',
  styleUrls: ['./ess-editor.component.scss']
})
export class EssEditorComponent implements OnInit {
  editMode: boolean = false;
  constructor() { }

  // collect the four form values and submit them back to the form
  lastName: FormControl;
  firstName: FormControl;
  bceid: FormControl;
  restrictedAccess: FormControl;

  ngOnInit() {
    this.initForm();
  }
  initForm() {
    this.bceid = new FormControl('');
    this.lastName = new FormControl('');
    this.firstName = new FormControl('');
    this.restrictedAccess = new FormControl(false)
  }
  submit() {
    alert(this.lastName.value + '-' + this.firstName.value + '-' + this.bceid.value + '-' + this.restrictedAccess.value);
  }
}
