import { Component, OnInit, Input, Directive } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';

import { Community } from 'src/app/core/models';
import { CustomValidators } from 'src/app/shared/validation/custom.validators';
import { compareById } from 'src/app/shared/utils';

@Component({
  selector: 'app-bc-address',
  template: `
    <section [formGroup]="parent">
      <div class="row">
        <app-form-field class="col-md-6" required="true">
          <label>Apt/Suite/Building Number &amp; Street Address/PO Box</label>
          <input [class.is-invalid]="invalidAddressLine1" class="form-control" type="text" formControlName="addressLine1">
          <span class="invalid-feedback">
            Please enter your street address.
          </span>
        </app-form-field>
      </div>
      <div class="row">
        <app-form-field class="col-md-6" required="true">
          <label>Community</label>
          <select [compareWith]="compareById" [class.is-invalid]="invalidCommunity" class="form-control" formControlName="community">
            <option [ngValue]="null">-- Select community</option>
            <option [ngValue]="item" *ngFor="let item of communities">{{item.name}}</option>
          </select>
          <span class="invalid-feedback">
            Please enter your community.
          </span>
        </app-form-field>
        <app-form-field class="col-md-3" required="true">
          <label>Province</label>
          <input class="form-control" type="text" [readonly]="true" formControlName="province">
        </app-form-field>
        <app-form-field class="col-md-3">
          <label>Postal Code</label>
          <input [class.is-invalid]="invalidPostalCode" class="form-control" type="text" formControlName="postalCode">
          <small>(Format: a1a 1a1)</small>
          <span class="invalid-feedback">
            Please enter a valid postal code.
          </span>
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
  @Input() touched = false;

  // convenience getter so we can use helper functions within Angular templates
  compareById = compareById;

  // convenience getter for easy access to form fields
  get f() { return this.parent.controls; }

  get invalidAddressLine1() { return this.f.addressLine1.hasError('required') && this.touched; }
  get invalidCommunity() { return this.f.community.hasError('required') && this.touched; }
  get invalidPostalCode() { return this.f.postalCode.hasError('postalCodeCanada') && this.touched; }

  ngOnInit() {
    this.parent.controls.addressSubtype.setValue('BCAD');
    this.setupValidation();
  }

  private setupValidation(): void {
    this.f.addressLine1.setValidators([Validators.required]);
    this.f.addressLine1.updateValueAndValidity();

    this.f.postalCode.setValidators([CustomValidators.postalCodeCanada]);
    this.f.postalCode.updateValueAndValidity();

    this.f.community.setValidators([Validators.required]);
    this.f.community.updateValueAndValidity();

    this.f.city.clearValidators();
    this.f.city.updateValueAndValidity();

    this.f.province.setValidators([Validators.required]);
    this.f.province.updateValueAndValidity();

    this.f.country.setValidators([Validators.required]);
    this.f.country.updateValueAndValidity();

    this.parent.updateValueAndValidity();
  }
}
