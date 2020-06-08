import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { Config } from 'src/app/core/models';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import {PRODUCTION, TEST, TRAINING, DEVELOPMENT} from 'src/app/constants/environments'
import { Router } from '@angular/router';

@Component({
  selector: 'app-environment-banner',
  templateUrl: './environment-banner.component.html',
  styleUrls: ['./environment-banner.component.scss']
})
export class EnvironmentBannerComponent implements OnInit {

  environment: string = null; // NB: this stays null/empty in Prod
  envTitle: SafeHtml = null; // The title is expected to be an HTML string
  inSelfReg: boolean = false; // flag to determine if we're in a self-registration

  get prod(): boolean {
    return this.environment === PRODUCTION;
  }

  get prodSelfReg(): boolean {
    return this.prod && this.inSelfReg
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
    private sanitizer: DomSanitizer,
    private router: Router
  ) { 
    router.events.subscribe((val) => {
      const url: string = router.url;
      this.inSelfReg = url.includes("self-registration");
  });
  }

  ngOnInit() {
    this.store.select(s => s.lookups.config.config).subscribe((config: Config) => {
      this.environment = config && config.environment.toLowerCase();
      this.envTitle = this.sanitizer.bypassSecurityTrustHtml(config && config.environmentTitle);
    });
  }

}
