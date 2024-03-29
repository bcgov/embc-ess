import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { PROVINCIAL_ADMIN, LOCAL_AUTHORITY, VOLUNTEER, EVERYONE } from 'src/app/constants';
import { Router } from '@angular/router';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';
import { ReadOnlyService } from '../../core/services/read-only.service';

@Component({
  selector: 'app-top-navbar',
  templateUrl: './top-navbar.component.html',
  styleUrls: ['./top-navbar.component.scss']
})
export class TopNavbarComponent implements OnInit {
  @ViewChild('navbarSupportedContent') navbarSupportedContent: ElementRef;

  // this is only to decide what to show.
  // all route protection should be handled in routing
  role: string;

  // store the constants
  provincialAdmin = PROVINCIAL_ADMIN;
  localAuthority = LOCAL_AUTHORITY;
  volunteer = VOLUNTEER;
  everyone = EVERYONE;

  constructor(
    private authService: AuthService,
    public router: Router, // used in HTML
    private uniqueKeyService: UniqueKeyService,
    public readOnlyService: ReadOnlyService
  ) { }

  ngOnInit() {
    // watch the current user for changes.
    this.authService.role.subscribe((role: string) => {
      this.role = role;
    });
  }
  // toggle collapse class when user clicks on navbar button
  toggleMenu() {
    if (this.navbarSupportedContent) {
      this.navbarSupportedContent.nativeElement.classList.toggle('collapse');
    }
  }

  // collapse menu when user clicks on a link
  closeMenu() {
    if (this.navbarSupportedContent) {
      // NB - class is only added once
      this.navbarSupportedContent.nativeElement.classList.add('collapse');
    }
  }

  // Clear out any stored registration Id - when the link is clicked we're always going to be doing a new one.
  clearCachedKey() {
    this.uniqueKeyService.clearKey();
  }


}
