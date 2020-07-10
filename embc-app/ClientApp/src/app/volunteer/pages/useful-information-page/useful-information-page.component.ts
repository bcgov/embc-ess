import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { VersionService } from 'src/app/core/services/version.service';
import { AppVersion } from 'src/app/core/models/app-version.model';

@Component({
  templateUrl: './useful-information-page.component.html',
  styleUrls: ['./useful-information-page.component.scss']
})
export class UsefulInformationPageComponent implements OnInit {

  isSuperUser: boolean;
  isAdmin: boolean;
  releaseVersion: string;
  constructor(private authService: AuthService, private version: VersionService) { }
  ngOnInit(): void {
    this.isSuperUser = this.authService.isSuperUserOrAdmin();
    this.authService.isProvincialAdmin$.subscribe(e => this.isAdmin = e);
    this.version.getVersion().subscribe(v => this.releaseVersion = v.releaseVersion);
  }

  openRefGuide() {
    const url = "https://ess.gov.bc.ca/era/documents/ERA-Quick-Reference-Guide-Getting-Started.pdf";
    window.open(url, "_blank");
  }

  openTrainingGuide() {
    let url = "https://ess.gov.bc.ca/era/documents/";
    // ERA User
    if (!this.isSuperUser) {
      url += "ERA-User-Guide-ERA-Users.pdf";
    }
    // Superuser
    else if (this.isSuperUser && !this.isAdmin) {
      url += "ERA-User-Guide-ERA-Superusers.pdf";
    }
    // Admin
    else if (this.isAdmin){
      url += "ERA-User-Guide-ERA-Admin.pdf";
    }
    window.open(url, "_blank");
  }

}
