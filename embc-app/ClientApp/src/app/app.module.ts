import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// ngrx + configuration
import { AppComponent } from './app.component';
import { StoreModule } from '@ngrx/store';
import { rootReducer, metaReducers } from './store';

import { HomeComponent } from './home/home.component';
import { TesterPageComponent } from './tester-page/tester-page.component';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';
import { SelfRegistrationModule } from './self-registration';
import { SharedModule } from './shared/shared.module';
import { RegistrationSummaryComponent } from './components/registration-summary/registration-summary.component';
import { TaskNumberListComponent } from './provincial-admin/components/task-number-list/task-number-list.component';
import { VolunteerListComponent } from './local-authority/components/volunteer-list/volunteer-list.component';
import { OrganizationListComponent } from './provincial-admin/components/organization-list/organization-list.component';
import { TaskNumberMakerComponent } from './provincial-admin/components/task-number-maker/task-number-maker.component';
import { VolunteerMakerComponent } from './local-authority/components/volunteer-maker/volunteer-maker.component';
import { OrganizationMakerComponent } from './provincial-admin/components/organization-maker/organization-maker.component';
import { RegistrationSummaryFullComponent } from './components/registration-summary-full/registration-summary-full.component';
import { VolunteerOrganizationListComponent } from './provincial-admin/components/volunteer-organization-list/volunteer-organization-list.component';
import { RegistrationMakerComponent } from './volunteer/components/registration-maker/registration-maker.component';
import { SessionExpiredComponent } from './components/session-expired/session-expired.component';
import { ReferralSearchResultsComponent } from './components/referral-search-results/referral-search-results.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationSummaryComponent } from './components/pagination-summary/pagination-summary.component';
import { UsefulInformationPageComponent } from './volunteer/pages/useful-information-page/useful-information-page.component';
import { LocalAuthorityRegistrationsPageComponent } from './local-authority/pages/local-authority-registrations-page/local-authority-registrations-page.component';
import { AdminRegistrationsPageComponent } from './provincial-admin/pages/admin-registrations-page/admin-registrations-page.component';
import { LocalAuthorityVolunteersPageComponent } from './local-authority/pages/local-authority-volunteers-page/local-authority-volunteers-page.component';
import { ProvincialAdminVolunteersOrganizationPageComponent } from './provincial-admin/pages/volunteers-organization/provincial-admin-volunteers-organization-page.component';
import { OrganizationsPageComponent } from './provincial-admin/pages/organizations-page/organizations-page.component';
import { TopNavbarComponent } from './components/top-navbar/top-navbar.component';
import { ProvincialAdminTaskNumbersPageComponent } from './provincial-admin/pages/task-numbers/provincial-admin-task-numbers-page.component';
import { SideboxAddRegistrationComponent } from './components/sidebox-add-registration/sidebox-add-registration.component';
import { SideboxAddOrganizationComponent } from './components/sidebox-add-organization/sidebox-add-organization.component';
import { SideboxAddTaskNumberComponent } from './components/sidebox-add-task-number/sidebox-add-task-number.component';
import { SideboxAddVolunteerComponent } from './components/sidebox-add-volunteer/sidebox-add-volunteer.component';
import { SideboxCollectionNoticeComponent } from './components/sidebox-collection-notice/sidebox-collection-notice.component';

// shared modals // TODO: move to shared module
import { LodgingRatesComponent } from './shared/modals/lodging-rates/lodging-rates.component';
import { ClothingRatesComponent } from './shared/modals/clothing-rates/clothing-rates.component';
import { ConfirmModalComponent } from './shared/modals/confirm/confirm.component';
import { FoodRatesComponent } from './shared/modals/food-rates/food-rates.component';
import { IncidentalsRatesComponent } from './shared/modals/incidentals-rates/incidentals-rates.component';
import { SessionExpiringModalComponent } from './shared/modals/session-expiring/session-expiring.component';
import { TransportationRatesComponent } from './shared/modals/transportation-rates/transportation-rates.component';

// TODO: move these to volunteer/local-authority/provincial-admin modules
import { ReferralMakerComponent } from './volunteer/components/referral-maker/referral-maker.component';
import { ReferralViewComponent } from './volunteer/pages/referral-view/referral-view.component';
import { ReferralTableComponent } from './volunteer/components/referral-table/referral-table.component';
import { ValidFromToComponent } from './volunteer/components/referral-maker/subcomponents/valid-from-to/valid-from-to.component';
import { VolunteerRegistrationsPageComponent } from './volunteer/pages/volunteer-registrations-page/volunteer-registrations-page.component';
import { EvacueeListComponent } from './volunteer/components/evacuee-list/evacuee-list.component';
import { ScrollToModule } from '@nicky-lenaers/ngx-scroll-to';
import { TopTabBarComponent } from './components/top-tab-bar/top-tab-bar.component';
import { AdminVolunteerMakerComponent } from './provincial-admin/components/admin-volunteer-maker/admin-volunteer-maker.component';
import { RegistrationPageComponent } from './volunteer/pages/registration-page/registration-page.component';
import { VolunteerMakerPageComponent } from './local-authority/pages/volunteer-maker-page/volunteer-maker-page.component';
import { OrganizationMakerPageComponent } from './provincial-admin/pages/organization-maker-page/organization-maker-page.component';
import { FullRegistrationSummaryBlockComponent } from './components/full-registration-summary-block/full-registration-summary-block.component';
import { SurveyPageComponent } from './volunteer/pages/survey-page/survey-page.component';
import { EvacueePickerComponent } from './volunteer/components/referral-maker/subcomponents/evacuee-picker/evacuee-picker.component';
import { LodgingReferralComponent } from './volunteer/components/referral-maker/subcomponents/lodging-referral/lodging-referral.component';
import { ClothingReferralComponent } from './volunteer/components/referral-maker/subcomponents/clothing-referral/clothing-referral.component';
import { FoodReferralComponent } from './volunteer/components/referral-maker/subcomponents/food-referral/food-referral.component';
import { IncidentalsReferralComponent } from './volunteer/components/referral-maker/subcomponents/incidentals-referral/incidentals-referral.component';
import { TransportationReferralComponent } from './volunteer/components/referral-maker/subcomponents/transportation-referral/transportation-referral.component';
import { ReferralListItemComponent } from './volunteer/components/referral-maker/subcomponents/referral-list-item/referral-list-item.component';
import { AbstractReferralComponent } from './volunteer/components/referral-maker/subcomponents/abstract-referral/abstract-referral.component';
import { SupplierComponent } from './volunteer/components/referral-maker/subcomponents/supplier/supplier.component';
import { AuditorComponent } from './provincial-admin/components/auditor/auditor.component';
import { ElectronicAccessAgreementComponent } from './components/electronic-access-agreement/electronic-access-agreement.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LocalAuthorityRegistrationsPageComponent,
    LocalAuthorityVolunteersPageComponent,
    RegistrationSummaryComponent,
    EvacueePickerComponent,
    EvacueeListComponent,
    RegistrationMakerComponent,
    RegistrationSummaryFullComponent,
    VolunteerMakerComponent,
    VolunteerListComponent,
    VolunteerOrganizationListComponent,
    VolunteerRegistrationsPageComponent,
    AdminRegistrationsPageComponent,
    ProvincialAdminVolunteersOrganizationPageComponent,
    OrganizationsPageComponent,
    ProvincialAdminTaskNumbersPageComponent,
    SideboxAddRegistrationComponent,
    SideboxAddOrganizationComponent,
    SideboxAddTaskNumberComponent,
    SideboxAddVolunteerComponent,
    SideboxCollectionNoticeComponent,
    ReferralSearchResultsComponent,
    OrganizationListComponent,
    OrganizationMakerComponent,
    TaskNumberMakerComponent,
    TopNavbarComponent,
    TesterPageComponent,
    TaskNumberListComponent,
    SessionExpiredComponent,
    SearchBarComponent,
    PaginationSummaryComponent,
    UsefulInformationPageComponent,
    LodgingRatesComponent,
    ClothingRatesComponent,
    ConfirmModalComponent,
    FoodRatesComponent,
    IncidentalsRatesComponent,
    SessionExpiringModalComponent,
    TransportationRatesComponent,
    ReferralMakerComponent,
    ReferralViewComponent,
    LodgingReferralComponent,
    ClothingReferralComponent,
    FoodReferralComponent,
    IncidentalsReferralComponent,
    TransportationReferralComponent,
    ReferralTableComponent,
    ValidFromToComponent,
    SupplierComponent,
    ReferralListItemComponent,
    AbstractReferralComponent,
    TopTabBarComponent,
    AdminVolunteerMakerComponent,
    RegistrationPageComponent,
    VolunteerMakerPageComponent,
    OrganizationMakerPageComponent,
    FullRegistrationSummaryBlockComponent,
    SurveyPageComponent,
    AuditorComponent,
    ElectronicAccessAgreementComponent,
  ],
  imports: [
    // angular
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,

    // 3rd party
    StoreModule.forRoot(rootReducer, { metaReducers }),
    ScrollToModule.forRoot(),

    // core & shared
    CoreModule,
    SharedModule,

    // app
    AppRoutingModule,
    SelfRegistrationModule,
  ],
  entryComponents: [
    LodgingRatesComponent,
    ClothingRatesComponent,
    ConfirmModalComponent,
    FoodRatesComponent,
    IncidentalsRatesComponent,
    SessionExpiringModalComponent,
    TransportationRatesComponent,
  ],
  bootstrap: [
    AppComponent
  ],
  providers: [
    { provide: APP_BASE_HREF, useValue: '/' }
  ]
})
export class AppModule { }
