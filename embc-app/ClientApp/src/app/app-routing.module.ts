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
import { VolunteerTeamDashboardComponent } from './volunteer-team-dashboard/volunteer-team-dashboard.component';
import { EvacueeSummaryComponent } from './evacuee-summary/evacuee-summary.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
import { LoggedInGuard } from './core/guards/logged-in.guard';
import { RoleGuard } from './core/guards/role.guard';
import { RedirectGuard } from './core/guards/redirect.guard';
import { VOLUNTEER, LOCAL_AUTHORITY, PROVINCIAL_ADMIN } from './constants';
import { VolunteerLayoutComponent } from './volunteers/containers/volunteer-layout/volunteer-layout.component';
import { LandingPageGuard } from './core/guards/landing-page.guard';
import { UsefulInformationContentComponent } from './useful-information-content/useful-information-content.component';
import { TaskNumberListComponent } from './task-number-list/task-number-list.component';
import { VolunteerListComponent } from './volunteer-list/volunteer-list.component';
import { OrganizationListComponent } from './organization-list/organization-list.component';
import { OrganizationMakerComponent } from './organization-maker/organization-maker.component';
import { TaskNumberMakerComponent } from './task-number-maker/task-number-maker.component';
import { VolunteerMakerComponent } from './volunteer-maker/volunteer-maker.component';
import { EvacueeListComponent } from './evacuee-list/evacuee-list.component';
import { VolunteerUsefulInformationComponent } from './volunteer-useful-information/volunteer-useful-information.component';
import { EvacueeSummaryPageComponent } from './evacuee-summary-page/evacuee-summary-page.component';
import { VolunteerOrganizationListComponent } from './volunteer-organization-list/volunteer-organization-list.component';

/*
  /
    self-registration
      /
      step-1
      step-2
      step-3
      step-4/:id
      error
    external
      login

  /volunteer
    evacuees
    evacuee-summary/:id
    register-evacuee
      /
      fill/:id
      fill
      confirmation
    useful-info

  /local-authority
    evacuees
    evacuee-summary/:id
    register-evacuee
      /
      fill/:id
      fill
      confirmation
    volunteers
    volunteer
    volunteer/:id
    useful-info

  /provincial-admin
    /
    evacuees
    evacuee-summary/:id
    organizations
    organization
    organization/:id
    task-numbers
    task-number
    task-number/:id
    volunteers
    volunteer
    volunteer/:id
    register-evacuee
      /
      fill/:id
      fill
      confirmation
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
    path: 'test/:id',
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
        role_volunteer: 'volunteer/evacuees',
        role_local_authority: 'local-authority/evacuees',
        role_provincial_admin: 'provincial-admin/evacuees',
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
        path: 'evacuee/:id',
        component: EvacueeSummaryPageComponent,
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
      },
      {
        path: 'register-evacuee/:id',
        component: EvacueeRegistrationComponent,
        data: { expectedRole: VOLUNTEER },
      },
      {
        path: 'register-evacuee/fill',
        component: EvacueeRegistrationComponent,
        data: { expectedRole: VOLUNTEER },
      },
      {
        path: 'register-evacuee/fill/:id',
        component: EvacueeRegistrationComponent,
        data: { expectedRole: VOLUNTEER },
      },
      // {
      //   path: 'register-evacuee',
      //   component: EvacueeRegistrationComponent,
      //   data: { expectedRole: VOLUNTEER },
      //   children: [
      //     {
      //       path: '',
      //       redirectTo: 'fill',
      //       pathMatch: 'full'
      //     },
      //     {
      //       path: 'fill/:id',
      //       component: EvacueeRegistrationOneComponent,
      //       data: { expectedRole: VOLUNTEER },
      //     },
      //     {
      //       path: 'fill',
      //       component: EvacueeRegistrationOneComponent,
      //       data: { expectedRole: VOLUNTEER },
      //     },
      //     {
      //       path: 'confirmation',
      //       component: EvacueeRegistrationConfirmationComponent,
      //       data: { expectedRole: VOLUNTEER },
      //     }
      //   ]
      // },
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
        path: 'evacuee/:id',
        component: EvacueeSummaryPageComponent,
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
      },
      {
        path: 'register-evacuee/:id',
        component: EvacueeRegistrationComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'register-evacuee/fill',
        component: EvacueeRegistrationComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'register-evacuee/fill/:id',
        component: EvacueeRegistrationComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      // {
      //   path: 'register-evacuee',
      //   component: EvacueeRegistrationComponent,
      //   data: { expectedRole: LOCAL_AUTHORITY },
      //   children: [
      //     {
      //       path: '',
      //       redirectTo: 'fill',
      //       pathMatch: 'full'
      //     },
      //     {
      //       path: 'fill/:id',
      //       component: EvacueeRegistrationOneComponent,
      //       data: { expectedRole: LOCAL_AUTHORITY },
      //     },
      //     {
      //       path: 'fill',
      //       component: EvacueeRegistrationOneComponent,
      //       data: { expectedRole: LOCAL_AUTHORITY },
      //     },
      //     {
      //       path: 'confirmation',
      //       component: EvacueeRegistrationConfirmationComponent,
      //       data: { expectedRole: LOCAL_AUTHORITY },
      //     }
      //   ]
      // },
      {
        path: 'volunteers',
        component: VolunteerTeamDashboardComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
        children: [
          {
            path: '',
            component: VolunteerListComponent,
            data: { expectedRole: LOCAL_AUTHORITY },
          }
        ]
      },
      {
        path: 'volunteer',
        component: VolunteerMakerComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'volunteer/:id',
        component: VolunteerMakerComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'useful-info',
        component: UsefulInformationContentComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
    ],
  },

  // PROVINCIAL_ADMIN routes
  {
    path: 'provincial-admin',
    component: AdminDashboardComponent,
    canActivate: [LoggedInGuard],
    canActivateChild: [RoleGuard],
    children: [
      {
        path: '',
        redirectTo: 'evacuees',
        pathMatch: 'full',
      },
      {
        path: 'evacuees',
        component: EvacueeListComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'evacuee/:id',
        component: EvacueeSummaryPageComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'evacuee-summary/:id',
        component: EvacueeSummaryComponent,
        data: { expectedRole: PROVINCIAL_ADMIN }
      },
      {
        path: 'organizations',
        component: OrganizationListComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'organization',
        component: OrganizationMakerComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'organization/:id',
        component: OrganizationMakerComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'task-numbers',
        component: TaskNumberListComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'task-number',
        component: TaskNumberMakerComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'task-number/:id',
        component: TaskNumberMakerComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'volunteers',
        component: VolunteerOrganizationListComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'volunteer',
        component: VolunteerMakerComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'volunteer/:id',
        component: VolunteerMakerComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'register-evacuee',
        component: EvacueeRegistrationOneComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'register-evacuee/:id',
        component: EvacueeRegistrationOneComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        // the fill is to make it work exactly like the two components replaced.
        path: 'register-evacuee/fill',
        component: EvacueeRegistrationOneComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        // the fill is to make it work exactly like the two components replaced.
        path: 'register-evacuee/fill/:id',
        component: EvacueeRegistrationOneComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'useful-info',
        component: UsefulInformationContentComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
    ],
  },

  {
    path: 'evacuee-summary/:id',
    component: EvacueeSummaryComponent,
  },

  // {
  //   // exception in routing
  //   path: 'register-evacuee',
  //   redirectTo: 'provincial-admin/register-evacuee/fill'
  // },
  {
    // exception in routing
    path: 'register-evacuee/register-evacuee/fill/:id',
    component: EvacueeRegistrationOneComponent,
  },
  {
    // exception in routing
    path: 'register-evacuee/fill',
    component: EvacueeRegistrationComponent,
  },
  {
    // exception in routing
    path: 'register-evacuee/fill/:id',
    component: EvacueeRegistrationComponent,
  },
  {
    // exception in routing
    path: 'evacuees',
    redirectTo: 'dashboard'
  },

  // 404 route (catch all default)
  {
    path: '**',
    component: PageNotFoundComponent
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
