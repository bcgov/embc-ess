import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: './lodging-rates.component.html',
  styleUrls: ['./lodging-rates.component.scss']
})

export class LodgingRatesComponent {

  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
