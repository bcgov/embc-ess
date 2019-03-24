import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';

import { Country } from 'src/app/core/models';

@Component({
  selector: 'app-other-address',
  template: `
    <section [formGroup]="parent">
      <div class="row">
        <app-form-field class="col-md-6" required="true">
          <label>Apt/Suite/Building Number &amp; Street Address</label>
          <input [class.is-invalid]="invalidAddressLine1" class="form-control" type="text" formControlName="addressLine1">
          <span *ngIf="invalidAddressLine1" class="invalid-feedback">
            Please enter your street address
          </span>
        </app-form-field>
      </div>
      <div class="row">
        <app-form-field class="col-md-6" required="true">
          <label>City</label>
          <input [class.is-invalid]="invalidCity" class="form-control" type="text" formControlName="city">
          <span *ngIf="invalidCity" class="invalid-feedback">
            Please enter your city
          </span>
        </app-form-field>
        <app-form-field class="col-md-3">
          <label>Region/Province/State</label>
          <input class="form-control" type="text" formControlName="province">
        </app-form-field>
        <app-form-field class="col-md-3">
          <label>Postal /ZIP Code</label>
          <input class="form-control" type="text" formControlName="postalCode">
        </app-form-field>
      </div>
      <div class="row">
        <app-form-field class="col-md-6" required="true">
          <label>Country/Region</label>
          <select [class.is-invalid]="invalidCountry" class="form-control" formControlName="country">
            <option [ngValue]="null">-- Select Country</option>
            <option [ngValue]="item" *ngFor="let item of countries">{{item.name}}</option>
          </select>
          <span *ngIf="invalidCountry" class="invalid-feedback">
            Please select a country from the dropdown list
          </span>
        </app-form-field>
      </div>
    </section>
  `,
  styles: []
})
export class OtherAddressComponent implements OnInit {

  @Input() parent: FormGroup;
  @Input() countries: Country[] = [];
  @Input() touched = false;

  // convenience getter for easy access to form fields
  get f() { return this.parent.controls; }

  get invalidAddressLine1() {
    return (
      this.f.addressLine1.hasError('required') &&
      this.touched
    );
  }

  get invalidCity() {
    return (
      this.f.city.hasError('required') &&
      this.touched
    );
  }

  get invalidCountry() {
    return (
      this.f.country.hasError('required') &&
      this.touched
    );
  }

  ngOnInit() {
    this.parent.controls.addressSubtype.setValue('OTAD');
    this.setupValidation();
  }

  private setupValidation(): void {
    this.f.addressLine1.setValidators([Validators.required]);
    this.f.postalCode.setValidators(null);
    this.f.community.setValidators(null);
    this.f.city.setValidators([Validators.required]);
    this.f.province.setValidators(null);
    this.f.country.setValidators([Validators.required]);
    this.parent.updateValueAndValidity();
  }
}
