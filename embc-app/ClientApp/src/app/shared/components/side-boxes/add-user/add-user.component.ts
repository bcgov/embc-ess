import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {
  @Input() route = ''; // TODO: don't pass route in when routing is flattened corrected for all components that use THIS component

  constructor() { }

  ngOnInit() {  }

}
