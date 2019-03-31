import { Component, OnInit } from '@angular/core';
import { concat } from 'rxjs';

import { User } from './core/models';
import { detectIE10orLower } from './shared/utils';
import { ControlledListService } from './core/services/controlled-list.service';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = '';
  isIE = false;
  currentUser: User;

  constructor(
    private lookups: ControlledListService,
    private authService: AuthService,

  ) { }

  ngOnInit() {
    this.isIE = detectIE10orLower();
    this.initializeApp();
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
    // subscribe to changes to the current user; i.e. when a user "logs in"
    this.authService.currentUser$.subscribe(user => this.currentUser = user);

    // Check for authenticated users
    this.reloadUser();

    // Loaded once at init time, as they do not change very often, and
    // certainly not within the app.
    this.getLookups().subscribe();
  }

  reloadUser() {
    this.authService.login();
  }


  getLookups() {
    return concat(
      this.lookups.getAllCountries(),
      this.lookups.getAllRegions(),
      this.lookups.getAllRegionalDistricts(),
      this.lookups.getAllCommunities(),
      this.lookups.getAllFamilyRelationshipTypes(),
      // ...add more
    );
  }
}
