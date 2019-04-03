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
  maker = true;
  editMode = false;
  componentActive = true;
  // currentVolunteer$ = this.store.select(s => s.volunteers.currentVolunteer);
  submitting = false; // tracks if in the process of submitting for the UI

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
    // initialize form controls
    this.bceid = new FormControl('');
    this.lastName = new FormControl('');
    this.firstName = new FormControl('');
    this.restrictedAccess = new FormControl(false);    // if there are route params we should grab them

    if (this.route.snapshot.params.id) {
      // there may be a user to edit because the route looks right
      this.volunteerService.getVolunteerById(this.route.snapshot.params.id)
        .subscribe((v: Volunteer) => {
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

  next(): void {
    // when routing to the next page we save first into the application state.
    this.onSave();
    // TODO: Enable restricted files later.
    if (this.volunteer.lastName && this.volunteer.firstName && this.volunteer.bceidAccountNumber) {
      // if (this.volunteer.lastName && this.volunteer.firstName && this.volunteer.bceidAccountNumber && this.volunteer.canAccessRestrictedFiles != null) {
      this.maker = false;
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
    // update the stored version
    this.volunteer = volunteer;
  }
  // -----------------------------------------------------------

  back() {
    // this shows the part of the UI that makes edits and hides the parts that don't.
    this.maker = true;
  }

  submit(addAnother?: boolean) {
    this.submitting = true;
    // check if this is an update
    if (this.volunteer.id) {
      // if the volunteer has an ID we need to update
      this.volunteerService.updateVolunteer(this.volunteer)
        .subscribe(() => {
          this.submitting = false;
          // if addAnother route back to the add page else route back to the volunteer-team-editor
          if (addAnother) {
            this.resetForm();
          } else {
            // go back to the volunteer team dashboard
            this.router.navigate(['/']);
          }
        });
    } else {
      // if the volunteer has no id we need to create a new one
      this.volunteerService.createVolunteer(this.volunteer)
        .subscribe(v => {
          this.submitting = false;
          // if addAnother route back to the add page else route back to the volunteer-team-editor
          if (addAnother) {
            this.resetForm();
          } else {
            // go back to the volunteer team dashboard
            this.router.navigate(['/']);
          }
        });
    }
  }
  resetForm() {
    this.volunteer = null;
    // initialize form controls
    this.bceid = new FormControl('');
    this.lastName = new FormControl('');
    this.firstName = new FormControl('');
    this.restrictedAccess = new FormControl(false);    // if there are route params we should grab them
    // go back to the first page
    this.back();
  }
}
