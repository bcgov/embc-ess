import { Component, OnInit } from '@angular/core';
import { CurrentUserService } from '../core/services/current-user.service';
import { User } from '../core/models';

@Component({
  selector: 'app-top-navigation',
  templateUrl: './top-navigation.component.html',
  styleUrls: ['./top-navigation.component.scss']
})
export class TopNavigationComponent implements OnInit {

  // this is only to decide what to show.
  // all route protection should be handled in routing
  currentUser: User;

  constructor(
    private currentUserService: CurrentUserService,
  ) { }

  ngOnInit() {
    // watch the current user for changes.
    this.currentUserService.currentUser.subscribe(c => this.currentUser = c);
  }

}
