import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-side-box-info',
  templateUrl: './side-box-info.component.html',
  styleUrls: ['./side-box-info.component.scss']
})
export class SideBoxInfoComponent implements OnInit {
  @Input() title = 'Sample Title';
  @Input() icon = 'description';

  constructor() { }

  ngOnInit() {
  }

}
