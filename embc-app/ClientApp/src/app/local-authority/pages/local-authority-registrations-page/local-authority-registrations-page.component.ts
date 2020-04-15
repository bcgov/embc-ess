import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-local-authority-registrations-page',
  templateUrl: './local-authority-registrations-page.component.html',
  styleUrls: ['./local-authority-registrations-page.component.scss']
})
export class LocalAuthorityRegistrationsPageComponent implements OnInit {
  route: string;
  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.path.subscribe((path: string) => {
      this.route = `/${path}/volunteer`;
    });
  }

}
