import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthService } from 'src/app/core/services/auth.service';
import { VolunteerService } from 'src/app/core/services/volunteer.service';
import { OrganizationService } from 'src/app/core/services/organization.service';
import { Volunteer, Organization } from 'src/app/core/models';
import { NotificationQueueService } from 'src/app/core/services/notification-queue.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { invalidField } from 'src/app/shared/utils';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';

@Component({
  selector: 'app-volunteer-maker',
  templateUrl: './volunteer-maker.component.html',
  styleUrls: ['./volunteer-maker.component.scss']
})
export class VolunteerMakerComponent implements OnInit {

  editing = true; // whether we are adding/editing or reviewing
  submitting = false; // whether we are submitting data to BE
  editMode: ('ADD' | 'EDIT') = null; // not set by default
  path: string = null; // the base path for routing
  currentOrganization: Organization = null; // organization in context (original org)
  volunteer: Volunteer = null;

  // the model for the form data collected
  form: FormGroup = null;

  // convenience getter for easy access to form fields
  get f(): any { return this.form.controls; }

  // to run validation after user clicks the Submit button
  shouldValidateForm = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private volunteerService: VolunteerService,
    private organizationService: OrganizationService,
    private authService: AuthService,
    private notificationQueueService: NotificationQueueService,
    private uniqueKeyService: UniqueKeyService,
    private fb: FormBuilder,
  ) { }

  ngOnInit() {
    // save the appropriate path for routing
    this.authService.path.subscribe((path: string) => this.path = path);

    // initialize form
    this.form = this.fb.group({
      lastName: ['', Validators.required],
      firstName: ['', Validators.required],
      bceidAccountNumber: ['', { validators: [Validators.required], updateOn: 'blur' }],
      isAdministrator: ['', Validators.required]
    });

    // get the organization from the route parameters
    const orgId = this.authService.currentUser.accountid;
    if (this.authService.currentUser.accountid) {
      this.organizationService.getOrganizationById(orgId)
        .subscribe((organization: Organization) => {
          this.currentOrganization = organization;
        }, err => {
          console.log('error getting organization =', err);
          this.notificationQueueService.addNotification('Failed to load organization', 'danger');

          // go back to the volunteers list
          this.cancel();
        });
    }
    // get the volunteer's id from the unique key service
    const volunteerId = this.uniqueKeyService.getKey(); // may be null
    if (volunteerId) {
      // this is an edit!
      // get volunteer to edit
      this.volunteerService.getVolunteerById(volunteerId)
        .subscribe((volunteer: Volunteer) => {
          this.editMode = 'EDIT';
          this.volunteer = volunteer;

          // set form fields
          this.form.patchValue({
            lastName: volunteer.lastName,
            firstName: volunteer.firstName,
            bceidAccountNumber: volunteer.bceidAccountNumber,
            isAdministrator: volunteer.isAdministrator
          });

          // do not allow duplicate BCeIDs
          this.enableBceidValidation(volunteer.bceidAccountNumber);

          // finally everything is loaded
          this.setInitialFocus();
        }, err => {
          console.log('error getting volunteer =', err);
          this.notificationQueueService.addNotification('Failed to get volunteer', 'danger');

          // go back to the volunteers list
          this.cancel();
        });
    } else {
      // This is an add volunteer to organization
      // no volunteerId -> add user
      this.editMode = 'ADD';

      // this is a fresh form and will be a simple add organization
      this.volunteer = <Volunteer>{
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
        isAdministrator: false, // if you are making a new volunteer as a local auth it won't be an admin user.
        isPrimaryContact: false // if you are making a new volunteer as a local auth it won't be the primary contact for your org.
      };

      // do not allow duplicate BCeIDs
      this.enableBceidValidation('');

      // finally everything is loaded
      this.setInitialFocus();
    }
  }

  private setInitialFocus() {
    // focus the first input
    const elements = document.getElementsByClassName('form-control') as HTMLCollectionOf<HTMLElement>;
    if (elements.length > 0) {
      elements[0].focus();
    } else {
      // wait for elements to display and try again
      setTimeout(() => this.setInitialFocus(), 100);
    }
  }

  private enableBceidValidation(originalValue?: string) {
    const ctrl = this.form ? this.form.controls.bceidAccountNumber : null;
    if (ctrl) {
      ctrl.setAsyncValidators([CustomValidators.uniqueBceid(this.volunteerService, originalValue)]);
      ctrl.updateValueAndValidity();
    }
  }

  private disableBceidValidation() {
    const ctrl = this.form ? this.form.controls.bceidAccountNumber : null;
    if (ctrl) {
      ctrl.clearAsyncValidators();
      ctrl.updateValueAndValidity();
    }
  }

  invalid(field: string, parent: FormGroup = this.form): boolean {
    return invalidField(field, parent, this.shouldValidateForm);
  }

  next() {
    this.shouldValidateForm = true;
    // check validation of form
    if (this.form.valid) {
      // update volunteer object
      this.volunteer = { ...this.volunteer, ...this.form.value, organization: this.currentOrganization };
      // if successful you switch from edit to view mode.
      this.editing = false;
      window.scrollTo(0, 0); // scroll to top
    }
  }

  back() {
    // show the editing parts of the form
    this.editing = true;
    window.scrollTo(0, 0); // scroll to top

    // page controls have been updated
    this.setInitialFocus();
  }

  submit(addAnother?: boolean) {
    this.submitting = true;
    // check if this is an update
    if (this.volunteer.id) {
      // if the volunteer has an ID we need to update
      this.volunteerService.updateVolunteer(this.volunteer)
        .subscribe(() => {
          this.submitting = false;
          // add a notification about the update
          this.notificationQueueService.addNotification('User updated successfully', 'success');

          if (addAnother) {
            this.resetForm();
          } else {
            // go back to the volunteers list
            this.cancel();
          }
        }, err => {
          this.submitting = false;
          console.log('error updating volunteer =', err);
          this.notificationQueueService.addNotification('Failed to update user. Please check that the BCeID is unique.', 'danger');
        });
    } else {
      // if the volunteer has no id we need to create a new one
      this.volunteerService.createVolunteer(this.volunteer)
        .subscribe(() => {
          this.submitting = false;
          // add a notification about the creation
          this.notificationQueueService.addNotification('User added successfully', 'success');

          // if addAnother route then reset this form
          // else route back to the volunteers list
          if (addAnother) {
            this.resetForm();
          } else {
            // navigate back to the volunteers list
            this.cancel();
          }
        }, err => {
          this.submitting = false;
          console.log('error creating volunteer =', err);
          this.notificationQueueService.addNotification('Failed to add user', 'danger');
        });
    }
  }

  private resetForm() {
    // reset form fields (except organization)
    this.form.patchValue({
      lastName: '',
      firstName: '',
      bceidAccountNumber: '',
      isAdministrator: false // default to false
    });
    this.shouldValidateForm = false;

    // go back to the first page
    this.back();
  }

  cancel() {
    // go back to current user's list of volunteers
    this.router.navigate([`/${this.path}/volunteers`]);
  }

}
