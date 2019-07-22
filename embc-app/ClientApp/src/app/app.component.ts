import { Component, OnInit, HostListener } from '@angular/core';
import { forkJoin } from 'rxjs';
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

  isIE = false;
  versionInfo = '';
  constructor(
    private controlledListService: ControlledListService,
    public authService: AuthService,
    public uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    this.isIE = detectIE10orLower();

    // check for authenticated user
    this.login();

    // load these globally at init time as they do not change very often,
    // and certainly not within the app
    this.getLookups();
  }

  // force logout when window (browser tab) is closed
  @HostListener('window:beforeunload', ['$event'])
  onWindowClose(event: any): void {
    this.authService.logout(true).subscribe();
  }

  private login() {
    // when visiting this page check for login status.
    this.authService.login(true).subscribe();
  }

  private getLookups() {
    return forkJoin(
      this.controlledListService.getConfig(),
      this.controlledListService.getAllCountries(),
      this.controlledListService.getAllRegions(),
      this.controlledListService.getAllCommunities(),
      this.controlledListService.getAllFamilyRelationshipTypes(),
      // ...add more
    ).subscribe();
  }

}
