import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ListResult, IncidentTask, PaginationSummary, Community } from 'src/app/core/models';
import { IncidentTaskService } from 'src/app/core/services/incident-task.service';
import { SearchQueryParameters } from 'src/app/core/models/search-interfaces';
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

  form: FormGroup;

  constructor(
    private incidentTaskService: IncidentTaskService,
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
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
    // collect the path for routing based on role
    this.authService.path.subscribe((path: string) => this.path = path);

    this.initSearchForm();

    // collect all incident tasks
    this.getIncidentTasks();
  }

  initSearchForm(): void {
    this.form = this.fb.group({ searchbox: null });
  }

  getIncidentTasks(params: SearchQueryParameters = {}) {
    this.incidentTaskService.getOpenIncidentTasks(params)
      .subscribe((listResult: ListResult<IncidentTask>) => {
        this.resultsAndPagination = listResult;
        this.notFoundMessage = 'No results found.';
      }, err => {
        console.log('error getting tasks =', err);
      });
  }

  filter(community: Community) {
    // submit and collect search
    this.getIncidentTasks({ q: community ? community.id : '' });
  }

  modifyTaskNumber(taskId?: string) {
    // save task ID for lookup in the new component
    this.uniqueKeyService.setKey(taskId); // may be null

    // go to task number maker
    this.router.navigate([`/${this.path}/task-number`]);
  }
}
