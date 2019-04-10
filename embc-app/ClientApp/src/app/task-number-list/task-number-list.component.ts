import { Component, OnInit } from '@angular/core';
import { ListResult, IncidentTask } from '../core/models';
import { IncidentTaskService } from '../core/services/incident-task.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-task-number-list',
  templateUrl: './task-number-list.component.html',
  styleUrls: ['./task-number-list.component.scss']
})
export class TaskNumberListComponent implements OnInit {

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
  maxSize = 5; // how many results should the UI show?
  boundaryLinks = true; // do we show the jump to first and last page links?

  constructor(
    private incidentTaskService: IncidentTaskService,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    // collect all volunteers
    this.getIncidentTasks();
  }

  routeTo(id: string) {
    // TODO: this seems like bad practice but fix when we have time
    this.router.navigate(['../task-number/' + id], { relativeTo: this.route });
  }

  getIncidentTasks(limit?: number, offset?: number, query?: string, sort?: string) {
    // get volunteers with supplied params defaults defined in
    this.incidentTaskService.getIncidentTasks().subscribe((v: ListResult<IncidentTask>) => {
      // save the metaVolunteers
      this.metaIncidentTasks = v;
    });
  }

  search(searchTerm: string) {
    // submit and collect search
    this.getIncidentTasks(null, null, searchTerm);
  }
}
