import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: './transportation-rates.component.html',
  styleUrls: ['./transportation-rates.component.scss']
})

export class TransportationRatesComponent {

  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
