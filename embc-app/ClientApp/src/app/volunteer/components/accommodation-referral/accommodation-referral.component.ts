import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AccommodationReferral } from 'src/app/core/models';
import { AccommodationRatesComponent } from 'src/app/shared/modals/accommodation-rates/accommodation-rates.component';

@Component({
  selector: 'app-accommodation-referral',
  templateUrl: './accommodation-referral.component.html',
  styleUrls: ['./accommodation-referral.component.scss']
})
export class AccommodationReferralComponent implements OnInit, OnDestroy, OnChanges {
  @Input() referral: AccommodationReferral = null;
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
    this.ratesModal = this.modals.open(AccommodationRatesComponent, { size: 'lg' });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }
}
