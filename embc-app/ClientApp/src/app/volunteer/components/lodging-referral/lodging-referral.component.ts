import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { LodgingReferral } from 'src/app/core/models';
import { LodgingRatesComponent } from 'src/app/shared/modals/lodging-rates/lodging-rates.component';

@Component({
  selector: 'app-lodging-referral',
  templateUrl: './lodging-referral.component.html',
  styleUrls: ['./lodging-referral.component.scss']
})
export class LodgingReferralComponent implements OnInit, OnDestroy, OnChanges {
  @Input() referral: LodgingReferral = null;
  @Input() readOnly = false;
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
    this.ratesModal = this.modals.open(LodgingRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
