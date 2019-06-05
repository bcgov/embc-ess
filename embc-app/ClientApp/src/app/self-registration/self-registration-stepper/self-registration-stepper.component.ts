import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-self-registration-stepper',
  templateUrl: './self-registration-stepper.component.html',
  styleUrls: ['./self-registration-stepper.component.scss']
})
export class SelfRegistrationStepperComponent implements OnInit {

  pages = [1, 2, 3, 4];
  constructor() { }

  ngOnInit() {
  }

}
