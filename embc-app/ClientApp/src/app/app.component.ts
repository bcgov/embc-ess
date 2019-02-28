import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';

import { User } from './core/models';
import { detectIE10orLower } from './shared/utils/environmentUtils';
import { ControlledListService } from './core/services/controlled-list.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = '';
  isIE = false;
  currentUser: User;

  constructor(private lookups: ControlledListService) { }

  ngOnInit() {
    this.isIE = detectIE10orLower();
  }

  get versionInfo(): any {
    return null;
  }

  get isAuthenticated(): boolean {
    return this.currentUser != undefined;
  }

  get isNewUser(): boolean {
    return this.isAuthenticated && this.currentUser.isNewUser === true;
  }

  get isReturningUser(): boolean {
    return this.isAuthenticated && this.currentUser.isNewUser === false;
  }

  initializeApp() {
    // TODO: Load current user (if authenticated)

    // Get lookups.
    return this.getLookups();
  }

  getLookups() {
    return forkJoin([
      this.lookups.getAllSupportTypes(),
      this.lookups.getAllRegions(),
      this.lookups.getAllRegionalDistricts(),
      this.lookups.getAllCommunities(),
      this.lookups.getAllFamilyRelationshipTypes(),
      // ...add more
    ]);
  }
}
