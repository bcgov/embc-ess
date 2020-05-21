import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store';
import { Config } from 'src/app/core/models';
import {PRODUCTION, TEST, TRAINING, DEVELOPMENT} from 'src/app/constants/environments'

@Component({
  selector: 'app-log-in-environment',
  templateUrl: './log-in-environment.component.html',
  styleUrls: ['./log-in-environment.component.scss']
})
export class LogInEnvironmentComponent implements OnInit {

  config: Config;

  constructor(private activeModal: NgbActiveModal,
              private store: Store<AppState>) { }

  ngOnInit() {
    this.store.select(s => s.lookups.config.config).subscribe((conf: Config) => {
      this.config = conf;
    });
  }

  

}
