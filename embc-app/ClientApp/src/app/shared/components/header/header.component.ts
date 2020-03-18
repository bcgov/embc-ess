import { Component, OnInit, Input, SimpleChange, OnChanges, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/core/models';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnChanges {
  @Input() currentUser: User = undefined;
  displayName: string = null;
  showSelfRegText: boolean = false;
  // This should be in a constants file or something
  private IDIR_USER_TYPE: string = "internal";
  private isIDIR: boolean;
  constructor(
    private router: Router,
    private authService: AuthService
  ) {
    // This is a stupid work around that I hate
    // Wanted to put the 'ShowHeaderText' into the data but it's always empty
    // Possibly related to this issue: https://github.com/angular/angular/issues/19420
    // So, what we want to do is show certain text in the header ONLY when it's a self-registration
    // and if we can't access the data observable we'll do the hacky solution of reading the URL 
    // and toggling the boolean if it matches /self-registration/
    router.events.subscribe((val) => {
      const url: string = router.url;
      this.showSelfRegText = url.includes("self-registration");
  });
   }
  ngOnChanges(changes: SimpleChanges): void {
    // This if statement evaluates to true but then when we access currentUser.userType, that propety doesn't exist
    // This isn't breaking anything... the next change it evaluates correctly, but we do get an error in the console.
    if (this.displayName == null && this.currentUser != undefined && this.currentUser.userType != null) {
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


  ngOnInit() {
    // this.activatedRoute.data.subscribe(data => {
    //   if (data.showHeaderText) {
    //     this.showSelfRegText = true;
    //   }
    //   console.info("activateRoute.data updated!", data);
    // });
    // this.activatedRoute.url.subscribe(foo => console.log(foo));
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
