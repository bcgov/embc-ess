import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IncidentalsComponent } from './components/incidentals/incidentals.component';

@NgModule({
  declarations: [
    IncidentalsComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    IncidentalsComponent, // needed to use component in test page
  ]
})
export class VolunteerModule { }
