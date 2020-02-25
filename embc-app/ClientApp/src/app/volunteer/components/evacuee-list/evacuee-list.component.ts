import { Component, OnInit } from '@angular/core';
import { ListResult, EvacueeListItem } from 'src/app/core/models';
import { Observable, from } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { EvacueeSearchQueryParameters } from 'src/app/core/models/search-interfaces';
import { EvacueeService } from 'src/app/core/services/evacuee.service';
import { FormBuilder } from '@angular/forms';
import {dateStringIsValid} from 'src/app/shared/utils/date-utils';
import * as moment from 'moment';


@Component({
  selector: 'app-evacuee-list',
  templateUrl: './evacuee-list.component.html',
  styleUrls: ['./evacuee-list.component.scss']
})
export class EvacueeListComponent implements OnInit {

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
  readonly dateMask = [/\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/]; // yyyy-mm-dd
  dobString: string = null;

  searchForm = this.fb.group({
    last_name: null,
    first_name: null,
    dob: null,
    task_no: null,
    ess_file_no: null,
    evacuated_from: null,
    evacuated_to: null,
    registration_completed: null,
    referrals_provided: null,
  });

  formValid = {
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
    private fb: FormBuilder,
  ) { }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe((path: string) => this.path = path);
  }

  getEvacuees(query: EvacueeSearchQueryParameters = this.defaultSearchQuery): Observable<ListResult<EvacueeListItem>> {
    // save the generic query for repeat searches
    this.previousQuery = { ...query };
    // save the organization id into the query from the default
    return this.evacueeService.getEvacuees(query);
  }

  doBasicSearch() {
    const dob = this.searchForm.get('dob').value;
    const fName = this.searchForm.get('first_name').value;
    const lName = this.searchForm.get('last_name').value;
    // Ensure required fields are not null or empty strings
    this.dobIsValid(dob);
    this.formValid.hasFirstName = fName != null && fName !== '';
    this.formValid.hasLastName = lName != null && lName !== '';

    if (this.formValid.hasDob && this.formValid.hasValidDobFormat && this.formValid.hasFirstName && this.formValid.hasLastName) {
      this.search();
    }
    else {
      this.notFoundMessage = 'Please fill out all fields.';
    }
  }

  private dobIsValid(dob: string): boolean {
    let result: boolean;
    // Check if dob has anything
    result = dob != null && dob !== '';
    this.formValid.hasDob = result;
    // if dob has a value, check it is a valid date
    if (result) {
      result = dateStringIsValid(dob);
      this.formValid.hasValidDobFormat = result;
    }

    return result;
  }

  updateDob(dob: string): void {
    const m = moment(dob, 'YYYY-MM-DD', true);

    if (m.isValid()) {
      // update dob
      this.searchForm.patchValue(
        { "dob": dob }
      );
    } else {
      // error message
      this.dobString = null;
    }
  }

  essSearch() {
    const essNum = this.searchForm.get("ess_file_no").value;
    this.formValid.hasESSNumber = essNum;
    if (this.formValid.hasESSNumber) {
      this.search();
    }
    else {
      this.notFoundMessage = 'Please fill out all fields.'
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
    // Navigate to results
    this.router.navigate([`/${this.path}/evacuee/results`]);

  }

  createSearchQuery(): EvacueeSearchQueryParameters {
    // store the sorting and pagination
    let query = { ...this.defaultSearchQuery, sort: this.sort };
    const form = this.searchForm.value;
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

    return query;
  }

}
