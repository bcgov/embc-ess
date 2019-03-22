import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import {
  SelfRegistrationComponent,
  SelfRegistrationOneComponent,
  SelfRegistrationTwoComponent,
  SelfRegistrationThreeComponent,
  SelfRegistrationFourComponent,
  SelfRegistrationErrorComponent,
} from './self-registration';
import { VolunteerLoginComponent } from './volunteer-login/volunteer-login.component';
import { VolunteerDashboardComponent } from './volunteer-dashboard/volunteer-dashboard.component';
import { EvacueeRegistrationComponent } from './evacuee-registration/evacuee-registration.component';
import { TesterPageComponent } from './tester-page/tester-page.component';
import { EvacueeRegistrationOneComponent } from './evacuee-registration/evacuee-registration-one/evacuee-registration-one.component';
import { EvacueeRegistrationConfirmationComponent } from './evacuee-registration';
import { EssEditorComponent } from './ess-editor/ess-editor.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'self-registration',
    component: SelfRegistrationComponent,
    children: [
      {
        path: '',
        redirectTo: 'step-1',
        pathMatch: 'full'
      },
      {
        path: 'step-1',
        component: SelfRegistrationOneComponent
      },
      {
        path: 'step-2',
        component: SelfRegistrationTwoComponent
      },
      {
        path: 'step-3',
        component: SelfRegistrationThreeComponent
      },
      {
        path: 'step-4',
        component: SelfRegistrationFourComponent
      },
      {
        path: 'error',
        component: SelfRegistrationErrorComponent
      },
    ]
  },
  {
    path: 'volunteer-login',
    component: VolunteerLoginComponent
  },
  {
    path: 'volunteer-dashboard',
    component: VolunteerDashboardComponent
  },
  {
    path: 'register-evacuee',
    component: EvacueeRegistrationComponent,
    children: [
      {
        path: '',
        redirectTo: 'fill',
        pathMatch: 'full'
      },
      {
        path: 'fill/:essFileNumber',
        component: EvacueeRegistrationOneComponent
      },
      {
        path: 'fill',
        component: EvacueeRegistrationOneComponent
      },
      {
        path: 'confirmation',
        component: EvacueeRegistrationConfirmationComponent
      }
    ]
  },
  {
    path: 'user-edit',
    component: EssEditorComponent
  },
  {
    path: 'test',
    component: TesterPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
