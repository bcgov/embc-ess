import { Component, OnInit, Input } from '@angular/core';
import range from 'lodash/range';

@Component({
  selector: 'app-valid-from-to',
  templateUrl: './valid-from-to.component.html',
  styleUrls: ['./valid-from-to.component.scss']
})
export class ValidFromToComponent implements OnInit {
  @Input() validFrom: Date = null;
  @Input() validTo: Date = null;

  days = range(1, 15); // [1..14]

  constructor() { }

  ngOnInit() { }

}
