import { Component, Input } from '@angular/core';
import { IconSizes } from '../font-awesome-icon/font-awesome-icon.component';

@Component({
  selector: 'app-fa-link',
  template: `
    <a [routerLink]="link">
      <app-font-awesome-icon
        [icon]="icon"
        [border]="border"
        [color]="color"
        [inverse]="inverse"
        [size]="size">
      </app-font-awesome-icon>
      <span class="ml-2">
        <ng-content></ng-content>
      </span>
    </a>
  `,
  styles: []
})
export class FontAwesomeLinkComponent {
  @Input() icon: string;
  @Input() border = false;
  @Input() color = 'inherit';
  @Input() inverse = false;
  @Input() size: IconSizes = null;
  @Input() text: string;
  @Input() link: string = null;
}
