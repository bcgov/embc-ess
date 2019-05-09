import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';

@Component({
  selector: 'app-sidebox-add-volunteer',
  templateUrl: './sidebox-add-volunteer.component.html',
  styleUrls: ['./sidebox-add-volunteer.component.scss']
})
export class SideboxAddVolunteerComponent implements OnInit {
  route: string;

  constructor(
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    this.authService.path.subscribe(p => {
      // this is a "new" organization
      this.uniqueKeyService.clearKey();
      this.route = `/${p}/volunteer`;
    });
  }

}
