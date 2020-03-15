import { Component, OnInit, AfterViewInit, Output, EventEmitter } from '@angular/core';
import { Organization } from 'src/app/core/models';
import { Router, ActivatedRoute } from '@angular/router';
import { OrganizationService } from 'src/app/core/services/organization.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NotificationQueueService } from 'src/app/core/services/notification-queue.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { invalidField } from 'src/app/shared/utils';
import { VolunteerService } from 'src/app/core/services/volunteer.service';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';

@Component({
  selector: 'app-organization-maker',
  templateUrl: './organization-maker.component.html',
  styleUrls: ['./organization-maker.component.scss']
})
export class OrganizationMakerComponent implements OnInit, AfterViewInit {

  maker: boolean = null;
  editMode: boolean = null;
  submitting = false; // tracks if in the process of submitting for the UI
  organization: Organization = null;
  path: string = null; // the base path for routing
  form: FormGroup = null;
  showErrorsWhen = false; // wait until the user click NEXT before showing any validation errors

  @Output()
  onEditModeChanged: EventEmitter<boolean> = new EventEmitter<boolean>();

  // convenience getter for easy access to form fields
  get f(): any { return this.form.controls; }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private volunteerService: VolunteerService,
    private organizationService: OrganizationService,
    private notificationQueueService: NotificationQueueService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private fb: FormBuilder,
  ) { }

  ngOnInit() {
    // subscribe to the path so we can route based on user role
    this.authService.path.subscribe((path: string) => this.path = path);

    // initialize form controls
    this.initializeForm();

    const orgId = this.route.snapshot.paramMap.get('orgId'); // may be null

    if (orgId) {
      // there may be an organization to edit because the route looks right
      this.organizationService.getOrganizationById(orgId)
        .subscribe((organization: Organization) => {
          this.editMode = true;
          this.onEditModeChanged.emit(this.editMode);
          this.maker = true;
          this.organization = organization;

          // set form fields
          this.form.patchValue({
            organizationName: organization.name,
            adminBCeID: organization.adminBCeID,
            adminLastName: organization.adminLastName,
            adminFirstName: organization.adminFirstName,
            community: organization.community
          });

          // NB - the admin user fields are not shown on UPDATE so no need to validate the BCeID
          this.disableBceidValidation();
        }, err => {
          this.notificationQueueService.addNotification('Failed to load organization', 'danger');
          console.log('error getting organization =', err);
          this.cancel();
        });
    } else {
      // no orgId -> add organization
      this.editMode = false;
      this.onEditModeChanged.emit(this.editMode);
      this.maker = true;
      this.organization = {
        name: '',
        adminBCeID: '',
        adminLastName: '',
        adminFirstName: '',
        community: null,
        region: null
      };

      // do not allow duplicate BCeIDs
      this.enableBceidValidation('');

      // NB - no form fields to set
    }
  }

  ngAfterViewInit() {
    // focus the first input
    const elements = document.getElementsByClassName('form-control') as HTMLCollectionOf<HTMLElement>;
    if (elements.length > 0) {
      elements[0].focus();
    } else {
      // wait for elements to display and try again
      setTimeout(() => this.ngAfterViewInit(), 100);
    }
  }

  private initializeForm() {
    this.form = this.fb.group({
      organizationName: ['', Validators.required],
      adminBCeID: ['', { validators: [Validators.required], updateOn: 'blur' }],
      adminLastName: ['', Validators.required],
      adminFirstName: ['', Validators.required],
      community: [null, Validators.required]
    });
  }

  private enableBceidValidation(originalValue?: string) {
    const ctrl = this.form ? this.form.controls.adminBCeID : null;
    if (ctrl) {
      ctrl.setAsyncValidators([CustomValidators.uniqueBceid(this.volunteerService, originalValue)]);
      ctrl.updateValueAndValidity();
    }
  }

  private disableBceidValidation() {
    const ctrl = this.form ? this.form.controls.adminBCeID : null;
    if (ctrl) {
      ctrl.clearAsyncValidators();
      ctrl.updateValueAndValidity();
    }
  }

  next(): void {
    this.showErrorsWhen = true;

    // proceed if form is valid
    if (this.form.valid) {
      // show the review part of the form
      this.maker = false;
      this.onSave();
      window.scrollTo(0, 0); // scroll to top
    }
  }

  back() {
    // show the editing parts of the form
    this.maker = true;
    window.scrollTo(0, 0); // scroll to top
  }

  invalid(field: string, parent: FormGroup = this.form): boolean {
    return invalidField(field, parent, this.showErrorsWhen);
  }

  private onSave(): void {
    // stuff the data back into the organization object
    const organization: Organization = this.organization;
    organization.id = this.organization.id || null; // keep the id for updates

    // save content from the form
    organization.name = this.f.organizationName.value;
    organization.legalName = '-'; // FUTURE: query API for this
    organization.adminBCeID = this.f.adminBCeID.value;
    organization.adminLastName = this.f.adminLastName.value;
    organization.adminFirstName = this.f.adminFirstName.value;
    organization.community = this.f.community.value;
  }

  submit(addUsers?: boolean) {
    this.submitting = true;

    if (this.organization.id) {
      // if the organization has an ID then we need to update
      this.organizationService.updateOrganization(this.organization)
        .subscribe(() => {
          this.submitting = false;

          // add a message to the UI
          this.notificationQueueService.addNotification('Organization updated successfully', 'success');

          // if addUsers then go to the volunteer adder page
          // else go back to the organizations page
          if (addUsers) {
            this.router.navigate([`/${this.path}/organization/${this.organization.id}/volunteers`]);
          } else {
            this.router.navigate([`/${this.path}/organizations`]);
          }

          // done editing the key - clear it
          this.uniqueKeyService.clearKey();
        }, err => {
          this.notificationQueueService.addNotification('Failed to update organization', 'danger');
          console.log('error updating organization =', err);
        });
    } else {
      // if the organization has no id then we need to create a new one
      this.organizationService.createOrganization(this.organization)
        .subscribe((organization: Organization) => {
          this.submitting = false;

          // add a message to the UI
          this.notificationQueueService.addNotification('Organization added successfully', 'success');

          // if addUsers then go to the volunteer adder page
          // else go back to the organizations page
          if (addUsers) {
            this.router.navigate([`/${this.path}/organization/${organization.id}/volunteers`]);
          } else {
            this.router.navigate([`/${this.path}/organizations`]);
          }

          // NB - there is no key in this scenario
        }, err => {
          this.notificationQueueService.addNotification('Failed to add organization', 'danger');
          console.log('error creating organization =', err);
        });
    }
  }

  cancel() {
    // clear the loaded record if available
    this.uniqueKeyService.clearKey();
    // go back to list of organizations
    this.router.navigate([`/${this.path}/organizations`]);
  }

}
