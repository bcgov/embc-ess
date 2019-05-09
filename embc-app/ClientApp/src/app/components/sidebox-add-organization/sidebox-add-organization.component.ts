import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';

@Component({
  selector: 'app-sidebox-add-organization',
  templateUrl: './sidebox-add-organization.component.html',
  styleUrls: ['./sidebox-add-organization.component.scss']
})
export class SideboxAddOrganizationComponent implements OnInit {

  route: string;

  // TODO: THIS MUST BE IMPORTED AND MODIFIED BEFORE USE
  constructor(
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    this.authService.path.subscribe(p => {
      // this is a "new" organization
      this.uniqueKeyService.clearKey();
      this.route = `/${p}/organization`;
    });
  }

}
