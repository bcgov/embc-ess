import { Component, OnInit } from '@angular/core';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-sidebox-add-volunteer',
  templateUrl: './sidebox-add-volunteer.component.html',
  styleUrls: ['./sidebox-add-volunteer.component.scss']
})
export class SideboxAddVolunteerComponent implements OnInit {

  route: string;

  constructor(
    private authService: AuthService,
  ) { }

  ngOnInit() {
    this.authService.path.subscribe((path: string) => {
      this.route = `/${path}/volunteer`;
    });
  }

}
