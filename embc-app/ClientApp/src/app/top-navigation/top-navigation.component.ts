import { Component, OnInit } from '@angular/core';
import { AuthService } from '../core/services/auth.service';

// export const EVERYONE = 'role_everyone';                    // --> default (means no role associated, ie the PUBLIC side)
// export const VOLUNTEER = 'role_volunteer';                  // --> can add/edit/finalize evacuee registrations
// export const LOCAL_AUTHORITY = 'role_local_authority';      // --> can add/edit/deactivate volunteers, deactivate registrations
// export const PROVINCIAL_ADMIN = 'role_provincial_admin'; 

@Component({
  selector: 'app-top-navigation',
  templateUrl: './top-navigation.component.html',
  styleUrls: ['./top-navigation.component.scss']
})
export class TopNavigationComponent implements OnInit {

  // this is only to decide what to show.
  // all route protection should be handled in routing
  role: string;

  constructor(
    private authService: AuthService,
  ) { }

  ngOnInit() {
    // watch the current user for changes.
    this.authService.user.subscribe(r => {
      // check that the user exists and has roles
      if (r !== null && r.appRoles.length > 0) {
        // handle the role and save the user
        // if there are elements in the array
        this.role = r.appRoles[r.appRoles.length - 1];
      } else {
        this.role = 'role_everyone';
      }
    });
  }

}
