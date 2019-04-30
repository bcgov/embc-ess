import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: './incidentals-rates.component.html',
  styleUrls: ['./incidentals-rates.component.scss']
})

export class IncidentalsRatesComponent {

  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
