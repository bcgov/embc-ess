import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { MetaRegistration } from '../core/models/meta-registration';
import { EvacueeSearchResults, SearchQueryParameters } from '../shared/components/search';
import { RegistrationService } from '../core/services/registration.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  constructor() { }
  ngOnInit() { }
}
