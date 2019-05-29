import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Organization } from '../../../core/models';
import { Router } from '@angular/router';
import { OrganizationService } from '../../../core/services/organization.service';
import { FormControl } from '@angular/forms';
import { NotificationQueueService } from '../../../core/services/notification-queue.service';
import { AuthService } from '../../../core/services/auth.service';
import { UniqueKeyService } from '../../../core/services/unique-key.service';

@Component({
  selector: 'app-organization-maker',
  templateUrl: './organization-maker.component.html',
  styleUrls: ['./organization-maker.component.scss']
})
export class OrganizationMakerComponent implements OnInit, AfterViewInit {

  maker: boolean;
  editMode: boolean;
  submitting = false; // tracks if in the process of submitting for the UI
  organization: Organization = null;
  // path for routing based on user role
  path: string;

  // form value collectors
  organizationName: FormControl;
  adminBceid: FormControl;
  adminLastName: FormControl;
  adminFirstName: FormControl;
  community: FormControl;

  constructor(
    private router: Router,
    private organizationService: OrganizationService,
    private notificationQueueService: NotificationQueueService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    // subscribe to the path so we can route based on user role
    this.authService.path.subscribe(p => this.path = p);

    // initialize form controls
    this.organizationName = new FormControl('');
    this.adminBceid = new FormControl('');
    this.adminLastName = new FormControl('');
    this.adminFirstName = new FormControl('');
    this.community = new FormControl('');

    const orgId = this.uniqueKeyService.getKey();
    if (orgId) {
      // there may be an organization to edit because the route looks right
      // TODO: error checking if org not found
      this.organizationService.getOrganizationById(orgId)
        .subscribe(organization => {
          this.editMode = true;
          this.maker = true;
          this.organization = organization;

          // set form fields
          this.organizationName.setValue(organization.name);
          this.adminBceid.setValue(organization.adminBCeID);
          this.adminLastName.setValue(organization.adminLastName);
          this.adminFirstName.setValue(organization.adminFirstName);
          this.community.setValue(organization.community);
        });
    } else {
      // no orgId -> add organization
      this.editMode = false;
      this.maker = true;
      this.organization = {
        name: '',
        adminBCeID: '',
        adminLastName: '',
        adminFirstName: '',
        community: null,
        region: null,
        regionalDistrict: null
      };
      // no form fields to set
    }
  }

  ngAfterViewInit() {
    // focus the first input
    const elements = document.getElementsByTagName('input');
    if (elements.length > 0) {
      elements[0].focus();
    }
  }

  next(): void {
    // only go to next page if all fields are non null
    if (this.editMode && this.organizationName.value && this.community.value) {
      this.maker = false;
      this.onSave();
      window.scrollTo(0, 0); // scroll to top

    } else if (!this.editMode && this.organizationName.value && this.adminBceid.value && this.adminLastName.value && this.adminFirstName.value && this.community.value) {
      this.maker = false;
      this.onSave();
      window.scrollTo(0, 0); // scroll to top

    } else {
      alert('All fields are required.');
    }
  }

  back() {
    // show the editing parts of the form
    this.maker = true;
    window.scrollTo(0, 0); // scroll to top
  }

  private onSave(): void {
    // stuff the data back into the organization object
    const organization: Organization = this.organization;
    organization.id = this.organization.id || null; // keep the id for updates
    // save content from the form
    organization.name = this.organizationName.value;
    organization.legalName = '-'; // TODO: query API for this
    organization.adminBCeID = this.adminBceid.value;
    organization.adminLastName = this.adminLastName.value;
    organization.adminFirstName = this.adminFirstName.value;
    organization.community = this.community.value;
  }

  submit(addUsers?: boolean) {
    this.submitting = true;
    // check if this is an update
    if (this.organization.id) {
      // if the organization has an ID then we need to update
      this.organizationService.updateOrganization(this.organization)
        .subscribe(() => {
          this.submitting = false;
          // add a message to the UI
          this.notificationQueueService.addNotification('Organization updated successfully', 'success');
          // if addUsers then route to the add users page
          // else route back to the organizations list
          if (addUsers) {
            // save the organization id and route to the volunteer adder page
            this.uniqueKeyService.setKey(this.organization.id);
            this.router.navigate([`/${this.path}/organization/${this.organization.id}/volunteers`]);
          } else {
            // go back to the organization page
            this.router.navigate([`/${this.path}/organizations`]);
          }
          // done editing the key. Clear it.
          this.uniqueKeyService.clearKey();
        });
    } else {
      // if the organization has no id then we need to create a new one
      this.organizationService.createOrganization(this.organization)
        .subscribe(organization => {
          this.submitting = false;
          // add a message to the UI
          this.notificationQueueService.addNotification('Organization added successfully', 'success');
          // if addUsers then route to the add users page
          // else route back to the organizations list
          if (addUsers) {
            // save the organization id and route to the volunteer adder page
            this.uniqueKeyService.setKey(organization.id);
            this.router.navigate([`/${this.path}/organization/${organization.id}/volunteers`]);
          } else {
            // go back to the organization page
            this.router.navigate([`/${this.path}/organizations`]);
          }
          // NB - there is no key in this scenario
        });
    }
  }

  cancel() {
    // clear the loaded record if available
    this.uniqueKeyService.clearKey();
    // navigate back home
    this.router.navigate([`/${this.path}/organizations`]);
  }

}
