import { Component, OnInit } from '@angular/core';
import { Volunteer } from 'src/app/core/models';
import { AppState } from 'src/app/store';
import { Store } from '@ngrx/store';
import { VolunteerService } from 'src/app/core/services/volunteer.service';
import { takeWhile } from 'rxjs/operators';
import { UpdateVolunteer } from 'src/app/store/volunteer/volunteer.actions';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-volunteer-editor-confirmation',
  templateUrl: './volunteer-editor-confirmation.component.html',
  styleUrls: ['./volunteer-editor-confirmation.component.scss']
})
export class VolunteerEditorConfirmationComponent implements OnInit {
  componentActive = true;
  currentVolunteer$ = this.store.select(s => s.volunteers.currentVolunteer);
  volunteer: Volunteer;

  constructor(
    private store: Store<AppState>,
    private volunteerService: VolunteerService,
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.currentVolunteer$.pipe(takeWhile(() => this.componentActive))
      .subscribe(v => {
        this.volunteer = v;
      });
  }
  back() {
    this.onSave();
    const nextRoute = this.volunteer.id ? `../fill/${this.volunteer.id}` : '../fill';
    this.router.navigate([nextRoute], { relativeTo: this.route });
  }
  onSave() {
    // update the stored version
    const volunteer = this.volunteer;
    this.store.dispatch(new UpdateVolunteer({ volunteer }));
  }
  submit(addAnother?: boolean) {
    // the page should have all information in this page. if not this was routed to by mistake.
    if (!(this.volunteer.lastName && this.volunteer.firstName && this.volunteer.bceidAccountNumber && this.volunteer.canAccessRestrictedFiles != null)) {
      this.router.navigate(['../../volunteers'], { relativeTo: this.route });
    } else {
      // the information required was passed in from the state proceed with update or create

      // check if this is an update
      if (this.volunteer.id) {
        // if the volunteer has an ID we need to update
        this.volunteerService.updateVolunteer(this.volunteer)
          .subscribe(() => {
            // if addAnother route back to the add page else route back to the volunteer-team-editor
            if (addAnother) {
              this.router.navigate(['../fill'], { relativeTo: this.route });
            } else {
              // go back to the volunteer team dashboard
              this.router.navigate(['../../volunteers'], { relativeTo: this.route });
            }
          });
      } else {
        // if the volunteer has no id we need to create a new one
        this.volunteerService.createVolunteer(this.volunteer)
          .subscribe(v => {
            // if addAnother route back to the add page else route back to the volunteer-team-editor
            if (addAnother) {
              this.router.navigate(['../fill'], { relativeTo: this.route });
            } else {
              // go back to the volunteer team dashboard
              this.router.navigate(['../../volunteers'], { relativeTo: this.route });
            }
          });
      }
    }
  }
}
