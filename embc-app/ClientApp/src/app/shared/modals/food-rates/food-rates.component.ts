import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { RESTAURANT_BREAKFAST, RESTAURANT_LUNCH, RESTAURANT_DINNER, GROCERIES } from 'src/app/constants/rates';
@Component({
  templateUrl: './food-rates.component.html',
  styleUrls: ['./food-rates.component.scss']
})

export class FoodRatesComponent {
  breakfast = RESTAURANT_BREAKFAST;
  lunch = RESTAURANT_LUNCH;
  dinner = RESTAURANT_DINNER;
  groceries = GROCERIES;
  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
