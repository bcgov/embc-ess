import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { IncidentalsReferral } from 'src/app/core/models';
import { IncidentalsRatesComponent } from 'src/app/shared/modals/incidentals-rates/incidentals-rates.component';
import * as moment from 'moment';
import { ReferralDate } from 'src/app/core/models/referral-date';

@Component({
  selector: 'app-incidentals-referral',
  templateUrl: './incidentals-referral.component.html',
  styleUrls: ['./incidentals-referral.component.scss']
})
export class IncidentalsReferralComponent implements OnInit, OnDestroy, OnChanges {
  @Input() referral: IncidentalsReferral = null;
  @Input() editMode = false;
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
    if (changes.referral) {
      console.log('referral =', changes.referral.currentValue);
    }
    if (changes.editMode) {
      console.log('editMode =', changes.editMode.currentValue);
    }
  }

  viewIncidentalsRates() {
    this.incidentalsRatesModal = this.modals.open(IncidentalsRatesComponent, { size: 'lg' });
    this.incidentalsRatesModal.result.then(
      () => { this.incidentalsRatesModal = null; },
      () => { this.incidentalsRatesModal = null; }
    );
  }

  // TODO: move this to some shared util
  numDays(validFrom: Date, validTo: Date): number {
    const a = moment(validFrom);
    const b = moment(validTo);
    return b.diff(a, 'days') + 1; // TODO: verify this
  }
  onDate(event: ReferralDate) {
    // collect the referral date from the event
    this.referral.dates = event;
  }
}
