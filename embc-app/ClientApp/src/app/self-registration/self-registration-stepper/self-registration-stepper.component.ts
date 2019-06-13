import { Component, OnInit } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-self-registration-stepper',
  templateUrl: './self-registration-stepper.component.html',
  styleUrls: ['./self-registration-stepper.component.scss']
})
export class SelfRegistrationStepperComponent implements OnInit {
  pages = [1, 2, 3, 4];
  step;
  constructor(
    private router: Router
  ) { }

  ngOnInit() {
    // which step is this?
    this.step = parseInt(this.router.url.split('/')[2].split('-')[1]);
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        // tslint:disable-next-line: radix
        this.step = parseInt(this.router.url.split('/')[2].split('-')[1]);
      };
    });
  }

}
