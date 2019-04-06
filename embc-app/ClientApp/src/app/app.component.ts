import { Component, OnInit, HostListener } from '@angular/core';
import { concat } from 'rxjs';

import { User } from './core/models';
import { detectIE10orLower } from './shared/utils';
import { ControlledListService } from './core/services/controlled-list.service';
import { AuthService } from './core/services/auth.service';
import { CookieService } from './core/services/cookie.service';

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
    private cookies: CookieService,
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
    // Check for authenticated users
    this.reloadUser();

    // Loaded once at init time, as they do not change very often, and
    // certainly not within the app.
    this.getLookups().subscribe();
  }

  // TODO: Let's do a shotgun blast here, will improve later...
  @HostListener('window:beforeunload', ['$event'])
  onWindowClose(event: any): void {
    this.logout(event);
  }

  logout(event?: any): void {
    // Remove all saved data from sessionStorage
    sessionStorage.clear();

    // clear all cookies
    this.cookies.clear();

    // let's also try to get to the logout endpoint on the server...
    this.authService.logout(true)
      .subscribe(() => console.log('Current user logged out'));
  }

  reloadUser() {
    this.authService.login()
      .subscribe(() => this.currentUser = this.authService.currentUser);
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
