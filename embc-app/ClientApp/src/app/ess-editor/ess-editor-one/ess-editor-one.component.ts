import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { VolunteerService } from 'src/app/core/services/volunteer.service';
import { ActivatedRoute } from '@angular/router';
import { Volunteer } from 'src/app/core/models';

@Component({
  selector: 'app-ess-editor-one',
  templateUrl: './ess-editor-one.component.html',
  styleUrls: ['./ess-editor-one.component.scss']
})
export class EssEditorOneComponent implements OnInit {
  editMode = false;
  constructor(
    private route: ActivatedRoute,
    private volunteerService: VolunteerService,
  ) { }

  // collect the four form values and submit them back to the form
  lastName: FormControl;
  firstName: FormControl;
  bceid: FormControl;
  restrictedAccess: FormControl;
  volunteer: Volunteer = {
    id: '',
    firstName: '',
    initials: '',
    lastName: '',
    nickname: '',
    gender: '',
    dob: null,
    bceidAccountNumber: '',
    personType: 'VOLN',
    canAccessRestrictedFiles: null,
    organization: null,
    isAdministrator: null,
    isPrimaryContact: null,
  };

  ngOnInit() {
    this.initForm();
    // if there are route params we should grab them
    if (this.route.snapshot.params.essUser) {
      // there may be a user to edit because the route looks right
      this.volunteerService.getVolunteerByBceidAccountNumber(this.route.snapshot.params.essUser).subscribe((v: Volunteer) => {
        // save the volunteer for filling in information later.
        this.volunteer = v;
        this.editMode = true;
      });
    } else {
      // this is a fresh form and will be a simple add user
      this.editMode = false;
    }
  }
  initForm() {
    this.bceid = new FormControl('');
    this.lastName = new FormControl('');
    this.firstName = new FormControl('');
    this.restrictedAccess = new FormControl(false);
  }
  displayVolunteer(v: Volunteer) {
    // flow the volunteer into the form
  }

  submit() {
    // stuff the data back into the volunteer object
    this.volunteer.lastName = this.lastName.value;
    this.volunteer.firstName = this.firstName.value;
    this.volunteer.bceidAccountNumber = this.bceid.value;
    this.volunteer.canAccessRestrictedFiles = this.restrictedAccess.value;

    if (this.volunteer.id) {
      // if the volunteer has an ID we need to update
      this.volunteerService.updateVolunteer(this.volunteer)
        .subscribe(v => {
          alert(v);
        });
    } else {
      // if the volunteer has no id we need to create a new one
      this.volunteerService.createVolunteer(this.volunteer)
        .subscribe(v => {
          alert(v);
        });
    }
    // alert(this.lastName.value + '-' + this.firstName.value + '-' + this.bceid.value + '-' + this.restrictedAccess.value);
  }
}
