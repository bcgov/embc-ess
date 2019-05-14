import { Component, OnInit } from '@angular/core';
import { VolunteerService } from '../core/services/volunteer.service';


@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {
  page: number;
  params = {
    limit: 10,
    offset: 0,
  };
  volunteers;
  constructor(
    private volunteerService: VolunteerService
  ) { }
  ngOnInit() {
    this.getVolunteers();
  }
  getVolunteers() {
    this.volunteerService.getVolunteers(this.params).subscribe(v => this.volunteers = v);
  }
  onEvent(event) {
    this.params = event;
  }
}
