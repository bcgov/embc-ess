import { Component, OnInit, ViewChild, ElementRef, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-search-box',
  template: `
    <div class="form-row">
      <div class="col-md-9">
        <input #searchBox
          type="text"
          aria-label="search"
          placeholder="{{placeholder}}"
          (keydown.enter)="doSearch()">
      </div>
      <div class="col-md-3">
        <button class="btn btn-primary" (click)="doSearch()">Search</button>
      </div>
    </div>
  `,
  styles: []
})
export class SearchBoxComponent implements OnInit {

  @ViewChild('searchBox') searchBox: ElementRef;
  @Input() placeholder: string;
  @Output() search = new EventEmitter<string>();

  ngOnInit() {
  }

  doSearch() {
    this.search.emit(this.query);
  }

  private get query() { return this.searchBox.nativeElement.value; }
  private set query(value: string) { this.searchBox.nativeElement.value = value; }
}
