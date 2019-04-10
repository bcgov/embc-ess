import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Store } from '@ngrx/store';

import { AppState } from '../store';
import { ListResult, IncidentTask, PaginationSummary, Community } from '../core/models';
import { IncidentTaskService } from '../core/services/incident-task.service';
import { SearchQueryParameters } from '../shared/components/search';

@Component({
  selector: 'app-task-number-list',
  templateUrl: './task-number-list.component.html',
  styleUrls: ['./task-number-list.component.scss']
})
export class TaskNumberListComponent implements OnInit {

  communities$ = this.store.select(s => s.lookups.communities.communities);

  // simple server response
  metaIncidentTasks: ListResult<IncidentTask>;
  notFoundMessage: string = '';

  // collection of pagination parameters for UI pagination
  // display and pagination
  increments: number[] = [5, 10, 25, 50, 100, 1000];
  // doesn't need to be an object besides it provides a visual seper
  page: number; // the current displayed page
  totalPages: number; // how many pages are returned?
  pageSize: number; // how many entries are on the page
  previousQuery: string; // a place to save the last query parameters
  sort: string = ''; // how do we sort the list
  collectionSize: number = 0; // how large is the collection?
  maxSize = 100; // how many results should the UI show?
  boundaryLinks = true; // do we show the jump to first and last page links?
  resultsAndPagination: ListResult<IncidentTask>;
  notFoundMessage = 'Searching ...';

  form: FormGroup;

  constructor(
    private incidentTaskService: IncidentTaskService,
    private router: Router,
    private route: ActivatedRoute,
    private store: Store<AppState>,
    private fb: FormBuilder,
  ) { }

  // convenience getters
  get results(): IncidentTask[] {
    return this.resultsAndPagination ? this.resultsAndPagination.data : null;
  }

  get pagination(): PaginationSummary {
    return this.resultsAndPagination ? this.resultsAndPagination.metadata : null;
  }

  ngOnInit() {
    this.initSearchForm();

    // collect all volunteers
    this.getIncidentTasks();
  }

  initSearchForm(): void {
    this.form = this.fb.group({ searchbox: null });
  }

  getIncidentTasks(limit?: number, offset?: number, query?: string, sort?: string) {
    // get volunteers with supplied params defaults defined in
    this.incidentTaskService.getIncidentTasks().subscribe((v: ListResult<IncidentTask>) => {
      // save the metaVolunteers
      this.metaIncidentTasks = v;
      // collect all of the meta into variables
      this.page = v.metadata.page;
      this.totalPages = v.metadata.totalPages;
      this.collectionSize = v.metadata.totalCount;
      this.maxSize = v.metadata.pageSize;
      // alert(v.metadata.pageSize)
      //save the last query performed
      this.previousQuery = query || '';
    });
  }

  filter(community: Community) {
    // submit and collect search
    this.getIncidentTasks({ q: community ? community.id : '' });
  }
}
