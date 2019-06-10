import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-self-registration-four',
  templateUrl: './self-registration-four.component.html',
  styleUrls: ['./self-registration-four.component.scss']
})
export class SelfRegistrationFourComponent implements OnInit {
  registrationNumber: string;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.registrationNumber = this.route.snapshot.paramMap.get('id');
  }
}
