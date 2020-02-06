import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { BILLETING_NIGHTLY, BILLETING_ADDED_ADULT, BILLETING_ADDED_CHILD } from 'app/constants/rates';
@Component({
  templateUrl: './lodging-rates.component.html',
  styleUrls: ['./lodging-rates.component.scss']
})

export class LodgingRatesComponent {
  nightly = BILLETING_NIGHTLY;
  adult = BILLETING_ADDED_ADULT;
  child = BILLETING_ADDED_CHILD;
  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
