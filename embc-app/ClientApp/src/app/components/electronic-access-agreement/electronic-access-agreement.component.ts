import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-electronic-access-agreement',
  templateUrl: './electronic-access-agreement.component.html',
  styleUrls: ['./electronic-access-agreement.component.scss']
})
export class ElectronicAccessAgreementComponent implements OnInit {
  @Output() termsAccepted = new EventEmitter<boolean>();
  window = window;
  _termsAccepted: boolean;

  constructor() { }

  ngOnInit() {
  }
}
