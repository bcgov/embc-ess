import { Component, Input, HostBinding } from '@angular/core';
import { IconSizes } from '../font-awesome-icon/font-awesome-icon.component';

/**
 * Wrapper component around a FontAwesomeIcon that provides
 * attention icons (i.e. exclamation triangles) to bring attention
 * to a given form field that needs to be captured.
 */
@Component({
  selector: 'app-attention-icon',
  template: `
    <app-font-awesome-icon
      icon="exclamation-triangle"
      class="attention-icon col-md-auto p-0"
      [class.pull-icon-left]="pullLeft"
      [color]="color"
      [size]="size">
    </app-font-awesome-icon>
  `,
  styles: [`
    .attention-icon.pull-icon-left {
      position: relative;
      left: -3rem;
      width: 0px;
    }
  `]
})
export class AttentionIconComponent {

  @HostBinding('class.d-flex')
  @HostBinding('class.align-items-center')
  @Input() alignCenter = true;
  @Input() pullLeft = true;
  @Input() color = '#FF9900';
  @Input() size: IconSizes = 'lg';
}
