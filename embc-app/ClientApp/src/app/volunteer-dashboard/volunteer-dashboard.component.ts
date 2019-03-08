import { Component, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material';
import { RestService } from '../core/services/rest.service';
import { Evacuee } from '../core/models';
@Component({
  selector: 'app-volunteer-dashboard',
  templateUrl: './volunteer-dashboard.component.html',
  styleUrls: ['./volunteer-dashboard.component.scss']
})
export class VolunteerDashboardComponent implements OnInit {

  evacueeList: Evacuee[];
  constructor(
    private restService: RestService
  ) { }

  ngOnInit() {
  }

}
