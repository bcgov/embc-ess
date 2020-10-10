import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ListResult, IncidentTask, PaginationSummary, Community, OpenAndClosedTasksMetadata } from 'src/app/core/models';
import { IncidentTaskService } from 'src/app/core/services/incident-task.service';
import { IncidentTaskSearchQueryParameters } from 'src/app/core/models/search-interfaces';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';

@Component({
  selector: 'app-task-number-list',
  templateUrl: './task-number-list.component.html',
  styleUrls: ['./task-number-list.component.scss']
})
export class TaskNumberListComponent implements OnInit {
  // simple server response
  resultsAndPagination: ListResult<IncidentTask>;
  notFoundMessage = 'Searching ...';
  path: string = null; // the base path for routing

  defaultSearchQuery: IncidentTaskSearchQueryParameters = {
    offset: 0,
    limit: 20,
    activeTasks: true
  };
  // a place to save the last query parameters
  previousQuery: IncidentTaskSearchQueryParameters = { ...this.defaultSearchQuery };

  form: FormGroup;

  get showActiveTasks(): boolean {
    return this.previousQuery.activeTasks;
  }

  constructor(
    private incidentTaskService: IncidentTaskService,
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private fb: FormBuilder,
  ) { }

  // convenience getters
  get results(): IncidentTask[] {
    return this.resultsAndPagination !== undefined ? this.resultsAndPagination.data : [];
  }

  get pagination(): PaginationSummary {
    return this.resultsAndPagination !== undefined ? this.resultsAndPagination.metadata : null;
  }

  ngOnInit() {
    // collect the path for routing based on role
    this.authService.path.subscribe((path: string) => this.path = path);

    this.initSearchForm();

    // collect all incident tasks
    this.getIncidentTasks();
  }

  initSearchForm(): void {
    this.form = this.fb.group({ searchbox: null });
  }

  getIncidentTasks() {
    this.incidentTaskService.getIncidentTasks(this.previousQuery).subscribe(
      (listResult: ListResult<IncidentTask>) => {
        this.resultsAndPagination = listResult;
        this.notFoundMessage = 'No results found.';
      }, err => {
        console.log('error getting tasks =', err);
      }
    );
  }

  filter(community: Community) {
    // submit and collect search
    this.previousQuery.limit = this.defaultSearchQuery.limit;
    this.previousQuery.offset = this.defaultSearchQuery.offset;
    this.previousQuery.q = community ? community.id : '';
    this.getIncidentTasks();
  }

  modifyTaskNumber(taskId?: string) {
    // save task ID for lookup in the new component
    this.uniqueKeyService.setKey(taskId); // may be null

    // go to task number maker
    this.router.navigate([`/${this.path}/task-number`]);
  }

  toggleDisplayedTasks() {
    this.previousQuery.limit = this.defaultSearchQuery.limit;
    this.previousQuery.offset = this.defaultSearchQuery.offset;
    this.previousQuery.activeTasks = !this.previousQuery.activeTasks;
    this.getIncidentTasks();
  }

  onPaginationEvent(event: IncidentTaskSearchQueryParameters) {
    // submit and collect search
    this.previousQuery.limit = event.limit;
    this.previousQuery.offset = event.offset;
    this.getIncidentTasks();
  }
}
