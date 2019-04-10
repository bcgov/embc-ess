import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { combineLatest, of, Observable } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';

import { AuthService } from '../core/services/auth.service';
import { OrganizationService } from '../core/services/organization.service';
import { VolunteerService, VolunteerSearchQueryParameters } from '../core/services/volunteer.service';
import { ListResult, Volunteer, PaginationSummary, User, Organization } from '../core/models';

@Component({
  selector: 'app-volunteer-list',
  templateUrl: './volunteer-list.component.html',
  styleUrls: ['./volunteer-list.component.scss']
})
export class VolunteerListComponent implements OnInit {
  // simple server response
  resultsAndPagination: ListResult<Volunteer>;
  notFoundMessage = 'Searching ...';

  // who's accessing this list component
  currentUser: User;
  isLocalAuthority: boolean;
  isProvincialAdmin: boolean;

  // the parent organization of all volunteers shown in this list
  currentOrganization: Organization | null = null;

  // local constants used in the FORM
  readonly SHOW_ALL = 1;
  readonly SHOW_ADMINS_ONLY = 2;
  readonly SHOW_ESS_USERS_ONLY = 3;

  // the search form and associated toggles (show all, show only admins, show only regular users)
  form: FormGroup;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private volunteers: VolunteerService,
    private organizations: OrganizationService,
    private auth: AuthService,
  ) { }

  // convenience getters
  get results(): Volunteer[] {
    return this.resultsAndPagination ? this.resultsAndPagination.data : null;
  }

  get pagination(): PaginationSummary {
    return this.resultsAndPagination ? this.resultsAndPagination.metadata : null;
  }

  ngOnInit() {
    this.initSearchForm();

    // collect all volunteers
    this.initVars()
      .subscribe(org => this.getVolunteers({ org_id: org ? org.id : null }));
  }

  initSearchForm(): void {
    this.form = this.fb.group({
      searchbox: null,
      userToggle: this.SHOW_ALL,
    });
  }

  initVars(): Observable<Organization> {
    // combine everything we need to display this form into one Observable
    const combined$ = combineLatest(
      this.auth.getCurrentUser(),
      this.auth.isLocalAuthority$,
      this.auth.isProvincialAdmin$,
      this.route.queryParamMap,
    );

    // subscribe to the single observable, giving us all the data we need
    return combined$
      .pipe(
        tap(([user, isLocalAuthority, isProvincialAdmin]) => {
          this.currentUser = user;
          this.isLocalAuthority = isLocalAuthority;
          this.isProvincialAdmin = isProvincialAdmin;
        }),
        switchMap(([user, isLocalAuthority, isProvincialAdmin, queryParams]) => {
          // get the organization matching the query parameter passed in (if any)
          if (isProvincialAdmin) {
            const orgId = queryParams.get('orgId');
            return this.getAnyOrganization(orgId);
          }
          // for local authorities, we know what org they belong to
          if (isLocalAuthority) {
            const volunteerId = user.contactid;
            return this.getVolunteerOrganization(volunteerId);
          }
        }),
        tap(org => this.currentOrganization = org)
      );
  }

  getAnyOrganization(orgId: string): Observable<Organization> {
    if (!orgId) {
      return of(null as Organization);
    }
    return this.organizations.getOrganizationById(orgId);
  }

  getVolunteerOrganization(volunteerId: string): Observable<Organization> {
    if (!volunteerId) {
      return of(null as Organization);
    }
    return this.volunteers
      .getVolunteerById(volunteerId)
      .pipe(switchMap(v => this.organizations.getOrganizationById(v.organization.id)));
  }

  // get volunteers with supplied params defaults defined in
  getVolunteers(params: VolunteerSearchQueryParameters = {}) {
    this.volunteers.getVolunteers(params)
      .subscribe((v: ListResult<Volunteer>) => {
        this.resultsAndPagination = v;
        this.notFoundMessage = 'No results found.';
      });
  }

  // submit and collect search
  search() {
    if (!this.currentOrganization || !this.currentOrganization.id) {
      return;
    }
    this.getVolunteers(this.createQueryParams());
  }

  createQueryParams(): VolunteerSearchQueryParameters {
    const essOnly = this.form.value.userToggle == this.SHOW_ESS_USERS_ONLY;
    const adminOnly = this.form.value.userToggle == this.SHOW_ADMINS_ONLY;
    return {
      q: this.form.value.searchbox,
      org_id: this.currentOrganization.id,
      ess_only: essOnly,
      admin_only: adminOnly,
    };
  }
}
