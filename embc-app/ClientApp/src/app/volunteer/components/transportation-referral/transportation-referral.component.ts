import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TransportationReferral } from 'src/app/core/models';
import { TransportationRatesComponent } from 'src/app/shared/modals/transportation-rates/transportation-rates.component';

@Component({
  selector: 'app-transportation-referral',
  templateUrl: './transportation-referral.component.html',
  styleUrls: ['./transportation-referral.component.scss']
})
export class TransportationReferralComponent implements OnInit, OnDestroy, OnChanges {
  @Input() referral: TransportationReferral = null;
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
    this.ratesModal = this.modals.open(TransportationRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
