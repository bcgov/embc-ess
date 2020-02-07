import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CLOTHING_PER_PERSON, CLOTHING_PER_PERSON_WINTER_CONDITIONS } from 'src/app/constants/rates';

@Component({
  templateUrl: './clothing-rates.component.html',
  styleUrls: ['./clothing-rates.component.scss']
})

export class ClothingRatesComponent {
  clothing = CLOTHING_PER_PERSON;
  clothingWinter = CLOTHING_PER_PERSON_WINTER_CONDITIONS;
  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
