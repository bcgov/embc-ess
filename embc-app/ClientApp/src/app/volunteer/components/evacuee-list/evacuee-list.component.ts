import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-evacuee-list',
  templateUrl: './evacuee-list.component.html',
  styleUrls: ['./evacuee-list.component.scss']
})
export class EvacueeListComponent implements OnInit {

  constructor() { }

  evacuees = [
    { firstName: 'Anne', lastName: 'Smith', checked: false },
    { firstName: 'Jack', lastName: 'Smith', checked: false },
    { firstName: 'Robert', lastName: 'Smith', checked: true },
    { firstName: 'Joan', lastName: 'Smith', checked: false }
  ];

  ngOnInit() {
  }

}
