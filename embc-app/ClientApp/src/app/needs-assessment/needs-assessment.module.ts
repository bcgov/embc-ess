import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncidentalsComponent } from './incidentals/incidentals.component';
// import { FontAwesomeLinkComponent } from '../shared/components/fa-link/fa-link.component';

@NgModule({
  declarations: [IncidentalsComponent],
  imports: [
    // FontAwesomeLinkComponent,
    CommonModule,
  ],
  exports: [
    IncidentalsComponent, // export to build in test page
  ]
})
export class NeedsAssessmentModule { }
