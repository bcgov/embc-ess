import { Component, OnInit } from '@angular/core';
import { IncidentTask, ListResult } from 'src/app/core/models';
import { IncidentTaskService } from 'src/app/core/services/incident-task.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-admin-task-numbers',
  templateUrl: './admin-task-numbers.component.html',
  styleUrls: ['./admin-task-numbers.component.scss']
})
export class AdminTaskNumbersComponent implements OnInit {

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
    this.router.navigate(['../task-number-edit/fill/' + id], { relativeTo: this.route });
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
