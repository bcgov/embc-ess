import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncidentalsComponent } from './incidentals/incidentals.component';

@NgModule({
  declarations: [IncidentalsComponent],
  imports: [
    CommonModule
  ],
  exports: [
    IncidentalsComponent, // export to build in test page
  ]
})
export class NeedsAssessmentModule { }
