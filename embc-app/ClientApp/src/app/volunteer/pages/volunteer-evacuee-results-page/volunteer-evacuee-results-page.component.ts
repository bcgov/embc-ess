import { Component, OnInit } from '@angular/core';
import { EvacueeService } from 'src/app/core/services/evacuee.service';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-volunteer-evacuee-results-page',
  templateUrl: './volunteer-evacuee-results-page.component.html',
  styleUrls: ['./volunteer-evacuee-results-page.component.scss']
})
export class VolunteerEvacueeResultsPageComponent implements OnInit {

  path:string = null;

  constructor(
    private evacService: EvacueeService,
    private router: Router,
    private authService: AuthService,
    ) { }

  ngOnInit() {
    // If end up here but have no results to display (e.g. browser refresh on results), route back to dashboard
    // this.authService.path.subscribe((path: string) => {
    // this.path = path;
    // if (!this.evacService.hasPreviousQuery()) {
    //   this.router.navigate([`/${this.path}/registrations`]);
    // }
    // });
  }

}
