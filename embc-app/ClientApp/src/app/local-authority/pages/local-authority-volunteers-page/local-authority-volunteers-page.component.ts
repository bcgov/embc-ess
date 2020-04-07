import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  templateUrl: './local-authority-volunteers-page.component.html',
  styleUrls: ['./local-authority-volunteers-page.component.scss']
})
export class LocalAuthorityVolunteersPageComponent implements OnInit {
  route: string = "/";
  constructor(private authService: AuthService) { }
  
  ngOnInit(): void {
    this.authService.path.subscribe((path: string) => {
      this.route = `/${path}/volunteer`;
    });
  }

  

}
