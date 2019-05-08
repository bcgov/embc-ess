import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { IncidentalsReferral } from 'src/app/core/models';
import { IncidentalsRatesComponent } from 'src/app/shared/modals/incidentals-rates/incidentals-rates.component';

@Component({
  selector: 'app-incidentals-referral',
  templateUrl: './incidentals-referral.component.html',
  styleUrls: ['./incidentals-referral.component.scss']
})
export class IncidentalsReferralComponent implements OnInit, OnDestroy, OnChanges {
  @Input() referral: IncidentalsReferral = null;
  @Input() readOnly = false;
  @Output() remove = new EventEmitter<any>();
  @Output() add = new EventEmitter<any>();

  private ratesModal: NgbModalRef = null;
  uuid: string;

  constructor(
    private modals: NgbModal,
  ) { }

  ngOnInit() {
    // for the purpose of accessibility this number is likely unique
    // if it breaks and isn't unique it won't break the form. (poor man's guid)
    this.uuid = new Date().valueOf().toString();
  }

  ngOnDestroy() {
    // close modal if it's open
    if (this.ratesModal) { this.ratesModal.dismiss(); }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.referral) {
      // console.log('referral =', changes.referral.currentValue);
    }
    if (changes.readOnly) {
      // console.log('readOnly =', changes.readOnly.currentValue);
    }
  }

  viewRates() {
    this.ratesModal = this.modals.open(IncidentalsRatesComponent, { size: 'lg', centered: true });
    this.ratesModal.result.then(
      () => { this.ratesModal = null; },
      () => { this.ratesModal = null; }
    );
  }

  // --------------------HELPERS-----------------------------------------
  numDays(validFrom: Date, validTo: Date) { return numberOfDays(validFrom, validTo); }
}
