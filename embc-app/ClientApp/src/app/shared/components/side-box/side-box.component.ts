import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-side-box',
  styleUrls: ['./side-box.component.scss'],
  template: `

    <div class="card text-white bg-primary">
      <div class="card-header">
      <app-font-awesome-icon icon="info-circle" size="lg"></app-font-awesome-icon>
      <span class="h5 ml-2 text-wrap">Useful Information</span>
      </div>
      <div class="card-body">
        <!--<span class="h2 card-title">{{secondaryTitle}}</span>-->
        <p class="card-text">
          Evacuees listed here that do not have a task number are self-registered and have not yet received Emergency Support Services.
        </p>
        <p class="card-text">
          Use the “Action” column to “view” evacuee registrations as well as “finalize” evacuee self-registrations.
        </p>
      </div>
    </div>
  `,
})
export class SideBoxComponent {

  @Input() title = '';
  @Input() secondaryTitle = '';
  @Input() bodyText = '';
  @Input() icon = 'description';

  constructor() { }

}
