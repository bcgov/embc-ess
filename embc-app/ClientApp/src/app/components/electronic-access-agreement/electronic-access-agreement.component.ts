import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, switchMap } from 'rxjs/operators';
import { VolunteerService } from '@app/core/services/volunteer.service';
import { AuthService } from 'app/core/services/auth.service';
import { User, Volunteer } from 'app/core/models';

@Component({
  selector: 'app-electronic-access-agreement',
  templateUrl: './electronic-access-agreement.component.html',
  styleUrls: ['./electronic-access-agreement.component.scss']
})
export class ElectronicAccessAgreementComponent implements OnInit {
  @Output() termsAccepted = new EventEmitter<boolean>();
  window = window;
  _termsAccepted: boolean;
  redirectUrl: string;
  volunteer: Volunteer;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private volunteerService: VolunteerService) {
    this.route.queryParams
      .pipe(filter(params => params.redirectUrl))
      .subscribe(params => {
        this.redirectUrl = params.redirectUrl;
      });

    this.authService.getCurrentUser()
      .pipe(switchMap((user: User) => {
        return this.volunteerService.getVolunteerById(user.contactid)
      }))
      .subscribe((volunteer: Volunteer) => {
        this.volunteer = volunteer;
      });

  }

  ngOnInit() {
  }

  acceptTerms() {
    const data = <Volunteer>{ ...this.volunteer, electronicAccessAgreementAccepted: true };
    this.volunteerService.updateVolunteer(data)
      .subscribe(res => {
        this.router.navigateByUrl(this.redirectUrl);
      });
  }
}
