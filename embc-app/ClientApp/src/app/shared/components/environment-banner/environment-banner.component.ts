import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { Config } from 'src/app/core/models';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import {PRODUCTION, TEST, TRAINING, DEVELOPMENT} from 'src/app/constants/environments'

@Component({
  selector: 'app-environment-banner',
  templateUrl: './environment-banner.component.html',
  styleUrls: ['./environment-banner.component.scss']
})
export class EnvironmentBannerComponent implements OnInit {

  environment: string = null; // NB: this stays null/empty in Prod
  envTitle: SafeHtml = null; // The title is expected to be an HTML string

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

  constructor(
    private store: Store<AppState>, // ngrx app state
    private sanitizer: DomSanitizer
  ) { }

  ngOnInit() {
    this.store.select(s => s.lookups.config.config).subscribe((config: Config) => {
      this.environment = config && config.environment.toLowerCase();
      this.envTitle = this.sanitizer.bypassSecurityTrustHtml(config && config.environmentTitle);
    });
  }

}
