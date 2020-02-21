import { Component, OnInit, Input, SimpleChange, OnChanges } from '@angular/core';
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
    if (this.displayName == null && this.currentUser !== null) {
      // Separated by spaces... usually
      let spaceSplit: string[] = this.currentUser.name.split(' ');
      let commaSplit: string[] = this.currentUser.name.split(',');
      if (spaceSplit.length >= 2) {
        this.generateDisplayName(spaceSplit);
      }
      // Try splitting on commas
      else if (commaSplit.length >= 2) {
        this.generateDisplayName(commaSplit);
      }
      // Just use the name
      else {
        this.displayName = this.currentUser.name;
      }
    }
  }

  private generateDisplayName(nameArray: string[]) {
    let fName: string = null;
    let lName: string = null;
    if (nameArray.length >= 2) {
      fName = nameArray[0];
      lName = nameArray[1];
      if (lName.length >= 1) {
        lName = lName.substr(0, 1);
      }
      this.displayName = `${fName} ${lName}`;
      // Clean up any uneeded commas
      this.displayName = this.displayName.replace(',', '');
    }
    else if (nameArray.length === 1) {
      this.displayName = nameArray[0];
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
