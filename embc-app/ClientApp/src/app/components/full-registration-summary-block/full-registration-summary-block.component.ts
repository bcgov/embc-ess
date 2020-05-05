import { Component, OnInit, Input } from '@angular/core';
import { Registration, Address, isBcAddress } from 'src/app/core/models';
import { GENDER_OPTIONS, INSURANCE_OPTIONS } from 'src/app/constants';

@Component({
  selector: 'app-full-registration-summary-block',
  templateUrl: './full-registration-summary-block.component.html',
  styleUrls: ['./full-registration-summary-block.component.scss']
})
export class FullRegistrationSummaryBlockComponent implements OnInit {
  @Input() registration: Registration = null;
  constructor() { }

  ngOnInit() {
  }

  get hasEmail(): boolean {
    return this.registration != null && this.registration.headOfHousehold.email != null;
  }

  get hasPhoneNumber(): boolean {
    return this.registration != null && this.registration.headOfHousehold.phoneNumber != null;
  }

  isBcAddress(address: Address): boolean {
    return isBcAddress(address);
  }

  genderOption(key: string) {
    const option = GENDER_OPTIONS.find(item => item.key === key);
    return option ? option.value : null;
  }

  insuranceOption(key: string) {
    const option = INSURANCE_OPTIONS.find(item => item.key === key);
    return option ? option.value : null;
  }

  yesNo(value: boolean): string {
    if (value === true) {
      return 'Yes';
    } else if (value === false) {
      return 'No';
    } else {
      return '';
    }
  }
}
