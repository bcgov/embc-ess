import { Component, OnInit, HostListener } from '@angular/core';
import { forkJoin } from 'rxjs';
// import { User } from './core/models';
import { detectIE10orLower } from './shared/utils';
import { ControlledListService } from './core/services/controlled-list.service';
import { AuthService } from './core/services/auth.service';
import { UniqueKeyService } from './core/services/unique-key.service';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AppState } from './store';
import { Config } from './core/models';
import { WatchdogService } from './core/services/watchdog.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  isIE = false;

  private config: Config;

  constructor(
    private lookups: ControlledListService,
    public authService: AuthService,
    public uniqueKeyService: UniqueKeyService,
    // private router: Router,
    private store: Store<AppState> // ngrx app state
  ) {
    // get config
    this.store.select(s => s.lookups.config.config).subscribe((config: Config) => {
      this.config = config;
    });
  }

  ngOnInit() {
    this.isIE = detectIE10orLower();

    // there shall be no reloading of pages. You are always routed back to the home page if you weren't here to begin with. You are also forced to logout. No restoring sessions ever.
    // this.store.select(s => s.lookups.config.config).subscribe(config => {
    //   // this if statement allows refreshing a page in dev. Otherwise it is painful to do any testing.
    //   if (config && !config.environment.toUpperCase().includes('DEV (')) {
    //     if (this.router.url !== '') {
    //       this.router.navigateByUrl('');
    //       this.authService.logout(true);
    //     }
    //   }
    // });

    // check for authenticated user
    this.login();
    // load these once at init time as they do not change very often,
    // and certainly not within the app
    this.getLookups();
  }

  get versionInfo(): any {
    return this.config
      ? `${this.config.sourceReference}_${this.config.sourceCommit}_${this.config.fileCreationTime}`
      : ''
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
    this.authService.login(true).subscribe();
  }

  private getLookups() {
    return forkJoin(
      this.lookups.getConfig(),
      this.lookups.getAllCountries(),
      this.lookups.getAllRegions(),
      this.lookups.getAllCommunities(),
      this.lookups.getAllFamilyRelationshipTypes(),
      // ...add more
    ).subscribe();
  }
}
