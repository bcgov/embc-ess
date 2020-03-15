import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-admin-registrations-page',
  templateUrl: './admin-registrations-page.component.html',
  styleUrls: ['./admin-registrations-page.component.scss']
})
export class AdminRegistrationsPageComponent implements OnInit {

  isAdmin: boolean = false;
  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.isProvincialAdmin$.subscribe((result: boolean) => this.isAdmin = result);
  }



}
