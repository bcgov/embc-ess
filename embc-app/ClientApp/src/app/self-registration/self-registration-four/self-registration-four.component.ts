import { Component, OnInit } from '@angular/core';
import { of } from 'rxjs';

// mocking server-side API for now
const mockResponse = { registrationNumber: '1234 5678' };

@Component({
  selector: 'app-self-registration-four',
  templateUrl: './self-registration-four.component.html',
  styleUrls: ['./self-registration-four.component.scss']
})
export class SelfRegistrationFourComponent implements OnInit {
  registrationNumber: string;

  constructor() { }

  ngOnInit() {
    this.fetchRegistration()
      .subscribe(reg => {
        this.registrationNumber = reg.registrationNumber;
      });
  }

  // TODO: Get this from backend server
  fetchRegistration() {
    return of({ ...mockResponse });
  }
}
