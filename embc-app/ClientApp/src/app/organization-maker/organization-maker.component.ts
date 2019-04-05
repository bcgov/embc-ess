import { Component, OnInit } from '@angular/core';
import { Organization } from '../core/models';
import { ActivatedRoute, Router } from '@angular/router';
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
  maker: boolean;
  editMode: boolean;
  submitting = false; // tracks if in the process of submitting for the UI

  communities$ = this.store.select(s => s.lookups.communities.communities);
  organization: Organization;

  // form value collectors
  organizationName: FormControl;
  adminBceid: FormControl;
  adminLastName: FormControl;
  adminFirstName: FormControl;
  community: FormControl;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private organizationService: OrganizationService,
    private store: Store<AppState>,
  ) { }

  ngOnInit() {
    // initialize form for collection
    this.organizationName = new FormControl('');
    this.adminBceid = new FormControl('');
    this.adminLastName = new FormControl('');
    this.adminFirstName = new FormControl('');
    this.community = new FormControl('');

    if (this.route.snapshot.params.id) {
      // there may be an organization to edit because the route looks right
      this.organizationService.getOrganizationById(this.route.snapshot.params.id)
        .subscribe(o => {
          console.log('o =', o);
          this.editMode = true;
          this.maker = true;
          this.organization = o;
          this.organizationName.setValue(o.name);
          this.community.setValue(o.community);
        });
    } else {
      // this is a fresh form and will be a simple add organization
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
    }
  }

  next(): void {
    // only go next if all fields are non null
    if (this.editMode && this.organizationName.value && this.community.value) {
      this.maker = false;
      this.onSave();
    } else if (!this.editMode && this.organizationName.value && this.adminBceid.value && this.adminLastName.value && this.adminFirstName.value && this.community.value) {
      this.maker = false;
      this.onSave();
    } else {
      alert('All fields are required.');
    }
  }

  back() {
    // show the editing parts of the form
    this.maker = true;
  }

  onSave(): void {
    // stuff the data back into the organization object
    const organization: Organization = this.organization;
    organization.id = this.organization.id || null; // keep the id for updates
    // save content from the form
    organization.name = this.organizationName.value;
    organization.adminBCeID = this.adminBceid.value;
    organization.adminLastName = this.adminLastName.value;
    organization.adminFirstName = this.adminFirstName.value;
    organization.community = this.community.value;
    console.log('organization =', organization);
  }

  submit(addUsers?: boolean) {
    this.submitting = true;
    if (this.organization.id) {
      // if the organization has an ID we need to update
      this.organizationService.updateOrganization(this.organization)
        .subscribe(() => {
          this.submitting = false;
          // go back to the organization team dashboard
          this.router.navigate(['/']); // TODO: go somewhere that the application state can go
        });
    } else {
      // if the volunteer has no id we need to create a new one
      this.organizationService.createOrganization(this.organization)
        .subscribe(o => {
          this.submitting = false;
          // if addAnother route back to the add page else route back to the volunteer-team-editor
          // go back to the volunteer team dashboard
          this.router.navigate(['/']);
        });
    }
  }

  cancel() {
    // TODO: this seems like bad practive but fix when we have time
    // go back to the volunteer team dashboard
    this.editMode ? this.router.navigate(['../../organizations'], { relativeTo: this.route }) : this.router.navigate(['../organizations'], { relativeTo: this.route });
  }
}
