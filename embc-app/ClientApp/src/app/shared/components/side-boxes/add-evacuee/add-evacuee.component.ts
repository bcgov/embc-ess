import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-add-evacuee',
  templateUrl: './add-evacuee.component.html',
  styleUrls: ['./add-evacuee.component.scss']
})
export class AddEvacueeComponent implements OnInit {

  @Input() route = '../register-evacuee'; // TODO: don't pass route in when routing is flattened corrected for all components that use THIS component
  @Input() absoluteRouting = false;

  constructor() { }

  ngOnInit() {
  }

}
