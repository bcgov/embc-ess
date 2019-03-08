import { Component, OnInit, Input, Directive } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { Community, Country } from 'src/app/core/models';

@Component({
  selector: 'app-bc-address',
  template: `
    <section [formGroup]="parent">
      <div class="row">
        <app-form-field class="col-md-6" required="true">
          <label>Apt/Suite/Building Number &amp; Street Address</label>
          <input class="form-control" type="text" formControlName="addressLine1">
        </app-form-field>
      </div>
      <div class="row">
        <app-form-field class="col-md-6" required="true">
          <label>Community</label>
          <select class="form-control" formControlName="communityOrCity">
            <option [ngValue]="null">-- Select community</option>
            <option [value]="item.id" *ngFor="let item of communities">{{item.name}}</option>
          </select>
        </app-form-field>
        <app-form-field class="col-md-3" required="true">
          <label>Province</label>
          <input class="form-control" type="text" [readonly]="true" formControlName="provinceOrState">
        </app-form-field>
        <app-form-field class="col-md-3">
          <label>Postal Code</label>
          <input class="form-control" type="text" formControlName="postalCodeOrZip">
        </app-form-field>
      </div>
      <div class="row">
        <app-form-field class="col-md-6" required="true">
          <label>Country</label>
          <input class="form-control" type="text" [readonly]="true" value="Canada">
          <input type="hidden" formControlName="country">
        </app-form-field>
      </div>
    </section>
  `,
  styles: []
})
export class BcAddressComponent implements OnInit {
  @Input() parent: FormGroup;
  @Input() communities: Community[] = [];

  ngOnInit() {
  }

  get invalid() {
    return false;
    // TODO: Implement Form VALIDATION
    //   return (
    //     this.parent.get('name').hasError('required') &&
    //     this.parent.get('name').touched
    //   );
  }
}
