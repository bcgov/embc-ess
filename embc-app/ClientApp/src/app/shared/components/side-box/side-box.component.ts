import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-side-box',
  styleUrls: ['./side-box.component.scss'],
  template: `

    <div class="card text-white bg-primary">
      <div class="card-header">
      <app-font-awesome-icon icon="info-circle" size="lg"></app-font-awesome-icon>
      <span class="h5 ml-2 text-wrap">{{title}}</span>
      </div>
      <div class="card-body">
        <span class="card-text">
          <ng-content></ng-content>
        </span>
      </div>
    </div>
  `,
})
export class SideBoxComponent {

  @Input() title = 'Useful Information';
  @Input() secondaryTitle = '';
  @Input() icon = 'description';

  constructor() { }

}
