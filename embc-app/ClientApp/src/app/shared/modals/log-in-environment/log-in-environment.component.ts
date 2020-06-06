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
  envTitle: string;
	body: SafeHtml;

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
      this.envTitle = config && config.environmentTitle;
      this.buildHTML();
    });
  }

  private buildHTML() {
    let html: string = null;
    if (this.prod) {
      html = `<strong>Live</strong> Evacuee Registration & Assistance (ERA) Tool
              <br/>
              <small>
              You are entering the <strong>LIVE</strong> version of the <strong>Evacuee Registration & Assitance Tool</strong>.
              <br/>
              All information here will be treated as <strong>real</strong> and <strong>accurate</strong>.
              </small>`;
    }
    else if (this.training) {
      html = `<strong>Training</strong> Evacuee Registration & Assistance (ERA) Tool
      <br/>
      <small>
      You are entering the <strong>TRAINING</strong> version of the <strong>Evacuee Registration & Assitance Tool</strong>.
      <br/>
      All information here will be treated as <strong>training</strong> data.
      </small>`;
    }
    else {
      // we only care about prod and training - just reuse the environment text for dev and testing
      html = this.envTitle;
    }
    this.body = this.sanitizer.bypassSecurityTrustHtml(html);
  }


  close() {
    this.activeModal.dismiss();
  }

}
