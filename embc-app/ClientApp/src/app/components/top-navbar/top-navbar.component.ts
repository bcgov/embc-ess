import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { PROVINCIAL_ADMIN, LOCAL_AUTHORITY, VOLUNTEER } from 'src/app/constants';
import { Router } from '@angular/router';

@Component({
  selector: 'app-top-navbar',
  templateUrl: './top-navbar.component.html',
  styleUrls: ['./top-navbar.component.scss']
})
export class TopNavbarComponent implements OnInit {

  // this is only to decide what to show.
  // all route protection should be handled in routing
  role: string;

  // store the constants
  provincialAdmin = PROVINCIAL_ADMIN;
  localAuthority = LOCAL_AUTHORITY;
  volunteer = VOLUNTEER;

  constructor(
    private authService: AuthService,
    public router: Router // used in HTML
  ) { }

  ngOnInit() {
    // watch the current user for changes.
    this.authService.role.subscribe((role: string) => {
      this.role = role;
    });
  }

}
