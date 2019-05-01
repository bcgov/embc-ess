import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: './clothing-rates.component.html',
  styleUrls: ['./clothing-rates.component.scss']
})

export class ClothingRatesComponent {

  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
