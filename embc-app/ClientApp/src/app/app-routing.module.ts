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
import { VolunteerEditorComponent } from './volunteer-editor/volunteer-editor.component';
import { VolunteerEditorOneComponent } from './volunteer-editor/volunteer-editor-one/volunteer-editor-one.component';
import { VolunteerEditorConfirmationComponent } from './volunteer-editor/volunteer-editor-confirmation/volunteer-editor-confirmation.component';
import { VolunteerTeamDashboardComponent } from './volunteer-team-dashboard/volunteer-team-dashboard.component';
import { VolunteerUsefulInformationComponent } from './volunteer-useful-information/volunteer-useful-information.component';
import { EvacueeSummaryComponent } from './evacuee-summary/evacuee-summary.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { AdminAddTaskNumberComponent } from './admin-add-task-number/admin-add-task-number.component';
import { AdminAddTaskNumberOneComponent, AdminAddTaskNumberConfirmationComponent } from './admin-add-task-number';
import { AdminTaskNumbersComponent, AdminEvacueesComponent, AdminOrganizationsComponent } from './admin-dashboard';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    // TODO: naming this should be "evacuee-self-registration"
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
        path: 'step-4/:id',
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
    path: 'volunteer-info',
    component: VolunteerUsefulInformationComponent
  },
  {
    path: 'volunteer-dashboard',
    component: VolunteerDashboardComponent
  },
  {
    path: 'volunteer-team-dashboard',
    component: VolunteerTeamDashboardComponent
  },
  {
    path: 'admin-dashboard',
    component: AdminDashboardComponent,
    children: [
      {
        path: '',
        redirectTo: 'evacuees',
        pathMatch: 'full'
      },
      {
        path: 'task-numbers',
        component: AdminTaskNumbersComponent
      },
      {
        path: 'evacuees',
        component: AdminEvacueesComponent
      },
      {
        path: 'organizations',
        component: AdminOrganizationsComponent
      },
    ]
  },
  {
    path: 'add-task-number',
    component: AdminAddTaskNumberComponent,
    children: [
      {
        path: '',
        redirectTo: 'fill',
        pathMatch: 'full'
      },
      {
        path: 'fill',
        component: AdminAddTaskNumberOneComponent
      },
      {
        path: 'fill/:id',
        component: AdminAddTaskNumberOneComponent
      },
      {
        path: 'confirmation',
        component: AdminAddTaskNumberConfirmationComponent
      }
    ]
  },
  {
    path: 'evacuee-summary/:id',
    component: EvacueeSummaryComponent
  },
  {
    // TODO: naming: this should be changed to be "evacuee-registration"
    path: 'register-evacuee',
    component: EvacueeRegistrationComponent,
    children: [
      {
        path: '',
        redirectTo: 'fill',
        pathMatch: 'full'
      },
      {
        path: 'fill/:id',
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
    path: 'volunteer-edit',
    component: VolunteerEditorComponent,
    children: [
      {
        path: '',
        redirectTo: 'fill',
        pathMatch: 'full'
      },
      {
        path: 'fill',
        component: VolunteerEditorOneComponent
      },
      {
        path: 'fill/:id',
        component: VolunteerEditorOneComponent
      },
      {
        path: 'confirmation',
        component: VolunteerEditorConfirmationComponent
      }
    ]
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
