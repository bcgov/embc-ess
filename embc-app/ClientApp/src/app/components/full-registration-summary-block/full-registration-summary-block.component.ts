import { Component, OnInit, Input } from '@angular/core';
import { Registration, Address, isBcAddress } from 'app/core/models';
import { GENDER_OPTIONS, INSURANCE_OPTIONS } from 'app/constants';

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
