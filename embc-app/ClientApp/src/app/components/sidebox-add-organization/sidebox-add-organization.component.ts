import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-sidebox-add-organization',
  templateUrl: './sidebox-add-organization.component.html',
  styleUrls: ['./sidebox-add-organization.component.scss']
})
export class SideboxAddOrganizationComponent implements OnInit {

  route: string;

  // TODO: THIS MUST BE IMPORTED AND MODIFIED BEFORE USE
  constructor(
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.authService.path.subscribe(p => {
      this.route = `/${p}/organization`;
    });
  }

}
