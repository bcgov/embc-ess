import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-sidebox-add-registration',
  templateUrl: './sidebox-add-registration.component.html',
  styleUrls: ['./sidebox-add-registration.component.scss']
})
export class SideboxAddRegistrationComponent implements OnInit {

  route = '../registration';

  constructor() { }

  ngOnInit() {
  }

}
