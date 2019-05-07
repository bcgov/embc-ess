import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ClothingReferral } from 'src/app/core/models';
import { ClothingRatesComponent } from 'src/app/shared/modals/clothing-rates/clothing-rates.component';

@Component({
  selector: 'app-clothing-referral',
  templateUrl: './clothing-referral.component.html',
  styleUrls: ['./clothing-referral.component.scss']
})
export class ClothingReferralComponent implements OnInit, OnDestroy, OnChanges {
  @Input() referral: ClothingReferral = null;
  @Input() editMode = false;
  @Output() remove = new EventEmitter<any>();
  @Output() add = new EventEmitter<any>();

  private ratesModal: NgbModalRef = null;

  constructor(
    private modals: NgbModal,
  ) { }

  ngOnInit() { }

  ngOnDestroy() {
    // close modal if it's open
    if (this.ratesModal) { this.ratesModal.dismiss(); }
  }

  ngOnChanges(changes: SimpleChanges) { }

  viewRates() {
    this.ratesModal = this.modals.open(ClothingRatesComponent, { size: 'lg' });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
