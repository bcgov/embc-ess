import { Component, OnInit } from '@angular/core';
import { Organization } from '../core/models';
import { ActivatedRoute, Router } from '@angular/router';
import { VolunteerService } from '../core/services/volunteer.service';
import { AppState } from '../store';
import { Store } from '@ngrx/store';
import { OrganizationService } from '../core/services/organization.service';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-organization-maker',
  templateUrl: './organization-maker.component.html',
  styleUrls: ['./organization-maker.component.scss']
})
export class OrganizationMakerComponent implements OnInit {
  maker = true;
  editMode = false;
  submitting = false; // tracks if in the process of submitting for the UI

  organization: Organization;

  // form value collectors
  organizationName: FormControl;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private organizationService: OrganizationService,
    private store: Store<AppState>,
  ) { }

  ngOnInit() {
    // initialize form controls
    this.organizationName = new FormControl('');

    if (this.route.snapshot.params.id) {
      // there may be a user to edit because the route looks right
      this.organizationService.getOrganizationById(this.route.snapshot.params.id)
        .subscribe((o: Organization) => {
          this.organization = o
          // save the volunteer for filling in information later.
          this.organizationName.setValue(o.name);
          this.editMode = true;
        });
    } else {
      // this is a fresh form and will be a simple add user
      this.editMode = false;
    }
  }

  next(): void {
    // when routing to the next page we save first into the application state.
    this.onSave(); // side effect is copying into the registration global
    // TODO: Enable restricted files later.
    if (this.organization.name) {
      // if (this.volunteer.lastName && this.volunteer.firstName && this.volunteer.bceidAccountNumber && this.volunteer.canAccessRestrictedFiles != null) {
      this.maker = false;
    } else {
      alert("All fields are required.");
    }
  }

  onSave(): void {
    // stuff the data back into the volunteer object
    const organization: Organization = this.organization;
    // save content from the form
    organization.name = this.organizationName.value;

  }

  back() {
    // this shows the part of the UI that makes edits and hides the parts that don't.
    this.maker = true;
  }

  submit() {
    this.submitting = true;
    if (this.organization.id) {
      // if the volunteer has an ID we need to update
      this.organizationService.updateOrganization(this.organization)
        .subscribe(() => {
          this.submitting = false;
          // go back to the volunteer team dashboard
          this.router.navigate(['/']); // TODO: go somewhere that the application state can go
        });
    } else {
      // if the volunteer has no id we need to create a new one
      this.organizationService.createOrganization(this.organization)
        .subscribe(v => {
          this.submitting = false;
          // if addAnother route back to the add page else route back to the volunteer-team-editor
          // go back to the volunteer team dashboard
          this.router.navigate(['/']);
        });
    }
  }
  resetForm() {
    this.organization = null;
    // initialize form controls
    this.organizationName = new FormControl('');
    this.back();
  }
}
