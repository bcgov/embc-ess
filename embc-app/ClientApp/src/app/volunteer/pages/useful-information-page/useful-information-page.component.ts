import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  templateUrl: './useful-information-page.component.html',
  styleUrls: ['./useful-information-page.component.scss']
})
export class UsefulInformationPageComponent implements OnInit {

  isSuperUser: boolean;
  constructor(private authService: AuthService) { }
  ngOnInit(): void {
    this.isSuperUser = this.authService.isSuperUserOrAdmin();
  }

  openRefGuide() {
    const url = "https://ess.pathfinder.gov.bc.ca/era/documents/ERA-Quick-Reference-Guide-Getting-Started.pdf";
    window.open(url, "_blank");
  }

  openTrainingGuide() {
    const url = this.isSuperUser
                ? "https://ess.pathfinder.gov.bc.ca/era/documents/ERA-User-Guide-ERA-Superusers.pdf"
                : "https://ess.pathfinder.gov.bc.ca/era/documents/ERA-User-Guide-ERA-Users.pdf";
    window.open(url, "_blank");
  }

}
