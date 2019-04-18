import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbPaginationModule, NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';

// ngrx + configuration
import { AppComponent } from './app.component';
import { StoreModule } from '@ngrx/store';
import { rootReducer, metaReducers } from './store';

import { HomeComponent } from './home/home.component';
import { VolunteerDashboardComponent } from './volunteer-dashboard/volunteer-dashboard.component';
import { TesterPageComponent } from './tester-page/tester-page.component';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';
import { VolunteerEditorModule } from './volunteer-editor';

import { SelfRegistrationModule } from './self-registration';
import { SharedModule } from './shared/shared.module';
import { VolunteerTeamDashboardComponent } from './volunteer-team-dashboard/volunteer-team-dashboard.component';
import { VolunteerUsefulInformationComponent } from './volunteer-useful-information/volunteer-useful-information.component';
import { RegistrationSummaryComponent } from './registration-summary/registration-summary.component';
import { TopNavigationComponent } from './top-navigation/top-navigation.component';
import { TaskNumberListComponent } from './task-number-list/task-number-list.component';
import { VolunteerListComponent } from './volunteer-list/volunteer-list.component';
import { RegistrationListComponent } from './registration-list/registration-list.component';
import { OrganizationListComponent } from './organization-list/organization-list.component';
import { UsefulInformationContentComponent } from './useful-information-content/useful-information-content.component';
import { VolunteerEditorContainerComponent } from './volunteer-editor-container/volunteer-editor-container.component';
import { TaskNumberMakerComponent } from './task-number-maker/task-number-maker.component';
import { VolunteerMakerComponent } from './volunteer-maker/volunteer-maker.component';
import { OrganizationMakerComponent } from './organization-maker/organization-maker.component';
import { RegistrationSummaryFullComponent } from './registration-summary-full/registration-summary-full.component';
import { VolunteerOrganizationListComponent } from './volunteer-organization-list/volunteer-organization-list.component';
import { LayoutMainComponent } from './components/layout-main/layout-main.component';
import { LayoutSideComponent } from './components/layout-side/layout-side.component';
import { RegistrationMakerComponent } from './registration-maker/registration-maker.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { SessionExpiredComponent } from './session-expired/session-expired.component';
import { EvacueeSearchResultsComponent } from './components/evacuee-search-results/evacuee-search-results.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationSummaryComponent } from './components/pagination-summary/pagination-summary.component';
import { RegistrationPageComponent } from './registration-page/registration-page.component';
import { VolunteerDashboardExamplePageComponent } from './pages/volunteer/volunteer-dashboard-example-page/volunteer-dashboard-example-page.component';
import { VolunteerRegistrationsPageComponent } from './pages/volunteer/volunteer-registrations-page/volunteer-registrations-page.component';
import { UsefulInformationPageComponent } from './pages/useful-information-page/useful-information-page.component';
import { LocalAuthorityRegistrationsPageComponent } from './pages/local-authority/local-authority-registrations-page/local-authority-registrations-page.component';
import { ProvincialAdminRegistrationsPageComponent } from './pages/provincial-admin/provincial-admin-registrations-page/provincial-admin-registrations-page.component';
import { LocalAuthorityVolunteersPageComponent } from './pages/local-authority/local-authority-volunteers-page/local-authority-volunteers-page.component';
import { ProvincialAdminVolunteersOrganizationPageComponent } from './pages/provincial-admin/provincial-admin-volunteers-organization-page/provincial-admin-volunteers-organization-page.component';
import { ProvincialAdminOrganizationsPageComponent } from './pages/provincial-admin/provincial-admin-organizations-page/provincial-admin-organizations-page.component';
import { TopNavbarComponent } from './components/top-navbar/top-navbar.component';
import { ProvincialAdminTaskNumbersPageComponent } from './pages/provincial-admin/provincial-admin-task-numbers-page/provincial-admin-task-numbers-page.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    VolunteerDashboardComponent,
    TesterPageComponent,
    VolunteerTeamDashboardComponent,
    VolunteerUsefulInformationComponent,
    RegistrationSummaryComponent,
    TopNavigationComponent,
    TaskNumberListComponent,
    VolunteerListComponent,
    RegistrationListComponent,
    OrganizationListComponent,
    UsefulInformationContentComponent,
    VolunteerEditorContainerComponent,
    TaskNumberMakerComponent,
    VolunteerMakerComponent,
    OrganizationMakerComponent,
    RegistrationSummaryFullComponent,
    VolunteerOrganizationListComponent,
    LayoutMainComponent,
    LayoutSideComponent,
    VolunteerDashboardExamplePageComponent,
    RegistrationMakerComponent,
    RegistrationPageComponent,
    AdminDashboardComponent,
    SessionExpiredComponent,
    EvacueeSearchResultsComponent,
    SearchBarComponent,
    PaginationSummaryComponent,
    VolunteerRegistrationsPageComponent,
    UsefulInformationPageComponent,
    LocalAuthorityRegistrationsPageComponent,
    ProvincialAdminRegistrationsPageComponent,
    LocalAuthorityVolunteersPageComponent,
    ProvincialAdminVolunteersOrganizationPageComponent,
    ProvincialAdminOrganizationsPageComponent,
    TopNavbarComponent,
    ProvincialAdminTaskNumbersPageComponent,
  ],
  imports: [
    // angular
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,

    // 3rd party
    StoreModule.forRoot(rootReducer, { metaReducers }),
    NgbPaginationModule,
    NgbAlertModule,

    // core & shared
    CoreModule,
    SharedModule,

    // app
    AppRoutingModule,
    SelfRegistrationModule,
    VolunteerEditorModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
