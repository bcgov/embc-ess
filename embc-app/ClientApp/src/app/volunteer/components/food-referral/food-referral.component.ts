import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { FoodReferral } from 'src/app/core/models';
import { FoodRatesComponent } from 'src/app/shared/modals/food-rates/food-rates.component';

@Component({
  selector: 'app-food-referral',
  templateUrl: './food-referral.component.html',
  styleUrls: ['./food-referral.component.scss']
})
export class FoodReferralComponent implements OnInit, OnDestroy, OnChanges {
  @Input() referral: FoodReferral = null;
  @Input() readOnly = false;
  @Output() remove = new EventEmitter<any>();
  @Output() add = new EventEmitter<any>();

  private ratesModal: NgbModalRef = null;
  uuid: string;

  constructor(
    private modals: NgbModal,
  ) { }

  ngOnInit() {
    // for the purpose of accesibility this number is likely unique
    // if it breaks and isn't unique it won't break the form. (poor man's guid)
    this.uuid = new Date().valueOf().toString();
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.ratesModal) { this.ratesModal.dismiss(); }
  }

  ngOnChanges(changes: SimpleChanges) { }

  viewRates() {
    this.ratesModal = this.modals.open(FoodRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
