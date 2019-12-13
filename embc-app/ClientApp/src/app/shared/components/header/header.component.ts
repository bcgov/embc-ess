import { Component, OnInit, Input, OnChanges, SimpleChanges, SimpleChange } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/core/models';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnChanges {

  @Input() currentUser: User;
  displayName: string = null;

  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.displayName === null) {
      let userChanges: SimpleChange = changes["currentUser"];
      if (userChanges.currentValue.firstname && userChanges.currentValue.lastname) {
        this.displayName = `${userChanges.currentValue.firstname} ${userChanges.currentValue.lastname.substr(0,1)}`;
      }
      // Not sure if necessary...
      else if (userChanges.currentValue.name) {
        this.displayName = userChanges.currentValue.name;
      }
    }
  }

  homeButton() {
    this.authService.role.subscribe((role: string) => {
      // if the role is null we redirect them to a logged out home location otherwise they go to their dash.
      if (!role || role === 'role_everyone') {
        this.router.navigate(['/']);
      } else {
        this.router.navigate(['dashboard']);
      }
    });
  }
}
