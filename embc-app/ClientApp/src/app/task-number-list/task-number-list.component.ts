import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ListResult, IncidentTask, PaginationSummary, Community } from '../core/models';
import { IncidentTaskService } from '../core/services/incident-task.service';
import { SearchQueryParameters } from '../shared/components/search';

@Component({
  selector: 'app-task-number-list',
  templateUrl: './task-number-list.component.html',
  styleUrls: ['./task-number-list.component.scss']
})
export class TaskNumberListComponent implements OnInit {
  // simple server response
  resultsAndPagination: ListResult<IncidentTask>;
  notFoundMessage = 'Searching ...';

  form: FormGroup;

  constructor(
    private incidentTaskService: IncidentTaskService,
    private router: Router,
    private route: ActivatedRoute,
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
}
