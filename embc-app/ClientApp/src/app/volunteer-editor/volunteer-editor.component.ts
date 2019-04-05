import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-volunteer-editor',
  templateUrl: './volunteer-editor.component.html',
  styleUrls: ['./volunteer-editor.component.scss']
})
export class VolunteerEditorComponent implements OnInit {
  // editMode = false;
  constructor() { }

  // collect the four form values and submit them back to the form
  // lastName: FormControl;
  // firstName: FormControl;
  // bceid: FormControl;
  // restrictedAccess: FormControl;

  ngOnInit() {
    // this.initForm();
  }
  // initForm() {
  //   this.bceid = new FormControl('');
  //   this.lastName = new FormControl('');
  //   this.firstName = new FormControl('');
  //   this.restrictedAccess = new FormControl(false);
  // }
  // submit() {
  //   alert(this.lastName.value + '-' + this.firstName.value + '-' + this.bceid.value + '-' + this.restrictedAccess.value);
  // }
}
