import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VolunteerService } from '../core/services/volunteer.service';
import { Store } from '@ngrx/store';
import { AppState } from '../store';
import { FormControl } from '@angular/forms';
import { Volunteer } from '../core/models';
import { UpdateVolunteer } from '../store/volunteer/volunteer.actions';

@Component({
  selector: 'app-volunteer-maker',
  templateUrl: './volunteer-maker.component.html',
  styleUrls: ['./volunteer-maker.component.scss']
})
export class VolunteerMakerComponent implements OnInit {

  editMode = false;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private volunteerService: VolunteerService,
    private store: Store<AppState>,
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
    if (this.route.snapshot.params.id) {
      // there may be a user to edit because the route looks right
      this.volunteerService.getVolunteerById(this.route.snapshot.params.id).subscribe((v: Volunteer) => {
        // save the volunteer for filling in information later.
        this.lastName.setValue(v.lastName);
        this.firstName.setValue(v.firstName);
        this.bceid.setValue(v.bceidAccountNumber);
        this.restrictedAccess.setValue(v.canAccessRestrictedFiles);

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


  next(): void {
    // when routing to the next page we save first into the application state.
    this.onSave();
    // TODO: Enable restricted files later.
    if (this.volunteer.lastName && this.volunteer.firstName && this.volunteer.bceidAccountNumber) {
      // if (this.volunteer.lastName && this.volunteer.firstName && this.volunteer.bceidAccountNumber && this.volunteer.canAccessRestrictedFiles != null) {

      // simple check to be sure that all fields are included.
      const nextRoute = this.editMode ? '../../confirmation' : '../confirmation';
      this.router.navigate([nextRoute], { relativeTo: this.route });
    } else {
      alert("All fields are required.");
    }
  }

  onSave(): void {
    // stuff the data back into the volunteer object
    const volunteer: Volunteer = this.volunteer;
    volunteer.lastName = this.lastName.value;
    volunteer.firstName = this.firstName.value;
    volunteer.bceidAccountNumber = this.bceid.value;
    // nobody should be grandfathered into restricted files.
    volunteer.canAccessRestrictedFiles = null; // TODO this should be a choice when we show the form info.
    // volunteer.canAccessRestrictedFiles = this.restrictedAccess.value;
    this.store.dispatch(new UpdateVolunteer({ volunteer }))
  }

}
