import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { PaginationSummary } from 'src/app/core/models';
import { SearchQueryParameters } from 'src/app/core/models/search-interfaces';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.scss']
})
export class PaginatorComponent implements OnChanges {
  @Input() metaData: PaginationSummary;
  @Output() params = new EventEmitter<SearchQueryParameters>();

  // collection of pagination parameters for UI pagination
  // display and pagination
  increments: number[] = [5, 10, 25, 50, 100, 1000];
  // doesn't need to be an object besides it provides a visual seper
  pageSize: number; // how many entries are on the page
  boundaryLinks = true; // do we show the jump to first and last page links?

  maxSize = 20; // how many records should the UI show?
  page: number; // the current displayed page
  collectionSize: number = 0; // how large is the collection?

  constructor() { }

  ngOnChanges() {
    this.parseMeta(this.metaData);
  }

  parseMeta(metaData: PaginationSummary) {
    // take the input and convert it to useful info for ng-bootstrap
    this.page = metaData.page;
    this.collectionSize = metaData.totalCount;
    this.maxSize = metaData.pageSize;
  }

  generateParameters(): SearchQueryParameters {
    return {
      // pagination is calculated
      offset: (this.page * this.maxSize) - this.maxSize,
      // how many records we want
      limit: this.maxSize,
    };
  }
  emitParameters() {
    const sqp = this.generateParameters();
    this.params.emit(sqp);
  }
}
