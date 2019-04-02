import { Component, OnInit, Input, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Store } from '@ngrx/store';

import { AppState } from 'src/app/store';
import { Country } from 'src/app/core/models';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-address-selector',
  template: `
    <app-bc-address *ngIf="withinBC" [parent]="parent" [touched]="touched" [communities]="communities$ | async"></app-bc-address>
    <app-other-address *ngIf="!withinBC" [parent]="parent" [touched]="touched" [countries]="countries$ | async"></app-other-address>
  `,
  styles: []
})
export class AddressSelectorComponent implements OnInit, OnChanges, OnDestroy {
  @Input() parent: FormGroup;
  @Input() withinBC = true;
  @Input() touched = false;

  communities$ = this.store.select(state => state.lookups.communities.communities);
  countries$ = this.store.select(state => state.lookups.countries.countries);
  // Find out the country ID for Canada as it is hard-coded for BC addresses...
  canada$ = this.countries$.pipe(map(countries => countries.find(x => x.name === 'Canada')));

  // convenience getter for easy access to form fields
  get f() { return this.parent.controls; }

  constructor(private store: Store<AppState>) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.withinBC != null) {
      const withinBC = changes.withinBC.currentValue;
      this.canada$.subscribe(canada => this.toggleAddressForm(withinBC, canada));
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


  private toggleAddressForm(withinBC: boolean, homeCountry: Country): void {
    const values = withinBC
      ? { province: 'British Columbia', country: homeCountry }
      : { province: null, country: null };
    this.parent.reset();
    this.parent.enable();
    this.parent.patchValue(values);
  }
}
