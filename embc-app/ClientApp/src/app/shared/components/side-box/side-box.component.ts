import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-side-box',
  styleUrls: ['./side-box.component.scss'],
  template: `
    <mat-card class="side-box shadow">
      <mat-card-header>
        <h2 mat-card-avatar class="round-icon-material">
          <mat-icon class="icon" aria-label="description icon">{{icon}}</mat-icon>
        </h2>
        <mat-card-title>
          <h2>{{title}}</h2>
        </mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <ng-content select="section[role=content]"></ng-content>
      </mat-card-content>
      <mat-card-actions align="end">
        <ng-content select="section[role=footer]"></ng-content>
      </mat-card-actions>
    </mat-card>
  `,
})
export class SideBoxComponent {

  @Input() title = 'Sample Title';
  @Input() icon = 'description';

  constructor() { }

}
