import { Component, OnInit } from '@angular/core';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-sidebox-add-registration',
  templateUrl: './sidebox-add-registration.component.html',
  styleUrls: ['./sidebox-add-registration.component.scss']
})
export class SideboxAddRegistrationComponent implements OnInit {

  route: string;

  constructor(
    private authService: AuthService,
  ) { }

  ngOnInit() {
    this.authService.path.subscribe((path: string) => {
      this.route = `/${path}/registration`;
    });
  }

}
