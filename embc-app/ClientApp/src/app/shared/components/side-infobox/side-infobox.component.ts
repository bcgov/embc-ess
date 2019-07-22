import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-side-infobox',
  styleUrls: ['./side-infobox.component.scss'],
  templateUrl: './side-infobox.component.html',
})
export class SideInfoboxComponent {

  @Input() title = 'Useful Information';
  @Input() secondaryTitle = '';
  @Input() icon = 'description';

  constructor() { }

}
