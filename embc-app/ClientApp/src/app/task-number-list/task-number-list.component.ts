import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ListResult, IncidentTask, PaginationSummary, Community } from '../core/models';
import { IncidentTaskService } from '../core/services/incident-task.service';
import { SearchQueryParameters } from '../core/models/search-interfaces';
import { AuthService } from '../core/services/auth.service';
import { UniqueKeyService } from '../core/services/unique-key.service';

@Component({
  selector: 'app-task-number-list',
  templateUrl: './task-number-list.component.html',
  styleUrls: ['./task-number-list.component.scss']
})
export class TaskNumberListComponent implements OnInit {
  // simple server response
  resultsAndPagination: ListResult<IncidentTask>;
  notFoundMessage = 'Searching ...';
  path: string;

  form: FormGroup;

  constructor(
    private incidentTaskService: IncidentTaskService,
    private router: Router,
    private route: ActivatedRoute,
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
    this.authService.path.subscribe(p => this.path = p);
    this.initSearchForm();

    // collect all volunteers
    this.getIncidentTasks();
  }

  initSearchForm(): void {
    this.form = this.fb.group({ searchbox: null });
  }

  getIncidentTasks(params: SearchQueryParameters = {}) {
    this.incidentTaskService.getIncidentTasks(params).subscribe((v: ListResult<IncidentTask>) => {
      this.resultsAndPagination = v;
      this.notFoundMessage = 'No results found.';
    });
  }

  filter(community: Community) {
    // submit and collect search
    this.getIncidentTasks({ q: community ? community.id : '' });
  }
  modifyTaskNumber(id?: string) {
    if (id) {
      // save the unique ID for lookup in the new component
      this.uniqueKeyService.setKey(id);
    }
    // save the volunteer
    this.router.navigate([`/${this.path}/task-number`]);
  }
}
