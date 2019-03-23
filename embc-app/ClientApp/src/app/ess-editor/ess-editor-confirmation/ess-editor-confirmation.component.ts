import { Component, OnInit } from '@angular/core';
import { Volunteer } from 'src/app/core/models';
import { AppState } from 'src/app/store';
import { Store } from '@ngrx/store';
import { VolunteerService } from 'src/app/core/services/volunteer.service';
import { takeWhile } from 'rxjs/operators';

@Component({
  selector: 'app-ess-editor-confirmation',
  templateUrl: './ess-editor-confirmation.component.html',
  styleUrls: ['./ess-editor-confirmation.component.scss']
})
export class EssEditorConfirmationComponent implements OnInit {
  componentActive = true;
  currentVolunteer$ = this.store.select(s => s.volunteers.currentVolunteer);
  volunteer: Volunteer;


  constructor(
    private store: Store<AppState>,
    private volunteerService: VolunteerService
  ) { }

  ngOnInit() {
    this.currentVolunteer$.pipe(takeWhile(() => this.componentActive))
      .subscribe(v => this.volunteer = v);
  }

  submit() {
    if (this.volunteer.id) {
      // if the volunteer has an ID we need to update
      this.volunteerService.updateVolunteer(this.volunteer)
        .subscribe(v => {
          alert(JSON.stringify(v));
        });
    } else {
      // if the volunteer has no id we need to create a new one
      this.volunteerService.createVolunteer(this.volunteer)
        .subscribe(v => {
          alert(JSON.stringify(v));
        });
    }
  }
}
