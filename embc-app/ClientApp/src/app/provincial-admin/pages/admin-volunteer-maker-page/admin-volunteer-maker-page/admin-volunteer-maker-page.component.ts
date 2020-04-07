import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-volunteer-maker-page',
  templateUrl: './admin-volunteer-maker-page.component.html',
  styleUrls: ['./admin-volunteer-maker-page.component.scss']
})
export class AdminVolunteerMakerPageComponent implements OnInit {
  onSummaryPage: boolean = false;
  constructor() { }

  ngOnInit() {
  }

  onSummaryChange(onSummary: boolean) {
    this.onSummaryPage = onSummary;
  }

}
