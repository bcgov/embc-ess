import { Component, OnInit, OnDestroy, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { Registration, IncidentalsReferral } from 'src/app/core/models';

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

  constructor() { }

  ngOnInit() { }

  ngOnDestroy() { }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.registration) {
      console.log('registration =', changes.registration);
    }
    if (changes.referral) {
      console.log('referral =', changes.referral);
    }
  }

  viewRates() {
    console.log('you clicked the View Rates button');
    // TODO: display modal with incidentals rates sheet template
  }
}
