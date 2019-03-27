import { Component, OnInit } from '@angular/core';
import { IncidentTaskService } from '../core/services/incident-task.service';
import { MetaIncidentTask } from '../core/models/meta-incident-task';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  incidentTasks: MetaIncidentTask;

  constructor(
    private incidentTaskService: IncidentTaskService
  ) { }

  ngOnInit() {
    this.incidentTaskService.getIncidentTasks().subscribe(i => this.incidentTasks = i);
  }

}
