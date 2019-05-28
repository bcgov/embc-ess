import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/core/models';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Input() currentUser: User;

  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
  }

  homeButton() {
    this.authService.role.subscribe(r => {
      // if the role is null we redirect them to a logged out home location otherwise they go to their dash.
      !r ? this.router.navigate(['dashboard']) : this.router.navigate(['']);
    });
  }
}
