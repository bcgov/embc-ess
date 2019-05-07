import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { combineLatest, Observable, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { AppState } from '../store';
import { AuthService } from '../core/services/auth.service';
import { VolunteerService } from '../core/services/volunteer.service';
import { OrganizationService } from '../core/services/organization.service';
import { User, Volunteer, Organization, ListResult } from '../core/models';
import { NotificationQueueService } from '../core/services/notification-queue.service';
import { UniqueKeyService } from '../core/services/unique-key.service';
// import { UpdateVolunteer } from '../store/volunteer/volunteer.actions';

@Component({
  selector: 'app-volunteer-maker',
  templateUrl: './volunteer-maker.component.html',
  styleUrls: ['./volunteer-maker.component.scss']
})
export class VolunteerMakerComponent implements OnInit, AfterViewInit {
  maker = true;
  editMode: boolean;
  submitting = false; // tracks if in the process of submitting for the UI
  currentUser: User;
  isLocalAuthority: boolean;
  isProvincialAdmin: boolean;
  isChangeOrg = false;

  // the approriate path for routing for the current user
  path: string;

  metaOrganizations: ListResult<Organization>;
  // currentVolunteer$ = this.store.select(s => s.volunteers.currentVolunteer); // TODO
  // currentOrganization$ = this.store.select(s => s.organizations.currentOrganization); // TODO
  volunteer: Volunteer;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private volunteerService: VolunteerService,
    private organizationService: OrganizationService,
    private authService: AuthService,
    private store: Store<AppState>,
    private notificationQueueService: NotificationQueueService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  // form value collectors
  organizationName: FormControl;
  lastName: FormControl;
  firstName: FormControl;
  bceid: FormControl;
  isAdministrator: FormControl;
  isPrimaryContact: FormControl;

  ngOnInit() {
    this.initVars();

    // get a bunch of variables we need
    // TODO: error handling if any observables fail
    combineLatest(
      this.authService.getCurrentUser(),
      this.authService.isLocalAuthority$,
      this.authService.isProvincialAdmin$,
      this.route.queryParamMap,
      this.authService.path
    ).subscribe(([currentUser, isLocalAuthority, isProvincialAdmin, queryParams, path]) => {
      this.currentUser = currentUser;
      this.isLocalAuthority = isLocalAuthority;
      this.isProvincialAdmin = isProvincialAdmin;
      this.path = path;

      // if local authority then get their organization
      if (this.isLocalAuthority) {
        const volunteerId = this.currentUser.contactid;
        // TODO: error handling if org not found
        this.getVolunteerOrganization(volunteerId)
          .subscribe(o => {
            // console.log('o =', o);
            this.volunteer.organization = o;
            this.organizationName.setValue(o); // set form field

            // continue
            this.ngOnInit2();
          });
      }

      // if provincial admin then check for orgId query param
      if (this.isProvincialAdmin) {
        const orgId = queryParams.get('orgId'); // may be null
        if (orgId) {
          // TODO: error handling if org not found
          this.organizationService.getOrganizationById(orgId)
            .subscribe(o => {
              // console.log('o =', o);
              this.volunteer.organization = o;
              this.organizationName.setValue(o); // set form field

              // continue
              this.ngOnInit2();
            });
        } else {
          // no id -- configure UI to select organization
          this.changeOrganization();

          // continue
          this.ngOnInit2();
        }
      }

      // should never get here
    }); // combineLatest
  }

  // async continuation of above
  private ngOnInit2() {
    // collect key from service for looking up the volunteer
    const key = this.uniqueKeyService.getKey();
    if (key) {
      // there may be a user to edit because the route looks right
      this.volunteerService.getVolunteerById(key)
        .subscribe(v => {
          // console.log('v =', v);
          this.editMode = true;
          this.volunteer = v;

          // set form fields
          this.organizationName.setValue(v.organization); // assume that a volunteer has an organization
          this.lastName.setValue(v.lastName);
          this.firstName.setValue(v.firstName);
          this.bceid.setValue(v.bceidAccountNumber);
          this.isAdministrator.setValue(v.isAdministrator);
          this.isPrimaryContact.setValue(v.isPrimaryContact);
        });
    } else {
      // this is a fresh form and will be a simple add user
      this.editMode = false;
      // NB: volunteer object was already initialized
      // NB: form fields don't need to be set
    }
  }

  ngAfterViewInit() {
    // focus the first input
    const elements = document.getElementsByTagName('input');
    if (elements.length > 0) {
      elements[0].focus();
    }
  }

  private initVars() {
    // initialize form controls
    this.organizationName = new FormControl('');
    this.lastName = new FormControl('');
    this.firstName = new FormControl('');
    this.bceid = new FormControl('');
    this.isAdministrator = new FormControl('');
    this.isPrimaryContact = new FormControl('');

    // initialize volunteer object
    // this will be overwritten if we get volunteer from API
    this.volunteer = {
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
      isPrimaryContact: null
    };
  }

  private getVolunteerOrganization(volunteerId: string): Observable<Organization> {
    if (!volunteerId) {
      return of(null as Organization);
    }

    // TODO: error handling if volunteer or organization not found
    return this.volunteerService
      .getVolunteerById(volunteerId)
      .pipe(
        switchMap(v => this.organizationService.getOrganizationById(v.organization.id))
      );
  }

  changeOrganization() {
    // get all organizations (sorted by name)
    // TODO: error handling if organizations not found
    this.organizationService.getOrganizations(null, null, null, '+name')
      .subscribe((v: ListResult<Organization>) => {
        // save the metaOrganizations
        this.metaOrganizations = v;
        this.isChangeOrg = true;
      });
  }

  next() {
    // first update volunteer object
    this.onSave();

    // only go to next page if all required fields are non null
    // NB: isPrimaryContact is required only if isAdministrator is true
    if (this.editMode
      && this.volunteer.lastName
      && this.volunteer.firstName
      && this.volunteer.bceidAccountNumber
      && (this.volunteer.isAdministrator === true || this.volunteer.isAdministrator === false)
      && (this.volunteer.isAdministrator === false || (this.volunteer.isPrimaryContact === true || this.volunteer.isPrimaryContact === false))) {
      this.maker = false;
      window.scrollTo(0, 0); // scroll to top

    } else if (!this.editMode
      && this.volunteer.organization
      && this.volunteer.lastName
      && this.volunteer.firstName
      && this.volunteer.bceidAccountNumber
      && (this.volunteer.isAdministrator === true || this.volunteer.isAdministrator === false)
      && (this.volunteer.isAdministrator === false || (this.volunteer.isPrimaryContact === true || this.volunteer.isPrimaryContact === false))) {
      this.maker = false;
      window.scrollTo(0, 0); // scroll to top

    } else {
      alert('All fields are required.');
    }
  }

  private onSave() {
    // stuff the data back into the volunteer object
    this.volunteer.organization = this.organizationName.value;
    this.volunteer.lastName = this.lastName.value;
    this.volunteer.firstName = this.firstName.value;
    this.volunteer.bceidAccountNumber = this.bceid.value;
    this.volunteer.isAdministrator = this.isAdministrator.value || false; // if null then set to false
    this.volunteer.isPrimaryContact = this.isPrimaryContact.value || false; // if null then set to false
    // console.log('volunteer =', this.volunteer);
  }

  back() {
    // show the editing parts of the form
    this.maker = true;
    window.scrollTo(0, 0); // scroll to top
  }

  submit(addAnother?: boolean) {
    // console.log('volunteer =', this.volunteer);
    this.submitting = true;
    // check if this is an update
    if (this.volunteer.id) {
      // if the volunteer has an ID we need to update
      this.volunteerService.updateVolunteer(this.volunteer)
        .subscribe(() => {
          this.submitting = false;
          // add a notification about the update
          this.notificationQueueService.addNotification('User updated successfuly');
          // if addAnother route then reset this form
          // else route back to the volunteers list
          if (addAnother) {
            this.resetForm();
          } else {
            // navigate back to the volunteers list
            this.router.navigate([`/${this.path}/volunteers`], { preserveQueryParams: true });
          }
        });
    } else {
      // if the volunteer has no id we need to create a new one
      this.volunteerService.createVolunteer(this.volunteer)
        .subscribe(v => {
          this.submitting = false;
          // add a notification about the creation
          this.notificationQueueService.addNotification('User added successfuly');
          // if addAnother route then reset this form
          // else route back to the volunteers list
          if (addAnother) {
            this.resetForm();
          } else {
            // TODO: preserveQueryParams is deprecated, use queryParamsHandling instead
            // navigate back to the volunteers list
            this.router.navigate([`/${this.path}/volunteers`], { preserveQueryParams: true });
          }
        });
    }
  }

  // FUTURE: why not just reload this page?
  resetForm() {
    // backup org, re-init vars, restore org
    const o = this.volunteer.organization;
    this.initVars();
    this.volunteer.organization = o;
    this.organizationName.setValue(o); // set form field
    // done editing a entry. Clear the reference key.
    this.uniqueKeyService.clearKey();
    // go back to the first page
    this.back();
  }

  cancel() {
    // TODO: this seems like bad practice but fix when we have time
    // navigate back to the volunteers list
    this.router.navigate([`/${this.path}/volunteers`], { preserveQueryParams: true });
  }
}
