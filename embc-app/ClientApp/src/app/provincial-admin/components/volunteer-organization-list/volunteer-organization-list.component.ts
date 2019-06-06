import { Component, OnInit, OnDestroy, ViewChild, TemplateRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/core/services/auth.service';
import { VolunteerService, VolunteerSearchQueryParameters } from 'src/app/core/services/volunteer.service';
import { ListResult, Volunteer, PaginationSummary, Organization } from 'src/app/core/models';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { SearchQueryParameters } from 'src/app/core/models/search-interfaces';
import { OrganizationService } from 'src/app/core/services/organization.service';
import { NotificationQueueService } from 'src/app/core/services/notification-queue.service';

@Component({
  selector: 'app-volunteer-organization-list',
  templateUrl: './volunteer-organization-list.component.html',
  styleUrls: ['./volunteer-organization-list.component.scss']
})
export class VolunteerOrganizationListComponent implements OnInit, OnDestroy {

  @ViewChild('viewAlert') viewAlert: TemplateRef<any>;

  // simple server response
  resultsAndPagination: ListResult<Volunteer>;
  notFoundMessage = 'Searching ...';

  // the organization of all volunteers shown in this list
  currentOrganization: Organization;

  defaultSearchQuery: VolunteerSearchQueryParameters = {
    offset: 0,
    limit: 20,
  };

  // a place to save the last query parameters
  previousQuery: VolunteerSearchQueryParameters;

  // the contents of the searchbox
  queryString: string;

  // show types of users
  userType = 'SHOW_ALL';

  sort = ''; // how do we sort the list

  path: string = null; // the base path for routing

  // local constants used in the FORM
  readonly SHOW_ALL = '1';
  readonly SHOW_ADMINS_ONLY = '2';
  readonly SHOW_ESS_USERS_ONLY = '3';

  confirmModal: NgbModalRef = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private modals: NgbModal,
    private volunteerService: VolunteerService,
    private organizationService: OrganizationService,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private notificationQueueService: NotificationQueueService,
  ) { }

  // convenience getters
  get results(): Volunteer[] {
    // if there are non null results in the global variable to contain the volunteers return the collection of data otherwise return null
    return this.resultsAndPagination ? this.resultsAndPagination.data : null;
  }

  get pagination(): PaginationSummary {
    // if there are non null results in the global pagination summary to contain the results and pagination return the metadata otherwise return null
    return this.resultsAndPagination ? this.resultsAndPagination.metadata : null;
  }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe((path: string) => this.path = path);

    this.previousQuery = this.copyProperties(this.defaultSearchQuery);

    // collect all volunteers
    this.getVolunteers();

    // get route parameter
    const orgId = this.route.snapshot.paramMap.get('orgId'); // should never be null

    if (orgId) {
      this.organizationService.getOrganizationById(orgId)
        .subscribe((organization: Organization) => {
          // save the organization
          this.currentOrganization = organization;

          // collect all volunteers
          this.getVolunteers();
        }, err => {
          this.notificationQueueService.addNotification('Failed to load organization', 'danger');
          console.log('error getting organization =', err);

          // go back to list of organizations
          this.router.navigate([`/${this.path}/organizations`]);
        });
    } else {
      // TODO: when the user gets kicked out of the organization for making an edit redirect them back to the place they can make a decision
      this.router.navigate([`/${this.path}/organizations`]);
    }
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.confirmModal) { this.confirmModal.dismiss(); }
  }

  private copyProperties(obj: {}): {} {
    const fresh = {};
    for (const k in obj) {
      if (k) { fresh[k] = obj[k]; }
    }
    return fresh;
  }

  // get volunteers with supplied params defaults defined in
  private getVolunteers() {
    // the query is the value in the searchbox
    this.previousQuery.q = this.queryString;

    // the organization is the one in the global state
    this.previousQuery.org_id = this.route.snapshot.paramMap.get('orgId');

    // pagination is calculated
    this.previousQuery.offset = this.previousQuery.offset;

    // how many records we want
    this.previousQuery.limit = this.previousQuery.limit;

    // set parameter flags according to what is checked.
    switch (this.userType) {
      case 'SHOW_ADMINS_ONLY':
        this.previousQuery.admin_only = true;
        break;
      case 'SHOW_ESS_USERS_ONLY':
        this.previousQuery.ess_only = true;
        break;
      case 'SHOW_ALL':
        break;
      default:
        break;
    }

    // go get the collection of meta and data
    // get the volunteers using the parameters supplied
    this.volunteerService.getVolunteers(this.previousQuery)
      .subscribe((listResult: ListResult<Volunteer>) => {
        // save the result of the service into an object with both the result and service
        this.resultsAndPagination = listResult;

        // set the not found result message
        // it should be hidden when results flow into the form
        this.notFoundMessage = 'No results found.';
      }, err => {
        this.notificationQueueService.addNotification('Failed to load volunteers', 'danger');
        console.log('error getting volunteers =', err);

        // go back to list of organizations
        this.router.navigate([`/${this.path}/organizations`]);
      });
  }

  onPaginationEvent(event: SearchQueryParameters) {
    // submit and collect search
    this.previousQuery.limit = event.limit;
    this.previousQuery.offset = event.offset;
    this.getVolunteers();
  }

  search() {
    // on search return to page 1
    this.getVolunteers();
  }

  modifyOrganizationVolunteer(volunteerId?: string) {
    if (!volunteerId) {
      // no id means 'add user' -> clear unique key
      this.uniqueKeyService.clearKey();
      this.router.navigate([`/${this.path}/volunteer`, { orgId: this.previousQuery.org_id }]);
      return;
    }

    this.confirmModal = this.modals.open(this.viewAlert, { centered: true, windowClass: 'modal-small' });

    // handle result
    this.confirmModal.result.then(
      () => {
        // modal was closed
        this.confirmModal = null;

        // save volunteer ID for lookup in the new component
        this.uniqueKeyService.setKey(volunteerId);

        // go to vounteer maker
        this.router.navigate([`/${this.path}/volunteer`, { orgId: this.previousQuery.org_id }]);
      },
      () => {
        // modal was dismissed
        this.confirmModal = null;
      }
    );
  }

}
