import { Component, Input } from '@angular/core';
import { PaginationSummary } from 'app/core/models';

@Component({
  selector: 'app-pagination-summary',
  template: `
    <ng-container *ngIf="value">
      {{start}} - {{end}} of {{value?.totalCount}}
      <ng-content></ng-content>
    </ng-container>
  `,
  styles: []
})
export class PaginationSummaryComponent {
  @Input() value: PaginationSummary;

  // convenience getters
  get start(): number | null {
    if (!this.value) {
      return null;
    }
    const startOfPage = (this.value.page - 1) * this.value.pageSize;
    return startOfPage + 1;
  }

  get end(): number | null {
    if (!this.value) {
      return null;
    }
    const total = this.value.totalCount;
    const endOfPage = this.value.page * this.value.pageSize;
    return Math.min(total, endOfPage);
  }
}
