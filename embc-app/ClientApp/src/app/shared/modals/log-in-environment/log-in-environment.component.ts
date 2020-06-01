import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { Config } from 'src/app/core/models';
import {PRODUCTION, TEST, TRAINING, DEVELOPMENT} from 'src/app/constants/environments'
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-log-in-environment',
  templateUrl: './log-in-environment.component.html',
  styleUrls: ['./log-in-environment.component.scss']
})
export class LogInEnvironmentComponent implements OnInit {

  environment: string;
  envTitle: SafeHtml;

  constructor(private activeModal: NgbActiveModal,
              private store: Store<AppState>,
              private sanitizer: DomSanitizer,
              private router: Router) { }

  get prod(): boolean {
    return this.environment === PRODUCTION;
  }

  get training(): boolean {
    return this.environment === TRAINING;
  }

  get testing(): boolean {
    return this.environment === TEST;
  }

  get dev(): boolean {
    return this.environment === DEVELOPMENT;
  }

  ngOnInit() {
    this.store.select(s => s.lookups.config.config).subscribe((config: Config) => {
      this.environment = config && config.environment.toLowerCase();
      this.envTitle = this.sanitizer.bypassSecurityTrustHtml(config && config.environmentTitle);
    });
  }

  close() {
    this.activeModal.dismiss();
  }

}
