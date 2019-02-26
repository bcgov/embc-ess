import { Component, OnInit, Input, Directive } from '@angular/core';

@Component({
  selector: 'app-side-box',
  templateUrl: './side-box.component.html',
  styleUrls: ['./side-box.component.scss']
})
export class SideBoxComponent implements OnInit {
  @Input() title = 'Sample Title';
  @Input() icon = 'description';

  constructor() { }

  ngOnInit() {
  }
}

/**
 * Action section of the card, needed as it's used as a selector in the API.
 * @docs-private
 */
@Directive({
  selector: 'app-side-box-actions',
})
export class SideBoxActions { }
