import { Component, OnInit } from '@angular/core';
import { VolunteerService } from '../core/services/volunteer.service';
import { Volunteer } from '../core/models';
import { Observable } from 'rxjs';
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
    ) { }

    ngOnInit() {
        this.volunteerService.getAllVolunteers().subscribe((v: Volunteer[]) => {
            this.volunteers = v;
        });
    }

}
