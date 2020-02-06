import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { PaginationSummary } from 'app/core/models';
import { SearchQueryParameters } from 'app/core/models/search-interfaces';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.scss']
})
export class PaginatorComponent implements OnChanges {
  @Input() metaData: PaginationSummary;
  @Output() params = new EventEmitter<SearchQueryParameters>();

  boundaryLinks = true; // do we show the jump to first and last page links?
  pageSize: number; // how many entries are on the page
  page: number; // the current displayed page
  totalCount: number; // how large is the collection?

  constructor() { }

  ngOnChanges() {
    // convert the input to a useful object
    if (this.metaData) {
      this.parseMeta(this.metaData);
    }
  }

  parseMeta(metaData: PaginationSummary) {
    // take the input and convert it to useful info for ng-bootstrap
    this.page = metaData.page;
    this.pageSize = metaData.pageSize;
    this.totalCount = metaData.totalCount;
  }

  emitParameters() {
    this.params.emit({
      // how many records we want
      limit: this.pageSize,
      // pagination is calculated
      offset: (this.page * this.pageSize) - this.pageSize,
    });
  }
}
