import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-address-form-group',
  templateUrl: './address-form-group.component.html',
  styleUrls: ['./address-form-group.component.scss']
})
export class AddressFormGroupComponent implements OnInit {

  @Input()
  parent: FormGroup;

  constructor() { }

  ngOnInit() {
  }

  get invalid() {
    return false;
    //   return (
    //     this.parent.get('name').hasError('required') &&
    //     this.parent.get('name').touched
    //   );
  }
}
