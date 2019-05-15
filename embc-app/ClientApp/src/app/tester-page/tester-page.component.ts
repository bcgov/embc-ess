import { Component, OnInit } from '@angular/core';
import { VolunteerService } from '../core/services/volunteer.service';
import { SearchQueryParameters } from '../core/models/search-interfaces';
import { PaginationSummary } from '../core/models';


@Component({
  selector: 'app-tester-page',
  templateUrl: './tester-page.component.html',
  styleUrls: ['./tester-page.component.scss']
})
export class TesterPageComponent implements OnInit {
  // page: number;
  // paginationSummary: PaginationSummary;
  // params: SearchQueryParameters = {
  //   limit: 10,
  //   offset: 0,
  // };
  // volunteers;
  constructor(
    // private volunteerService: VolunteerService
  ) { }
  ngOnInit() {
    // this.getVolunteers(this.params);
  }
  // getVolunteers(params) {
  //   this.volunteerService.getVolunteers(params).subscribe(v => {
  //     this.volunteers = v;
  //     this.paginationSummary = v.metadata;
  //   });
  // }
  // onEvent(event: SearchQueryParameters) {
  //   this.params = event;
  // }
}
