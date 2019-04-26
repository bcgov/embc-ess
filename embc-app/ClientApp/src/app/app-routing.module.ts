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
import { RegistrationPageComponent } from './registration-page/registration-page.component';
import { TesterPageComponent } from './tester-page/tester-page.component';
import { RegistrationMakerComponent } from './registration-maker/registration-maker.component';
import { VolunteerTeamDashboardComponent } from './volunteer-team-dashboard/volunteer-team-dashboard.component';
import { RegistrationSummaryComponent } from './registration-summary/registration-summary.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
import { LoggedInGuard } from './core/guards/logged-in.guard';
import { RoleGuard } from './core/guards/role.guard';
import { RedirectGuard } from './core/guards/redirect.guard';
import { VOLUNTEER, LOCAL_AUTHORITY, PROVINCIAL_ADMIN } from './constants';
import { LandingPageGuard } from './core/guards/landing-page.guard';
import { UsefulInformationContentComponent } from './useful-information-content/useful-information-content.component';
import { TaskNumberListComponent } from './task-number-list/task-number-list.component';
import { VolunteerListComponent } from './volunteer-list/volunteer-list.component';
import { OrganizationListComponent } from './organization-list/organization-list.component';
import { OrganizationMakerComponent } from './organization-maker/organization-maker.component';
import { TaskNumberMakerComponent } from './task-number-maker/task-number-maker.component';
import { VolunteerMakerComponent } from './volunteer-maker/volunteer-maker.component';
import { RegistrationListComponent } from './registration-list/registration-list.component';
import { VolunteerUsefulInformationComponent } from './volunteer-useful-information/volunteer-useful-information.component';
import { RegistrationSummaryFullComponent } from './registration-summary-full/registration-summary-full.component';
import { VolunteerOrganizationListComponent } from './volunteer-organization-list/volunteer-organization-list.component';
import { SessionExpiredComponent } from './session-expired/session-expired.component';
import { VolunteerRegistrationsPageComponent } from './pages/volunteer/volunteer-registrations-page/volunteer-registrations-page.component';
import { UsefulInformationPageComponent } from './pages/useful-information-page/useful-information-page.component';
import { LocalAuthorityRegistrationsPageComponent } from './pages/local-authority/local-authority-registrations-page/local-authority-registrations-page.component';
import { ProvincialAdminOrganizationsPageComponent } from './provincial-admin/pages/organizations/provincial-admin-organizations-page.component';
import { ProvincialAdminTaskNumbersPageComponent } from './provincial-admin/pages/task-numbers/provincial-admin-task-numbers-page.component';
import { ProvincialAdminRegistrationsPageComponent } from './provincial-admin/pages/registrations/provincial-admin-registrations-page.component';
import { LocalAuthorityVolunteersPageComponent } from './pages/local-authority/local-authority-volunteers-page/local-authority-volunteers-page.component';

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
    registrations               <-- shows evacuee list page
    registration/summary        <-- view tombstone summary page
    registration/summary/full   <-- view full summary page
    registration                <-- evacuee maker
    useful-info

  /local-authority
    registrations               <-- shows evacuee list page
    volunteers                  <-- volunteer list page
    registration/summary        <-- view tombstone summary page
    registration/summary/full   <-- view full summary page
    registration                <-- evacuee maker
    volunteer                   <-- volunteer maker
    useful-info

  /provincial-admin
    / <-- routes to task numbers
    evacuees                    <-- shows evacuee list page
    organizations               <-- organization list page
    volunteers                  <-- volunteer list page
    task-numbers                <-- task number list page
    registration/summary        <-- view tombstone summary page
    registration/summary/full   <-- view full tombstone summary page
    evacuee                     <-- evacuee maker
    organization                <-- organization maker
    task-number                 <-- task number maker
    volunteer                   <-- volunteer maker
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
    path: 'session-expired',
    component: SessionExpiredComponent
  },
  {
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
        role_volunteer: 'volunteer-',
        role_local_authority: 'local-authority-',
        role_provincial_admin: 'provincial-admin-',
      }
    },
  },

  // VOLUNTEER NEW routes
  {
    path: 'volunteer-',
    canActivate: [LoggedInGuard],
    canActivateChild: [RoleGuard],
    data: { expectedRole: VOLUNTEER },
    children: [
      {
        // set the default component to route to for this user
        path: '',
        component: VolunteerRegistrationsPageComponent,
        data: { expectedRole: VOLUNTEER },
      },
      {
        path: 'registrations',
        component: VolunteerRegistrationsPageComponent,
        data: { expectedRole: VOLUNTEER },
      },
      {
        path: 'registration',
        component: RegistrationMakerComponent,
        data: { expectedRole: VOLUNTEER },
      },
      {
        path: 'registration/summary',
        component: RegistrationSummaryComponent,
        data: { expectedRole: VOLUNTEER },
      },
      {
        path: 'registration/summary/full',
        component: RegistrationSummaryFullComponent,
        data: { expectedRole: VOLUNTEER },
      },
      {
        path: 'useful-info',
        component: UsefulInformationPageComponent,
        data: { expectedRole: VOLUNTEER },
      },
    ],
  },

  // LOCAL AUTHORITY NEW routes
  {
    path: 'local-authority-',
    canActivate: [LoggedInGuard],
    canActivateChild: [RoleGuard],
    children: [
      {
        // set the default component to route to for this user
        path: '',
        component: LocalAuthorityRegistrationsPageComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'registrations',
        component: LocalAuthorityRegistrationsPageComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'volunteers',
        component: LocalAuthorityVolunteersPageComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'registration',
        component: RegistrationMakerComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'registration/summary',
        component: RegistrationSummaryComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'registration/summary/full',
        component: RegistrationSummaryFullComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'volunteer',
        component: VolunteerMakerComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'useful-info',
        component: UsefulInformationPageComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
    ],
  },

  // PROVINCIAL_ADMIN NEW routes
  {
    path: 'provincial-admin-',
    canActivate: [LoggedInGuard],
    canActivateChild: [RoleGuard],
    children: [
      {
        // set the default component to route to for this user
        path: '',
        component: ProvincialAdminRegistrationsPageComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'registrations',
        component: ProvincialAdminRegistrationsPageComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'organizations',
        component: ProvincialAdminOrganizationsPageComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'task-numbers',
        component: ProvincialAdminTaskNumbersPageComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'registration',
        component: RegistrationMakerComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'registration/summary',
        component: RegistrationSummaryComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'registration/summary/full',
        component: RegistrationSummaryFullComponent,
        data: { expectedRole: LOCAL_AUTHORITY },
      },
      {
        path: 'organization',
        component: OrganizationMakerComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'organization/volunteers',
        component: VolunteerOrganizationListComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        // the volunteers route is really the organization starting point. Must be like this to share components with other roles
        path: 'volunteers',
        redirectTo: 'organizations'
      },
      {
        path: 'volunteer',
        component: VolunteerMakerComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'task-number',
        component: TaskNumberMakerComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },
      {
        path: 'useful-info',
        component: UsefulInformationPageComponent,
        data: { expectedRole: PROVINCIAL_ADMIN },
      },

    ]
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
