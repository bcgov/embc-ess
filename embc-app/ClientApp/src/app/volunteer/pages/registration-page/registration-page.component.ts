import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-registration-page',
  templateUrl: './registration-page.component.html',
  styleUrls: ['./registration-page.component.scss']
})
export class RegistrationPageComponent implements OnInit {

  onSummaryPage: boolean = false;
  constructor() { }

  ngOnInit() {
  }

  onSummaryChange(onSummary: boolean) {
    this.onSummaryPage = onSummary;
  }

}
