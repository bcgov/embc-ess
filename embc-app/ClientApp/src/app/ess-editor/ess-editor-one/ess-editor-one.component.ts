import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-ess-editor-one',
  templateUrl: './ess-editor-one.component.html',
  styleUrls: ['./ess-editor-one.component.scss']
})
export class EssEditorOneComponent implements OnInit {
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
