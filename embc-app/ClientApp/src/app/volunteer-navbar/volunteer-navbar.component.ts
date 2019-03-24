import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-volunteer-navbar',
  templateUrl: './volunteer-navbar.component.html',
  styleUrls: ['./volunteer-navbar.component.scss']
})
export class VolunteerNavbarComponent implements OnInit {

  // This component needs to be only shown when the application state for the user is correct.
  constructor() { }

  ngOnInit() {
  }

}
