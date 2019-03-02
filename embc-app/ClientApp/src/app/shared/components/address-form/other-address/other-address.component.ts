import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { Country } from 'src/app/core/models';

@Component({
  selector: 'app-other-address',
  template: `
    <section [formGroup]="parent">
      <div class="row">
        <app-form-field class="col-md-8" required="true">
          <label>Apt/Suite/Building Number &amp; Street Address</label>
          <input class="form-control" type="text" formControlName="addressLine1">
        </app-form-field>
      </div>
      <div class="row">
        <app-form-field class="col-md-6">
          <label>City</label>
          <input class="form-control" type="text" formControlName="communityOrCity">
        </app-form-field>
        <app-form-field class="col-md-3">
          <label>Region/Province/State</label>
          <input class="form-control" type="text" formControlName="provinceOrState">
        </app-form-field>
        <app-form-field class="col-md-3">
          <label>Postal /ZIP Code</label>
          <input class="form-control" type="text" formControlName="postalCodeOrZip">
        </app-form-field>
      </div>
      <div class="row">
        <app-form-field class="col-md-3">
          <label>Country/Region</label>
          <select class="form-control" formControlName="country">
            <option [ngValue]="null">-- Select Country</option>
            <option [value]="item.id" *ngFor="let item of countries">{{item.name}}</option>
          </select>
        </app-form-field>
      </div>
    </section>
  `,
  styles: []
})
export class OtherAddressComponent implements OnInit {

  @Input() parent: FormGroup;
  @Input() countries: Country[] = [];

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
