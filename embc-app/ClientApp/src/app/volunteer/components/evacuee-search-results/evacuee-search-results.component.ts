import { Component, OnInit, Input } from '@angular/core';
import { ListResult, EvacueeListItem } from 'src/app/core/models';

@Component({
  selector: 'app-evacuee-search-results',
  templateUrl: './evacuee-search-results.component.html',
  styleUrls: ['./evacuee-search-results.component.scss']
})
export class EvacueeSearchResultsComponent implements OnInit {
  @Input() searchResults: ListResult<EvacueeListItem>;

  constructor() { }

  ngOnInit() {
  }



}
