import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/core/models';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Input() currentUser: User;

  constructor(
    private router: Router,
  ) { }

  ngOnInit() {
  }

  homeButton(){
    // TODO: the home button should be a routerlink but because of the way the routing works doing a standard routerlink makes angular confused. Need to fix but deadline.
    this.router.navigate(['/']);
  }
}
