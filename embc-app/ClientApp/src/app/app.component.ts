import { Component, OnInit, HostListener } from '@angular/core';
import { concat } from 'rxjs';
// import { User } from './core/models';
import { detectIE10orLower } from './shared/utils';
import { ControlledListService } from './core/services/controlled-list.service';
import { AuthService } from './core/services/auth.service';
import { UniqueKeyService } from './core/services/unique-key.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  public isIE = false;

  constructor(
    private lookups: ControlledListService,
    public authService: AuthService,
    public uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    this.isIE = detectIE10orLower();

    // check for authenticated user
    this.login();

    // load these once at init time as they do not change very often,
    // and certainly not within the app
    this.getLookups();
  }

  get versionInfo(): any {
    return null;
  }

  // get currentUser(): User {
  //   return this.authService.currentUser;
  // }

  // get isAuthenticated(): boolean {
  //   return this.authService.isLoggedIn;
  // }

  // get isNewUser(): boolean {
  //   return this.authService.isNewUser;
  // }

  // get isReturningUser(): boolean {
  //   return this.authService.isLoggedIn;
  // }

  // force logout when window (browser tab) is closed
  @HostListener('window:beforeunload', ['$event'])
  onWindowClose(event: any): void {
    this.authService.logout(true).subscribe();
  }

  private login() {
    this.authService.login().subscribe();
  }

  private getLookups() {
    return concat(
      this.lookups.getAllCountries(),
      this.lookups.getAllRegions(),
      this.lookups.getAllRegionalDistricts(),
      this.lookups.getAllCommunities(),
      this.lookups.getAllFamilyRelationshipTypes(),
      // ...add more
    ).subscribe();
  }

}
