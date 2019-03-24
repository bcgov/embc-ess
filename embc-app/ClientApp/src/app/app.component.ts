import { Component, OnInit } from '@angular/core';
import { concat } from 'rxjs';

import { User } from './core/models';
import { detectIE10orLower } from './shared/utils';
import { ControlledListService } from './core/services/controlled-list.service';
import { UserDataService } from './core/services/user-data.service';

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
    private userDataService: UserDataService,

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

    this.reloadUser();
    // Loaded once at init time, as they do not change very often, and
    // certainly not within the app.
    this.getLookups().subscribe();
  }

  reloadUser() {
    this.userDataService.getCurrentUser()
      .subscribe((data: User) => {
        this.currentUser = data;
        //this.isNewUser = this.currentUser.isNewUser;

        //this.store.dispatch(new CurrentUserActions.SetCurrentUserAction(data));
        // this.isAssociate = (this.currentUser.businessname == null);
        // if (!this.isAssociate) {
        //   this.adoxioLegalEntityDataService.getBusinessProfileSummary().subscribe(
        //     res => {
        //       this.businessProfiles = res;
        //     });
        // }
      });
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
