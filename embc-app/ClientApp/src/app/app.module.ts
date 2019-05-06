import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbPaginationModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

// ngrx + configuration
import { AppComponent } from './app.component';
import { StoreModule } from '@ngrx/store';
import { rootReducer, metaReducers } from './store';

import { HomeComponent } from './home/home.component';
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
import { TaskNumberListComponent } from './provincial-admin/components/task-number-list/task-number-list.component';
import { VolunteerListComponent } from './volunteer-list/volunteer-list.component';
import { RegistrationListComponent } from './registration-list/registration-list.component';
import { OrganizationListComponent } from './provincial-admin/components/organization-list/organization-list.component';
import { UsefulInformationContentComponent } from './useful-information-content/useful-information-content.component';
import { VolunteerEditorContainerComponent } from './volunteer-editor-container/volunteer-editor-container.component';
import { TaskNumberMakerComponent } from './provincial-admin/components/task-number-maker/task-number-maker.component';
import { VolunteerMakerComponent } from './volunteer-maker/volunteer-maker.component';
import { OrganizationMakerComponent } from './provincial-admin/components/organization-maker/organization-maker.component';
import { RegistrationSummaryFullComponent } from './registration-summary-full/registration-summary-full.component';
import { VolunteerOrganizationListComponent } from './provincial-admin/components/volunteer-organization-list/volunteer-organization-list.component';
import { LayoutMainComponent } from './components/layout-main/layout-main.component';
import { LayoutSideComponent } from './components/layout-side/layout-side.component';
import { RegistrationMakerComponent } from './registration-maker/registration-maker.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { SessionExpiredComponent } from './session-expired/session-expired.component';
import { EvacueeSearchResultsComponent } from './components/evacuee-search-results/evacuee-search-results.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationSummaryComponent } from './components/pagination-summary/pagination-summary.component';
import { RegistrationPageComponent } from './registration-page/registration-page.component';
import { VolunteerRegistrationsPageComponent } from './pages/volunteer/volunteer-registrations-page/volunteer-registrations-page.component';
import { UsefulInformationPageComponent } from './pages/useful-information-page/useful-information-page.component';
import { LocalAuthorityRegistrationsPageComponent } from './pages/local-authority/local-authority-registrations-page/local-authority-registrations-page.component';
import { ProvincialAdminRegistrationsPageComponent } from './provincial-admin/pages/registrations/provincial-admin-registrations-page.component';
import { LocalAuthorityVolunteersPageComponent } from './pages/local-authority/local-authority-volunteers-page/local-authority-volunteers-page.component';
import { ProvincialAdminVolunteersOrganizationPageComponent } from './provincial-admin/pages/volunteers-organization/provincial-admin-volunteers-organization-page.component';
import { ProvincialAdminOrganizationsPageComponent } from './provincial-admin/pages/organizations/provincial-admin-organizations-page.component';
import { TopNavbarComponent } from './components/top-navbar/top-navbar.component';
import { ProvincialAdminTaskNumbersPageComponent } from './provincial-admin/pages/task-numbers/provincial-admin-task-numbers-page.component';
import { SideboxAddRegistrationComponent } from './components/sidebox-add-registration/sidebox-add-registration.component';
import { SideboxAddOrganizationComponent } from './components/sidebox-add-organization/sidebox-add-organization.component';
import { SideboxAddTaskNumberComponent } from './components/sidebox-add-task-number/sidebox-add-task-number.component';
import { SideboxAddVolunteerComponent } from './components/sidebox-add-volunteer/sidebox-add-volunteer.component';
import { SideboxCollectionNoticeComponent } from './components/sidebox-collection-notice/sidebox-collection-notice.component';

// shared modals // TODO: move to shared module
import { ClothingRatesComponent } from './shared/modals/clothing-rates/clothing-rates.component';
import { FoodRatesComponent } from './shared/modals/food-rates/food-rates.component';
import { IncidentalsRatesComponent } from './shared/modals/incidentals-rates/incidentals-rates.component';
import { LodgingRatesComponent } from './shared/modals/lodging-rates/lodging-rates.component';
import { SessionExpiringModalComponent } from './shared/modals/session-expiring/session-expiring.component';
import { TransportationRatesComponent } from './shared/modals/transportation-rates/transportation-rates.component';

// TODO: move these to volunteer/local-authority/provincial-admin modules
import { ReferralMakerComponent } from './volunteer/pages/referral-maker/referral-maker.component';
import { IncidentalsReferralComponent } from './volunteer/components/incidentals-referral/incidentals-referral.component';
import { ReferralTableComponent } from './volunteer/components/referral-table/referral-table.component';
import { ValidFromToComponent } from './volunteer/components/valid-from-to/valid-from-to.component';
import { SupplierComponent } from './volunteer/components/supplier/supplier.component';
import { EvacueeListComponent } from './volunteer/components/evacuee-list/evacuee-list.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LocalAuthorityRegistrationsPageComponent,
    LocalAuthorityVolunteersPageComponent,
    RegistrationSummaryComponent,
    RegistrationListComponent,
    RegistrationMakerComponent,
    RegistrationPageComponent,
    RegistrationSummaryFullComponent,
    VolunteerMakerComponent,
    VolunteerTeamDashboardComponent,
    VolunteerUsefulInformationComponent,
    VolunteerListComponent,
    VolunteerEditorContainerComponent,
    VolunteerOrganizationListComponent,
    VolunteerRegistrationsPageComponent,
    ProvincialAdminRegistrationsPageComponent,
    ProvincialAdminVolunteersOrganizationPageComponent,
    ProvincialAdminOrganizationsPageComponent,
    ProvincialAdminTaskNumbersPageComponent,
    SideboxAddRegistrationComponent,
    SideboxAddOrganizationComponent,
    SideboxAddTaskNumberComponent,
    SideboxAddVolunteerComponent,
    SideboxCollectionNoticeComponent,
    AdminDashboardComponent,
    EvacueeSearchResultsComponent,
    LayoutMainComponent,
    LayoutSideComponent,
    OrganizationListComponent,
    OrganizationMakerComponent,
    UsefulInformationContentComponent,
    TaskNumberMakerComponent,
    TopNavbarComponent,
    TesterPageComponent,
    TopNavigationComponent,
    TaskNumberListComponent,
    SessionExpiredComponent,
    SearchBarComponent,
    PaginationSummaryComponent,
    UsefulInformationPageComponent,

    ClothingRatesComponent,
    FoodRatesComponent,
    IncidentalsRatesComponent,
    LodgingRatesComponent,
    SessionExpiringModalComponent,
    TransportationRatesComponent,

    ReferralMakerComponent,
    IncidentalsReferralComponent,
    ReferralTableComponent,
    ValidFromToComponent,
    SupplierComponent,
    EvacueeListComponent,
  ],
  imports: [
    // angular
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,

    // 3rd party
    StoreModule.forRoot(rootReducer, { metaReducers }),
    NgbPaginationModule,
    NgbModalModule,

    // core & shared
    CoreModule,
    SharedModule,

    // app
    AppRoutingModule,
    SelfRegistrationModule,
    VolunteerEditorModule,
  ],
  entryComponents: [
    ClothingRatesComponent,
    FoodRatesComponent,
    IncidentalsRatesComponent,
    LodgingRatesComponent,
    SessionExpiringModalComponent,
    TransportationRatesComponent,
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }
