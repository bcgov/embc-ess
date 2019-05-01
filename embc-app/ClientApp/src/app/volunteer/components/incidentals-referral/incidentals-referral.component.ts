import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Registration, IncidentalsReferral } from 'src/app/core/models';
import { IncidentalsRatesComponent } from 'src/app/shared/modals/incidentals-rates/incidentals-rates.component';

@Component({
  selector: 'app-incidentals-referral',
  templateUrl: './incidentals-referral.component.html',
  styleUrls: ['./incidentals-referral.component.scss']
})
export class IncidentalsReferralComponent implements OnInit, OnDestroy, OnChanges {
  @Input() registration: Registration = null;
  @Input() referral: IncidentalsReferral = null;
  @Output() remove = new EventEmitter<any>();
  @Output() add = new EventEmitter<any>();

  private incidentalsRatesModal: NgbModalRef = null;

  constructor(
    private modals: NgbModal,
  ) { }

  ngOnInit() { }

  ngOnDestroy() {
    // close modal if it's open
    if (this.incidentalsRatesModal) { this.incidentalsRatesModal.dismiss(); }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.registration) {
      console.log('registration =', changes.registration);
    }
    if (changes.referral) {
      console.log('referral =', changes.referral);
    }
  }

  viewIncidentalsRates() {
    this.incidentalsRatesModal = this.modals.open(IncidentalsRatesComponent, { size: 'lg' });
    this.incidentalsRatesModal.result.then(
      () => { this.incidentalsRatesModal = null; },
      () => { this.incidentalsRatesModal = null; }
    );
  }
}
