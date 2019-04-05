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
    // TODO: this seems like bad practive but fix when we have time
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
