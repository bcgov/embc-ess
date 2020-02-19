import { Component, OnInit, Input, Output } from '@angular/core';
import { ListResult, EvacueeListItem } from 'src/app/core/models';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { EvacueeService } from 'src/app/core/services/evacuee.service';
import { normalizeDate } from "src/app/shared/utils/date-utils";

@Component({
  selector: 'app-evacuee-search-results',
  templateUrl: './evacuee-search-results.component.html',
  styleUrls: ['./evacuee-search-results.component.scss']
})
export class EvacueeSearchResultsComponent implements OnInit {
  path: string = null; // the base path for routing
  searchResults: ListResult<EvacueeListItem>;

  constructor( 
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private evacueeService: EvacueeService,
    ){ }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe((path: string) => this.path = path);
    // Get results of the previous search
    this.evacueeService.getPreviousQuery().subscribe((listResult: ListResult<EvacueeListItem>) => {
      // Normalize the dates of the results - fixes off by 1 day issue.
      listResult.data.forEach(item => {
        item.dob                        = normalizeDate(item.dob);
        item.registrationCompletionDate = normalizeDate(item.registrationCompletionDate);
        item.selfRegisteredDate         = normalizeDate(item.selfRegisteredDate);
      });
      this.searchResults = listResult;
    });
  }

  view(registrationId: string) {
    // save registration ID for lookup in the new component
    this.uniqueKeyService.setKey(registrationId);

    // go to registration summary page
    this.router.navigate([`/${this.path}/registration/summary`]);
  }

  searchAgain() {
    // Route back to dashboard
    this.router.navigate([`/${this.path}/registrations`]);
  }

}
