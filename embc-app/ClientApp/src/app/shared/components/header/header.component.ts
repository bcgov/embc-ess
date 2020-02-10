import { Component, OnInit, Input, OnChanges, SimpleChange } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/core/models';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Input() currentUser: User;

  displayName: string = null;

  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
  }
  
  ngOnChanges(changes: SimpleChange) {
    // Only build the display name once, after all inputs are initialized
    if (this.currentUser && this.displayName === null) {
      console.log("Hey there",this.currentUser);
      let lName: string = "";
      // Extract the first letter of the last name if exists
      // Wanted to write it as if (this.currentUser.lastname?.length > 0) but it wouldn't compile even though there were no errors <\(-_-)/>
      if (this.currentUser.lastname && this.currentUser.lastname.length > 0) {
        lName = this.currentUser.lastname.substr(0,1);
      }
      this.displayName = `${this.currentUser.firstname} ${lName}`;
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
