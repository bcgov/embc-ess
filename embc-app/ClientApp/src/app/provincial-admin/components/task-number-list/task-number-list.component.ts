import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ListResult, IncidentTask, PaginationSummary, Community } from 'src/app/core/models';
import { IncidentTaskService } from 'src/app/core/services/incident-task.service';
import { SearchQueryParameters } from 'src/app/core/models/search-interfaces';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { OpenAndClosedTasksMetadata } from 'src/app/core/models/open-and-closed-tasks-metaData.model';
import * as moment from 'moment';

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
  // default to showing active tasks
  showActiveTasks = true; 
  // private variable holding all tasks so we can filter active/inactive on the client
  private allTasks: ListResult<IncidentTask>; 
  private taskArray: IncidentTask[] = [];
  private openTasksPagination: PaginationSummary;
  private closedTasksPagination: PaginationSummary;
  
  constructor(
    private incidentTaskService: IncidentTaskService,
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    private fb: FormBuilder,
  ) { }

  // convenience getters
  get results(): IncidentTask[] {
    return this.taskArray;
  }

  
  get pagination(): PaginationSummary {
    return this.showActiveTasks ? this.openTasksPagination : this.closedTasksPagination;
  }

  ngOnInit() {
    // collect the path for routing based on role
    this.authService.path.subscribe((path: string) => this.path = path);

    this.initSearchForm();

    // collect all incident tasks
    this.getIncidentTasks();
    this.getMetaData();
  }

  initSearchForm(): void {
    this.form = this.fb.group({ searchbox: null });
  }

  getIncidentTasks(params: SearchQueryParameters = {}) {
    this.incidentTaskService.getIncidentTasks(params)
      .subscribe((listResult: ListResult<IncidentTask>) => {
        this.allTasks = listResult;
        this.resultsAndPagination = this.allTasks;
        this.taskArray = listResult.data;
        
        this.filterTasks();
        this.notFoundMessage = 'No results found.';
      }, err => {
        console.log('error getting tasks =', err);
      });
  }

  private getMetaData() {
    this.incidentTaskService.getOpenAndClosedIncidentTaskMetadata()
      .subscribe((pageData: OpenAndClosedTasksMetadata) => {
        this.openTasksPagination = pageData.openTasks;
        this.closedTasksPagination = pageData.closedTasks;
      })
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

  toggleDisplayedTasks() {
    this.showActiveTasks = !this.showActiveTasks;
    this.filterTasks();
  }

  // Filters out active/inactive tasks
  private filterTasks() {
    this.taskArray = [];
    let now = moment.utc();
    if (this.showActiveTasks) {
      this.taskArray = this.allTasks.data.filter(task => now.isBefore(moment.utc(task.taskNumberEndDate)))
    }
    else {
      this.taskArray = this.allTasks.data.filter(task => now.isAfter(moment.utc(task.taskNumberEndDate)))
    }
  }

}
