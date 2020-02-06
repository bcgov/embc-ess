import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
// import { combineLatest, concat } from 'rxjs';
// import { map } from 'rxjs/operators';
// import { Store } from '@ngrx/store';
// import { AppState } from '../store';
import { ListResult, Organization, Community, PaginationSummary } from 'app/core/models';
import { OrganizationService } from 'app/core/services/organization.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { SearchQueryParameters } from 'app/core/models/search-interfaces';
import { AuthService } from 'app/core/services/auth.service';
import { UniqueKeyService } from 'app/core/services/unique-key.service';

// TODO: Un-comment code below when we are ready to aggregate all communities + regions in a single drop-down
// interface SearchFilter {
//   id: string;
//   name: string;
//   isRegion: boolean;
// }

@Component({
  selector: 'app-organization-list',
  templateUrl: './organization-list.component.html',
  styleUrls: ['./organization-list.component.scss']
})
export class OrganizationListComponent implements OnInit {

  // TODO: We need the full aggregated list of communities + districts + regions here
  // TODO: This is for V2
  // TODO: May need to move this to communities-select component
  // locations$ = combineLatest(
  //   this.store.select(s => s.lookups.communities.communities.map<SearchFilter>(x => ({ id: x.id, name: x.name, isRegion: false }))),
  //   this.store.select(s => s.lookups.regions.regions.map<SearchFilter>(x => ({ id: x.id, name: `${x.name} (Region)`, isRegion: true }))),
  // ).pipe(
  //   map(([communities, regions]) => communities.concat(regions))
  // );

  // simple server response
  resultsAndPagination: ListResult<Organization>;
  notFoundMessage = 'Searching ...'; // TODO: should use spinner instead
  path: string = null; // the base path for routing

  form: FormGroup;

  constructor(
    private organizationService: OrganizationService,
    private router: Router,
    // private store: Store<AppState>,
    private fb: FormBuilder,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  // convenience getters
  get results(): Organization[] {
    return this.resultsAndPagination ? this.resultsAndPagination.data : null;
  }

  get pagination(): PaginationSummary {
    return this.resultsAndPagination ? this.resultsAndPagination.metadata : null;
  }

  ngOnInit() {
    this.authService.path.subscribe((path: string) => this.path = path);
    this.form = this.fb.group({ searchbox: null });

    // collect all organizations
    this.getOrganizations();
  }

  // get organizations with supplied params defaults defined in
  getOrganizations({ limit, offset, q: query, sort }: SearchQueryParameters = {}) {
    this.organizationService.getOrganizations(limit, offset, query, sort)
      .subscribe((listResult: ListResult<Organization>) => {
        this.resultsAndPagination = listResult;
        this.notFoundMessage = 'No results found.';
      }, err => {
        console.log('error getting organizations =', err);
      });
  }

  filter(community: Community) {
    this.getOrganizations({ q: community ? community.id : null });
  }

  modifyOrganization(orgId?: string) {
    // go to organization maker
    if (orgId) {
      this.router.navigate([`/${this.path}/organization`, { orgId }]); // TODO: should use /:id instead?
    } else {
      this.router.navigate([`/${this.path}/organization`]);
    }
  }

  modifyOrganizationVolunteers(orgId: string) {
    // go to organization's list of volunteers
    this.router.navigate([`/${this.path}/organization/${orgId}/volunteers`]);
  }

}
