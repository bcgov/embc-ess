import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { INCIDENTALS_MAX_PER_PERSON } from 'app/constants/rates';
@Component({
  templateUrl: './incidentals-rates.component.html',
  styleUrls: ['./incidentals-rates.component.scss']
})

export class IncidentalsRatesComponent {
  maxIncidentals = INCIDENTALS_MAX_PER_PERSON;
  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
