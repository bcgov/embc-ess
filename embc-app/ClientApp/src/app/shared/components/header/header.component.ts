import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/core/models';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Input() currentUser: User;

  constructor() { }

  ngOnInit() {
  }

}
