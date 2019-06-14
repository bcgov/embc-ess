import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-self-registration-stepper',
  templateUrl: './self-registration-stepper.component.html',
  styleUrls: ['./self-registration-stepper.component.scss']
})
export class SelfRegistrationStepperComponent implements OnInit {
  step = 1;
  constructor(
    private router: Router,
    private route: ActivatedRoute,

  ) { }

  ngOnInit() {
    // which step is this?
    this.router.events.forEach(event => {
      if (event instanceof NavigationEnd) {
        this.step = Number(this.route.snapshot.firstChild.url[0].path.split('-')[1]);
      }
    });
  }
}
