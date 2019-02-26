import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-warning-banner',
  templateUrl: './warning-banner.component.html',
  styleUrls: ['./warning-banner.component.scss']
})
export class WarningBannerComponent implements OnInit {
  @Input() show = false;

  constructor() { }

  ngOnInit() {
  }
}
