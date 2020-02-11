import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppVersion } from 'src/app/core/models/app-version.model';

@Component({
  templateUrl: './app-version.component.html',
  styleUrls: ['./app-version.component.scss']
})

export class AppVersionComponent {
  @Input() version: AppVersion;
  constructor(
    public activeModal: NgbActiveModal
  ) { }

}
