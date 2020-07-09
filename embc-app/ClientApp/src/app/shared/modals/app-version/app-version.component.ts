import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppVersion } from 'src/app/core/models/app-version.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { EVERYONE } from 'src/app/constants';

@Component({
  templateUrl: './app-version.component.html',
  styleUrls: ['./app-version.component.scss']
})

export class AppVersionComponent implements OnInit {
  @Input() version: AppVersion;
  userIsAuthenticated: boolean = false;
  constructor(
    public activeModal: NgbActiveModal,
    private auth: AuthService
  ) { }

    ngOnInit() {
      this.auth.isEveryone$.subscribe(e => this.userIsAuthenticated = !e);
    }

}
