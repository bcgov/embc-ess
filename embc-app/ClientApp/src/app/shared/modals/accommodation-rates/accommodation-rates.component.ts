import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: './accommodation-rates.component.html',
  styleUrls: ['./accommodation-rates.component.scss']
})

export class AccommodationRatesComponent {

  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
