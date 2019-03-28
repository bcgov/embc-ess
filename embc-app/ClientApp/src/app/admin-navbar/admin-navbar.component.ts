import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-navbar',
  templateUrl: './admin-navbar.component.html',
  styleUrls: ['./admin-navbar.component.scss']
})
export class AdminNavbarComponent implements OnInit {

  // This component needs to be only shown when the application state for the user is correct.
  constructor() { }

  ngOnInit() {
  }

}
