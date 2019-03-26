import { Component, OnInit } from '@angular/core';
import { VolunteerService } from '../core/services/volunteer.service';
import { Volunteer } from '../core/models';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
// import { }
@Component({
    selector: 'app-volunteer-team-dashboard',
    templateUrl: './volunteer-team-dashboard.component.html',
    styleUrls: ['./volunteer-team-dashboard.component.scss']
})
export class VolunteerTeamDashboardComponent implements OnInit {
    // simple server response
    volunteers: Volunteer[];

    constructor(
        private volunteerService: VolunteerService,
        private router: Router
    ) { }

    ngOnInit() {
        this.volunteerService.getAllVolunteers().subscribe((v: Volunteer[]) => {
            this.volunteers = v;
        });
    }
    routeTo(bceidAccountNumber: string) {
        // TODO: this seems like bad practive but fix when we have time
        this.router.navigate(['volunteer-edit/fill/' + bceidAccountNumber]);
    }
}
