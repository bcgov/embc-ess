import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-self-registration-stepper',
  templateUrl: './self-registration-stepper.component.html',
  styleUrls: ['./self-registration-stepper.component.scss']
})
export class SelfRegistrationStepperComponent implements OnInit {
  pages = [1, 2, 3, 4];
  step = 0;
  activeRoute;
  constructor(
    private router: Router,

  ) { }

  ngOnInit() {
    // which step is this?
    // this.step = parseInt(this.router.url.split('/')[2].split('-')[1]);
    this.router.events.subscribe(x => {
      if (x instanceof NavigationEnd) {
        this.step = parseInt(x.url.split('/')[2].split('-')[1]);
      }
    });
  }

}
