import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: './food-rates.component.html',
  styleUrls: ['./food-rates.component.scss']
})

export class FoodRatesComponent {

  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
