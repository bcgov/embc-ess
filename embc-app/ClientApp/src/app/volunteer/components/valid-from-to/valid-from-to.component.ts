import { Component, OnInit } from '@angular/core';
import range from 'lodash/range';

@Component({
  selector: 'app-valid-from-to',
  templateUrl: './valid-from-to.component.html',
  styleUrls: ['./valid-from-to.component.scss']
})
export class ValidFromToComponent implements OnInit {

  days = range(1, 6); // [1,2,3,4,5]

  constructor() { }

  ngOnInit() { }

}
