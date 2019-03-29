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
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
import { RoleGuardService } from './core/services/role-guard.service';
/**
  /
    self-registration
    login
  /volunteer
    evacuees
    edit-evacuee
    edit-evacuee/:id
    useful-info

  /local-authority
    evacuees
    edit-registration
    edit-registration/:id
    volunteers
    edit-volunteer
    edit-volunteer/:id
    useful-info

  /provincial-admin
    edit-task-numbers
    evacuees
    edit-registration
    edit-registration/:id
    organizations
    edit-volunteer
    edit-organization
    ess-team
    useful-info
*/
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
        pathMatch: 'full',

      },
      {
        path: 'step-1',
        component: SelfRegistrationOneComponent,

      },
      {
        path: 'step-2',
        component: SelfRegistrationTwoComponent,

      },
      {
        path: 'step-3',
        component: SelfRegistrationThreeComponent,

      },
      {
        path: 'step-4/:id',
        component: SelfRegistrationFourComponent,

      },
      {
        path: 'error',
        component: SelfRegistrationErrorComponent,

      },
    ]
  },
  {
    path: 'useful-info',
    component: VolunteerUsefulInformationComponent,
  },
  {
    path: 'volunteer/useful-info',
    component: VolunteerUsefulInformationComponent,
  },
  {
    path: 'volunteer/evacuees',
    component: VolunteerDashboardComponent,
    canActivate: [RoleGuardService],
    data: { expectedRole: 'volunteer' }
  },
  {
    path: 'local-authority/volunteers',
    component: VolunteerTeamDashboardComponent,
    canActivate: [RoleGuardService],
    data: { expectedRole: 'local_authority' }
  },
  {
    path: 'provincial-admin',
    component: AdminDashboardComponent,
    canActivate: [RoleGuardService],
    data: { expectedRole: 'provincial_admin' },
    children: [
      {
        path: '',
        redirectTo: 'evacuees',
        pathMatch: 'full',
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'evacuees',
        component: AdminEvacueesComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'edit-task-numbers',
        component: AdminTaskNumbersComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'edit-task-numbers/:id',
        component: AdminTaskNumbersComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'edit-registration',
        component: AdminTaskNumbersComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'edit-registration/:id',
        component: AdminTaskNumbersComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'edit-volunteer',
        component: AdminTaskNumbersComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'edit-volunteer/:id',
        component: AdminTaskNumbersComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'organizations',
        component: AdminOrganizationsComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'edit-organization',
        component: AdminTaskNumbersComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'edit-organization/:id',
        component: AdminTaskNumbersComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'useful-info',
        redirectTo: '/useful-info'
      },
    ]
  },
  {
    path: 'provincial-admin/',
    component: AdminAddTaskNumberComponent,
    canActivate: [RoleGuardService],
    data: { expectedRole: 'provincial_admin' },
    children: [
      {
        path: '',
        redirectTo: 'fill',
        pathMatch: 'full',
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'fill',
        component: AdminAddTaskNumberOneComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'fill/:id',
        component: AdminAddTaskNumberOneComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'confirmation',
        component: AdminAddTaskNumberConfirmationComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      },
      {
        path: 'confirmation',
        component: AdminAddTaskNumberConfirmationComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'provincial_admin' }
      }
    ]
  },
  {
    path: 'evacuee-summary/:id',
    component: EvacueeSummaryComponent,
    canActivate: [RoleGuardService],
    data: { expectedRole: 'volunteer' }
  },
  {
    path: 'register-evacuee',
    component: EvacueeRegistrationComponent,
    children: [
      {
        path: '',
        redirectTo: 'fill',
        canActivate: [RoleGuardService],
        data: { expectedRole: 'volunteer' },
        pathMatch: 'full'
      },
      {
        path: 'fill/:id',
        component: EvacueeRegistrationOneComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'volunteer' },
      },
      {
        path: 'fill',
        component: EvacueeRegistrationOneComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'volunteer' },
      },
      {
        path: 'confirmation',
        component: EvacueeRegistrationConfirmationComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'volunteer' },
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
        canActivate: [RoleGuardService],
        data: { expectedRole: 'local_authority' },
        pathMatch: 'full'
      },
      {
        path: 'fill',
        component: VolunteerEditorOneComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'local_authority' }
      },
      {
        path: 'fill/:id',
        component: VolunteerEditorOneComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'local_authority' }
      },
      {
        path: 'confirmation',
        component: VolunteerEditorConfirmationComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: 'local_authority' }
      }
    ]
  },
  {
    path: 'test',
    component: TesterPageComponent
  },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
