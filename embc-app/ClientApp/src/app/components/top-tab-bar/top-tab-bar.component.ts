import { Component, OnInit } from '@angular/core';
import { AuthService } from 'app/core/services/auth.service';
import { PROVINCIAL_ADMIN, LOCAL_AUTHORITY, VOLUNTEER } from 'app/constants';
import { Router } from '@angular/router';

@Component({
  selector: 'app-top-tab-bar',
  templateUrl: './top-tab-bar.component.html',
  styleUrls: ['./top-tab-bar.component.scss']
})
export class TopTabBarComponent implements OnInit {

  // this is only to decide what to show.
  // all route protection should be handled in routing
  role: string;

  // store the constants
  provincialAdmin = PROVINCIAL_ADMIN;
  localAuthority = LOCAL_AUTHORITY;
  volunteer = VOLUNTEER;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    // watch the current user for changes.
    this.authService.role.subscribe((role: string) => {
      // check that the user exists and has roles
      this.role = role;
    });
  }


}
