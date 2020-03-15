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
  // This should be in a constants file or something
  private IDIR_USER_TYPE: string = "internal";
  private isIDIR: boolean;
  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChange) {
    // TODO: Fix this spitting out an error on the first change
    // This if statement evaluates to true but then when we access currentUser.userType, that propety doesn't exist
    // This isn't breaking anything... the next change it evaluates correctly, but we do get an error in the console.
    if (this.displayName == null && this.currentUser !== null && this.currentUser.userType != null) { 
      // Determine user type: IDIR format is lName, fName while BCeID is fName, lName
      this.isIDIR = this.currentUser.userType === this.IDIR_USER_TYPE;
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
      fName = this.isIDIR ? nameArray[1] : nameArray[0];
      lName = this.isIDIR ? nameArray[0] : nameArray[1];
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
