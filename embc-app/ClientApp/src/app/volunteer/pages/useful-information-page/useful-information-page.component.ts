import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { User } from 'src/app/core/models';

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

}
