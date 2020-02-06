import { Component, OnInit, Input, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';

import { AppState } from 'app/store';
import { Country } from 'app/core/models';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-address-selector',
  template: `
    <app-bc-address *ngIf="withinBC" [parent]="parent" [touched]="touched"></app-bc-address>
    <app-other-address *ngIf="!withinBC" [parent]="parent" [touched]="touched" [countries]="countries$ | async"></app-other-address>
  `,
  styles: []
})
export class AddressSelectorComponent implements OnInit, OnChanges, OnDestroy {

  @Input() parent: FormGroup;
  @Input() withinBC = true;
  @Input() touched = false;

  countries$ = this.store.select(state => state.lookups.countries.countries.sort((a, b) => {
    if (b.name === 'Canada' || b.name === 'United States of America') {
      return 1;
    } else if (a.name === 'Canada' || a.name === 'United States of America') {
      return -1;
    } else {
      return 0;
    }
  }));

  // find out the country ID for Canada as it is hard-coded for BC addresses
  canada$ = this.countries$.pipe(map(countries => countries.find(x => x.name === 'Canada')));

  // convenience getter for easy access to form fields
  get f() { return this.parent.controls; }

  constructor(private store: Store<AppState>) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    const toggle = changes.withinBC;
    if (toggle != null) {
      const value = toggle.currentValue;
      const previous = toggle.previousValue;

      // reset the form ONLY when toggling between the two states, not upon initial loading of the component
      const shouldReset = (value !== previous && !toggle.isFirstChange());

      // look up the home country for BC addresses
      // then toggle the form between BC and non-BC addresses
      this.canada$.subscribe((country: Country) => this.toggleAddressForm(value, country, shouldReset));
    }
  }

  ngOnDestroy(): void {
    this.f.addressLine1.clearValidators();
    this.f.postalCode.clearValidators();
    this.f.community.clearValidators();
    this.f.city.clearValidators();
    this.f.province.clearValidators();
    this.f.country.clearValidators();

    this.f.addressLine1.updateValueAndValidity();
    this.f.postalCode.updateValueAndValidity();
    this.f.community.updateValueAndValidity();
    this.f.city.updateValueAndValidity();
    this.f.province.updateValueAndValidity();
    this.f.country.updateValueAndValidity();

    this.parent.updateValueAndValidity();
  }

  private toggleAddressForm(withinBC: boolean, homeCountry: Country, reset = false): void {
    const values = withinBC ? { province: 'British Columbia', country: homeCountry } : {};
    if (reset) {
      this.parent.reset();
    }
    this.parent.enable();
    this.parent.patchValue(values);
  }

}
