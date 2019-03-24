import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { VolunteerService } from 'src/app/core/services/volunteer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Volunteer } from 'src/app/core/models';
import { AppState } from 'src/app/store';
import { Store } from '@ngrx/store';
import { UpdateVolunteer } from 'src/app/store/volunteer/volunteer.actions';

@Component({
  selector: 'app-ess-editor-one',
  templateUrl: './ess-editor-one.component.html',
  styleUrls: ['./ess-editor-one.component.scss']
})
export class EssEditorOneComponent implements OnInit {
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
    if (this.route.snapshot.params.essUser) {
      // there may be a user to edit because the route looks right
      this.volunteerService.getVolunteerByBceidAccountNumber(this.route.snapshot.params.essUser).subscribe((v: Volunteer) => {
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
    this.router.navigate(['user-edit/confirmation']);
  }

  onSave(): void {
    // stuff the data back into the volunteer object
    const volunteer: Volunteer = this.volunteer;
    volunteer.lastName = this.lastName.value;
    volunteer.firstName = this.firstName.value;
    volunteer.bceidAccountNumber = this.bceid.value;
    volunteer.canAccessRestrictedFiles = this.restrictedAccess.value;
    this.store.dispatch(new UpdateVolunteer({ volunteer }))
  }
}
