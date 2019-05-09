import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';

@Component({
  selector: 'app-environment-banner',
  templateUrl: './environment-banner.component.html',
  styleUrls: ['./environment-banner.component.scss']
})
export class EnvironmentBannerComponent implements OnInit {

  environment: string = null; // NB: this stays null/empty in Prod

  constructor(
    private store: Store<AppState>, // ngrx app state
  ) { }

  ngOnInit() {
    this.store.select(s => s.lookups.config.config).subscribe(config => {
      this.environment = config && config.environment;
    });
  }

}
