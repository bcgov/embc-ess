import { Component, OnInit } from '@angular/core';
import { ListResult, Organization } from '../core/models';
import { Router, ActivatedRoute } from '@angular/router';
import { OrganizationService } from '../core/services/organization.service';

@Component({
  selector: 'app-organization-list',
  templateUrl: './organization-list.component.html',
  styleUrls: ['./organization-list.component.scss']
})
export class OrganizationListComponent implements OnInit {
  // simple server response
  metaOrganizations: ListResult<Organization>;
  notFoundMessage: string = '';
  constructor(
    private organizationService: OrganizationService,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    // collect all organizations
    this.getOrganizations();
  }

  routeTo(bceidAccountNumber: string) {
    // TODO: this seems like bad practive but fix when we have time
    this.router.navigate(['../organization/' + bceidAccountNumber], { relativeTo: this.route });
  }

  getOrganizations(limit?: number, offset?: number, query?: string, sort?: string) {
    // get organizations with supplied params defaults defined in
    this.organizationService.getOrganizations(limit, offset, query, sort).subscribe((v: ListResult<Organization>) => {
      // save the metaOrganizations
      this.metaOrganizations = v;
    });
  }

  search(searchTerm: string) {
    // submit and collect search
    this.getOrganizations(null, null, searchTerm);
  }
}
