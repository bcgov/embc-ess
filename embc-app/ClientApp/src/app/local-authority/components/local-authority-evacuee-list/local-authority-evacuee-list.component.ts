import { Component, OnInit } from '@angular/core';
import { ListResult, EvacueeListItem } from 'src/app/core/models';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { EvacueeSearchQueryParameters } from 'src/app/core/models/search-interfaces';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { EvacueeService } from 'src/app/core/services/evacuee.service';
import { FormBuilder } from '@angular/forms';
import { EVERYONE, VOLUNTEER, LOCAL_AUTHORITY, PROVINCIAL_ADMIN } from 'src/app/constants';
import * as moment from 'moment';
import { dateStringIsValid } from 'src/app/shared/utils/date-utils';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-local-authority-evacuee-list',
  templateUrl: './local-authority-evacuee-list.component.html',
  styleUrls: ['./local-authority-evacuee-list.component.scss']
})
export class LocalAuthorityEvacueeListComponent implements OnInit {
  evacuees: any;
  // server response
  resultsAndPagination: ListResult<EvacueeListItem>;
  notFoundMessage: string = "";
  defaultSearchQuery: EvacueeSearchQueryParameters = {
    offset: 0,
    limit: 20
  };
  queryString: string;
  // a place to save the last query parameters
  previousQuery: EvacueeSearchQueryParameters = {};
  sort = '-registrationId'; // how do we sort the list query param
  path: string = null; // the base path for routing
  isAdmin = false; // flag that controls whether to display the Superuser or Admin text
  exportValidationError = false;
  readonly dateMask = [/\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/]; // yyyy-mm-dd
  dobString: string = null;
  // for R1, advanced search mode is the only mode
  advancedSearchMode = false;
  advancedSearchForm = this.fb.group({
    last_name: null,
    first_name: null,
    dob: null,
    task_no: null,
    ess_file_no: null,
    evacuated_from: null,
    evacuated_to: null,
    registration_completed: null,
    referrals_provided: null,
    self_reg_date_from: null,
    self_reg_date_to: null,
    finalization_date_to: null,
    finalization_date_from: null,
  });

  advancedSearchValid = {
    hasLastName: true,
    hasFirstName: true,
    hasDob: true,
    hasValidDobFormat: true,
    hasESSNumber: true,
  }

  constructor(
    private evacueeService: EvacueeService,
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private fb: FormBuilder,
  ) { }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe((path: string) => this.path = path);
    this.getEvacuees().subscribe((listResult: ListResult<EvacueeListItem>) => {
      this.resultsAndPagination = listResult;
    });
    // Determine if user is Superuser or Admin
    this.authService.role.subscribe((role: string) => {
      // Only Superusers and Admins can be routed here, so only need to check the one role
      this.isAdmin = role === PROVINCIAL_ADMIN;
    });
  }

  switchToAdvancedSearch() {
    this.advancedSearchMode = true;
    // when you switch back reset the search
    this.search();
  }

  switchToBasicSearch() {
    this.advancedSearchMode = false;
    // when you switch back reset the search
    this.search();
  }

  getEvacuees(query: EvacueeSearchQueryParameters = this.defaultSearchQuery): Observable<ListResult<EvacueeListItem>> {
    // save the generic query for repeat searches
    this.previousQuery = { ...query };
    // save the organization id into the query from the default
    return this.evacueeService.getEvacuees(query);
  }

  essSearch() {
    const essNum = this.advancedSearchForm.get("ess_file_no").value;
    this.advancedSearchValid.hasESSNumber = essNum;
    if (this.advancedSearchValid.hasESSNumber) {
      this.search();
    }
    else {
      this.notFoundMessage = 'Please fill out all fields.'
    }
  }

  basicSearch() {
    const dob = this.advancedSearchForm.get('dob').value;
    const fName = this.advancedSearchForm.get('first_name').value;
    const lName = this.advancedSearchForm.get('last_name').value;
    // Ensure required fields are not null or empty strings
    //this.advancedSearchValid.hasDob = dob != null && dob !== '';
    this.dobIsValid(dob);
    this.advancedSearchValid.hasFirstName = fName != null && fName !== '';
    this.advancedSearchValid.hasLastName = lName != null && lName !== '';

    if (this.advancedSearchValid.hasDob && this.advancedSearchValid.hasValidDobFormat && this.advancedSearchValid.hasFirstName && this.advancedSearchValid.hasLastName) {
      this.search();
    }
    else {
      this.notFoundMessage = 'Please fill out all fields.';
    }
  }

  advancedSearch() {
    // Do any necessary pre-search work

    this.search();
  }

  private dobIsValid(dob: string): boolean {
    let result: boolean;
    // Check if dob has anything
    result = dob != null && dob !== '';
    this.advancedSearchValid.hasDob = result;
    // if dob has a value, check it is a valid date
    if (result) {
      result = dateStringIsValid(dob);
      this.advancedSearchValid.hasValidDobFormat = result;
    }

    return result;
  }

  updateDob(dob: string): void {
    const m = moment(dob, 'YYYY-MM-DD', true);

    if (m.isValid()) {
      // update dob
      this.advancedSearchForm.patchValue(
        { "dob": dob }
      );
    } else {
      // error message
      this.dobString = null;
    }
  }

  search() {
    // submit and collect search with a query string
    const query = this.createSearchQuery();

    this.getEvacuees(query).subscribe((listResult: ListResult<EvacueeListItem>) => {
      if (listResult.data.length <= 0) {
        this.notFoundMessage = 'No results found.';
      } else {
        this.notFoundMessage = 'Searching ...';
      }
      this.resultsAndPagination = listResult;
    });
  }

  createSearchQuery(): EvacueeSearchQueryParameters {
    // store the sorting and pagination
    let query = { ...this.defaultSearchQuery, sort: this.sort };
    if (this.advancedSearchMode) {
      const form = this.advancedSearchForm.value;

      // the community auto-complete returns an object. we only want the ID (string)
      const fromCommunity = form.evacuated_from ? form.evacuated_from.name : null;
      const toCommunity = form.evacuated_to ? form.evacuated_to.name : null;

      query = {
        ...query,
        ...form,
        evacuated_from: fromCommunity,
        evacuated_to: toCommunity
      };

      // delete unneeded values
      delete query.q;
      if (query.registration_completed === null) {
        delete query.registration_completed;
      }
      if (query.referrals_provided === null) {
        delete query.referrals_provided;
      }
    } else {
      // basic search
      query.q = this.queryString;
    }
    return query;
  }

  onPaginationEvent(event: EvacueeSearchQueryParameters) {
    // save the pagination into the previous query and execute the query again
    this.previousQuery.limit = event.limit;
    this.previousQuery.offset = event.offset;
    this.getEvacuees(this.previousQuery).subscribe((listResult: ListResult<EvacueeListItem>) => {
      this.resultsAndPagination = listResult;
    });
  }

  edit(registrationId: string) {
    // save registration ID for lookup in the new component
    this.uniqueKeyService.setKey(registrationId);

    // go to registration maker
    this.router.navigate([`/${this.path}/registration`]);
  }

  view(registrationId: string) {
    // save registration ID for lookup in the new component
    this.uniqueKeyService.setKey(registrationId);

    // go to registration summary page
    this.router.navigate([`/${this.path}/registration/summary`]);
  }

  onNullQueryString() {
    // when a user in IE11 clicks the x to clear the field we need to be sure that we reset the search
    if (!this.queryString) {
      this.search();
    }
  }


  onExportEvacuee() {
    this.exportValidationError = false;
    const query = this.createSearchQuery();
    // superusers need to enter a task number or evacuated from
    this.exportValidationError = !this.isAdmin && query.task_no == null && query.evacuated_to == null; // to is from
    
    if (!this.exportValidationError) {
      this.evacueeService.getEvacueesCSV(query).subscribe((data: { blob: Blob, fileName: string }) => {
        saveAs(data.blob, data.fileName);
      });
    }

  }

  onExportReferrals() {
    this.exportValidationError = false;
    const query = this.createSearchQuery();
    // superusers need to enter a task number or evacuated from
    this.exportValidationError = !this.isAdmin && query.task_no == null && query.evacuated_to == null; // to is from
    
    if (!this.exportValidationError) {
      this.evacueeService.getEvacueeReferralCSV(query).subscribe((data: { blob: Blob, fileName: string }) => {
        saveAs(data.blob, data.fileName);
      });
    }
  }
}
