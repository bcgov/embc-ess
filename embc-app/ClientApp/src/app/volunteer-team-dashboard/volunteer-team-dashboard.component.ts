import { Component, OnInit } from '@angular/core';
import { VolunteerService } from '../core/services/volunteer.service';
import { Volunteer } from '../core/models';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { SearchQueryParameters } from '../shared/components/search';
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
        // collect all volunteers
        this.getVolunteers();
    }
    routeTo(bceidAccountNumber: string) {
        // TODO: this seems like bad practive but fix when we have time
        this.router.navigate(['volunteer-edit/fill/' + bceidAccountNumber]);
    }
    getVolunteers(query?: string, offset?: number, limit?: number, sort?: string) {
        const queryParams: SearchQueryParameters = {};
        // add all the parameters supplied to the function
        if (query) { queryParams.q = query; }
        if (offset) { queryParams.offset = offset; }
        if (limit) { queryParams.limit = limit; }
        if (sort) { queryParams.sort = sort; }

        // get volunteers with supplied params defaults defined in
        this.volunteerService.getVolunteers(queryParams).subscribe((v: Volunteer[]) => {
            this.volunteers = v;
        });
    }
}
