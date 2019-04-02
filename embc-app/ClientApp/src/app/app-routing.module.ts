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
import { AdminEvacueesComponent, AdminOrganizationsComponent } from './admin-dashboard';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
import { LoggedInGuard } from './core/guards/logged-in.guard';
import { RoleGuard } from './core/guards/role.guard';
import { RedirectGuard } from './core/guards/redirect.guard';
import { VOLUNTEER, LOCAL_AUTHORITY, PROVINCIAL_ADMIN } from './constants';
import { VolunteerLayoutComponent } from './volunteers/containers/volunteer-layout/volunteer-layout.component';
import { LandingPageGuard } from './core/guards/landing-page.guard';
import { UsefulInformationContentComponent } from './useful-information-content/useful-information-content.component';
import { AdminTaskNumbersComponent } from './admin-task-numbers/admin-task-numbers.component';

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
    path: 'test',
    component: TesterPageComponent
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


  // TODO: New naming starts HERE

  {
    // special route to redirect to EXTERNAL links (i.e. http://www.google.com)
    // NOTE - we need this to redirect to the /login URL without Angular interfering
    path: 'external',
    canActivateChild: [RedirectGuard],
    children: [
      {
        path: 'login',
        component: PageNotFoundComponent, // We need a component here because we cannot define the route otherwise
        data: { externalUrl: 'login' },
      },
    ]
  },

  // Landing Page
  {
    path: 'dashboard',
    canActivate: [LandingPageGuard],
    component: PageNotFoundComponent, // TODO: See if we can remove this component here without breaking routing
    data: {
      navigateByRole: {
        'role_volunteer': 'volunteer/evacuees',
        'role_local_authority': 'local-authority/evacuees',
        'role_provincial_admin': 'provincial-admin/evacuees',
      }
    },
  },

  // VOLUNTEER routes
  {
    path: 'volunteer',
    component: VolunteerLayoutComponent,
    canActivate: [LoggedInGuard],
    canActivateChild: [RoleGuard],
    data: { expectedRole: VOLUNTEER },
    children: [
      {
        path: 'evacuees',
        component: VolunteerDashboardComponent,
        data: { expectedRole: VOLUNTEER },
      },
      {
        path: 'evacuee-summary/:id',
        component: EvacueeSummaryComponent,
        data: { expectedRole: VOLUNTEER },
      },
      {
        path: 'register-evacuee',
        component: EvacueeRegistrationComponent,
        data: { expectedRole: VOLUNTEER },
        children: [
          {
            path: '',
            redirectTo: 'fill',
            pathMatch: 'full'
          },
          {
            path: 'fill/:id',
            component: EvacueeRegistrationOneComponent,
            data: { expectedRole: VOLUNTEER },
          },
          {
            path: 'fill',
            component: EvacueeRegistrationOneComponent,
            data: { expectedRole: VOLUNTEER },
          },
          {
            path: 'confirmation',
            component: EvacueeRegistrationConfirmationComponent,
            data: { expectedRole: VOLUNTEER },
          }
        ]
      },
      {
        path: 'useful-info',
        component: VolunteerUsefulInformationComponent,
        data: { expectedRole: VOLUNTEER },
      },
    ],
  },

  // LOCAL_AUTHORITY routes
  {
    path: 'local-authority',
    component: VolunteerLayoutComponent,
    canActivate: [LoggedInGuard],
    canActivateChild: [RoleGuard],
    children: [
      {
        path: 'evacuees',
        component: VolunteerDashboardComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'evacuee-summary/:id',
        component: EvacueeSummaryComponent,
        data: { expectedRole: LOCAL_AUTHORITY }
      },
      {
        path: 'register-evacuee',
        component: EvacueeRegistrationComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
        children: [
          {
            path: '',
            redirectTo: 'fill',
            pathMatch: 'full'
          },
          {
            path: 'fill/:id',
            component: EvacueeRegistrationOneComponent,
            data: { expectedRole: LOCAL_AUTHORITY },
          },
          {
            path: 'fill',
            component: EvacueeRegistrationOneComponent,
            data: { expectedRole: LOCAL_AUTHORITY },
          },
          {
            path: 'confirmation',
            component: EvacueeRegistrationConfirmationComponent,
            data: { expectedRole: LOCAL_AUTHORITY },
          }
        ]
      },
      {
        path: 'volunteers',
        component: VolunteerTeamDashboardComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'volunteer-edit',
        component: VolunteerEditorComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
        children: [
          {
            path: '',
            redirectTo: 'fill',
            pathMatch: 'full'
          },
          {
            path: 'fill',
            component: VolunteerEditorOneComponent,
            data: { expectedRole: LOCAL_AUTHORITY },
          },
          {
            path: 'fill/:id',
            component: VolunteerEditorOneComponent,
            data: { expectedRole: LOCAL_AUTHORITY },
          },
          {
            path: 'confirmation',
            component: VolunteerEditorConfirmationComponent,
            data: { expectedRole: LOCAL_AUTHORITY },
          }
        ]
      },
      {
        path: 'useful-info',
        component: VolunteerUsefulInformationComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
    ],
  },
  {
    path: 'provincial-admin/task-number-edit',
    component: AdminAddTaskNumberComponent,
    data: { expectedRole: PROVINCIAL_ADMIN },
    children: [
      {
        path: '',
        redirectTo: 'fill',
        pathMatch: 'full'
      },
      {
        path: 'fill',
        component: AdminAddTaskNumberOneComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'fill/:id',
        component: AdminAddTaskNumberOneComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'confirmation',
        component: AdminAddTaskNumberConfirmationComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      }
    ]
  },
  // PROVINCIAL_ADMIN routes
  {
    path: 'provincial-admin',
    component: AdminDashboardComponent,
    // canActivate: [LoggedInGuard],
    // canActivateChild: [RoleGuard],
    children: [
      {
        path: '',
        redirectTo: 'evacuees',
        pathMatch: 'full',
      },

      {
        path: 'evacuees',
        component: AdminEvacueesComponent,
        // data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'evacuee-summary/:id',
        component: EvacueeSummaryComponent,
        // data: { expectedRole: PROVINCIAL_ADMIN }
      },
      {
        path: 'register-evacuee',
        component: EvacueeRegistrationComponent,
        // data: { expectedRole: PROVINCIAL_ADMIN },
        children: [
          {
            path: '',
            redirectTo: 'fill',
            pathMatch: 'full'
          },
          {
            path: 'fill/:id',
            component: EvacueeRegistrationOneComponent,
            // data: { expectedRole: PROVINCIAL_ADMIN },
          },
          {
            path: 'fill',
            component: EvacueeRegistrationOneComponent,
            // data: { expectedRole: PROVINCIAL_ADMIN },
          },
          {
            path: 'confirmation',
            component: EvacueeRegistrationConfirmationComponent,
            // data: { expectedRole: PROVINCIAL_ADMIN },
          }
        ]
      },
      {
        path: 'volunteers',
        component: VolunteerTeamDashboardComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'volunteer-edit',
        component: VolunteerEditorComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
        children: [
          {
            path: '',
            redirectTo: 'fill',
            pathMatch: 'full'
          },
          {
            path: 'fill',
            component: VolunteerEditorOneComponent,
            data: { expectedRole: PROVINCIAL_ADMIN },
          },
          {
            path: 'fill/:id',
            component: VolunteerEditorOneComponent,
            data: { expectedRole: PROVINCIAL_ADMIN },
          },
          {
            path: 'confirmation',
            component: VolunteerEditorConfirmationComponent,
            data: { expectedRole: PROVINCIAL_ADMIN },
          }
        ]
      },
      {
        path: 'useful-info',
        component: UsefulInformationContentComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      // {
      //   path: 'task-numbers',
      //   component: AdminTaskNumbersComponent,
      //   data: { expectedRole: PROVINCIAL_ADMIN },
      // },
    ],
  },

  // 404 route (catch all default)
  {
    path: '**',
    component: PageNotFoundComponent
  },

  // TODO: Remove/review routes BELOW HERE


  // {
  //   path: 'useful-info',
  //   component: VolunteerUsefulInformationComponent,
  // },
  // {
  //   path: 'volunteer/useful-info',
  //   component: VolunteerUsefulInformationComponent,
  // },
  // {
  //   path: 'volunteer-dashboard',
  //   component: VolunteerDashboardComponent,
  //   // canActivate: [RoleGuardService],
  //   // data: { expectedRole: 'volunteer' }
  // },
  // {
  //   path: 'local-authority/evacuees',
  //   component: VolunteerDashboardComponent,
  //   // canActivate: [RoleGuardService],
  //   // data: { expectedRole: 'local_authority' }
  // },
  // {
  //   path: 'volunteer-team-dashboard',
  //   component: VolunteerTeamDashboardComponent,
  //   // canActivate: [RoleGuardService],
  //   // data: { expectedRole: 'local_authority' }
  // },
  // {
  //   path: 'local-authority/volunteers',
  //   component: VolunteerTeamDashboardComponent,
  //   // canActivate: [RoleGuardService],
  //   // data: { expectedRole: 'local_authority' }
  // },
  // {
  //   path: 'provincial-admin',
  //   component: AdminDashboardComponent,
  //   // canActivate: [RoleGuardService],
  //   // data: { expectedRole: 'provincial_admin' },
  //   children: [
  //     {
  //       path: '',
  //       redirectTo: 'evacuees',
  //       pathMatch: 'full',
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'evacuees',
  //       component: AdminEvacueesComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'edit-task-numbers',
  //       component: AdminTaskNumbersComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'edit-task-numbers/:id',
  //       component: AdminTaskNumbersComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'edit-registration',
  //       component: AdminTaskNumbersComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'edit-registration/:id',
  //       component: AdminTaskNumbersComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'edit-volunteer',
  //       component: AdminTaskNumbersComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'edit-volunteer/:id',
  //       component: AdminTaskNumbersComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'organizations',
  //       component: AdminOrganizationsComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'edit-organization',
  //       component: AdminTaskNumbersComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'edit-organization/:id',
  //       component: AdminTaskNumbersComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'useful-info',
  //       redirectTo: '/useful-info'
  //     },
  //   ]
  // },
  // {
  //   path: 'provincial-admin/',
  //   component: AdminAddTaskNumberComponent,
  //   // canActivate: [RoleGuardService],
  //   // data: { expectedRole: 'provincial_admin' },
  //   children: [
  //     {
  //       path: '',
  //       redirectTo: 'fill',
  //       pathMatch: 'full',
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'fill',
  //       component: AdminAddTaskNumberOneComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'fill/:id',
  //       component: AdminAddTaskNumberOneComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'confirmation',
  //       component: AdminAddTaskNumberConfirmationComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     },
  //     {
  //       path: 'confirmation',
  //       component: AdminAddTaskNumberConfirmationComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'provincial_admin' }
  //     }
  //   ]
  // },
  // {
  //   path: 'evacuee-summary/:id',
  //   component: EvacueeSummaryComponent,
  //   // canActivate: [RoleGuardService],
  //   // data: { expectedRole: 'volunteer' }
  // },
  // {
  //   path: 'register-evacuee',
  //   component: EvacueeRegistrationComponent,
  //   children: [
  //     {
  //       path: '',
  //       redirectTo: 'fill',
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'volunteer' },
  //       pathMatch: 'full'
  //     },
  //     {
  //       path: 'fill/:id',
  //       component: EvacueeRegistrationOneComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'volunteer' },
  //     },
  //     {
  //       path: 'fill',
  //       component: EvacueeRegistrationOneComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'volunteer' },
  //     },
  //     {
  //       path: 'confirmation',
  //       component: EvacueeRegistrationConfirmationComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'volunteer' },
  //     }
  //   ]
  // },
  // {
  //   path: 'volunteer-edit',
  //   component: VolunteerEditorComponent,
  //   children: [
  //     {
  //       path: '',
  //       redirectTo: 'fill',
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'local_authority' },
  //       pathMatch: 'full'
  //     },
  //     {
  //       path: 'fill',
  //       component: VolunteerEditorOneComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'local_authority' }
  //     },
  //     {
  //       path: 'fill/:id',
  //       component: VolunteerEditorOneComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'local_authority' }
  //     },
  //     {
  //       path: 'confirmation',
  //       component: VolunteerEditorConfirmationComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'local_authority' }
  //     }
  //   ]
  // },
  // {
  //   path: 'task-number-edit',
  //   component: AdminAddTaskNumberComponent,
  //   children: [
  //     {
  //       path: '',
  //       redirectTo: 'fill',
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'local_authority' },
  //       pathMatch: 'full'
  //     },
  //     {
  //       path: 'fill',
  //       component: AdminAddTaskNumberOneComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'local_authority' }
  //     },
  //     {
  //       path: 'fill/:id',
  //       component: AdminAddTaskNumberOneComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'local_authority' }
  //     },
  //     {
  //       path: 'confirmation',
  //       component: AdminAddTaskNumberConfirmationComponent,
  //       // canActivate: [RoleGuardService],
  //       // data: { expectedRole: 'local_authority' }
  //     }
  //   ]
  // },

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
