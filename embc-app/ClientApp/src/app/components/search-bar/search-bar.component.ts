import { Component, ViewChild, ElementRef, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-search-bar',
  template: `
    <div role="search" class="form-row">
      <div class="col">
        <input #searchBox
          type="text"
          aria-label="search"
          class="form-control mb-2"
          placeholder="{{placeholder}}"
          (keydown.enter)="doSearch()">
      </div>
      <div class="col-auto">
        <button class="btn btn-primary" (click)="doSearch()">SEARCH</button>
      </div>
    </div>
  `,
  styles: []
})
export class SearchBarComponent {

  @ViewChild('searchBox') searchBox: ElementRef;
  @Input() placeholder = 'Search';
  @Output() valueChange = new EventEmitter<string>();

  doSearch() {
    this.valueChange.emit(this.query);
  }

  private get query() { return this.searchBox.nativeElement.value; }
  private set query(value: string) { this.searchBox.nativeElement.value = value; }
}
