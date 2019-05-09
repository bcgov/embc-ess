import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';

@Component({
  selector: 'app-sidebox-add-registration',
  templateUrl: './sidebox-add-registration.component.html',
  styleUrls: ['./sidebox-add-registration.component.scss']
})
export class SideboxAddRegistrationComponent implements OnInit {

  route: string;

  constructor(
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  ngOnInit() {
    this.authService.path.subscribe(p => {
      // this is a "new" organization
      this.uniqueKeyService.clearKey();
      this.route = `/${p}/registration`;
    });
  }

}
