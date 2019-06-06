import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
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
import { RegistrationSummaryComponent } from './registration-summary/registration-summary.component';
import { TaskNumberListComponent } from './provincial-admin/components/task-number-list/task-number-list.component';
import { VolunteerListComponent } from './local-authority/components/volunteer-list/volunteer-list.component';
import { OrganizationListComponent } from './provincial-admin/components/organization-list/organization-list.component';
import { TaskNumberMakerComponent } from './provincial-admin/components/task-number-maker/task-number-maker.component';
import { VolunteerMakerComponent } from './volunteer-maker/volunteer-maker.component';
import { OrganizationMakerComponent } from './provincial-admin/components/organization-maker/organization-maker.component';
import { RegistrationSummaryFullComponent } from './registration-summary-full/registration-summary-full.component';
import { VolunteerOrganizationListComponent } from './provincial-admin/components/volunteer-organization-list/volunteer-organization-list.component';
import { LayoutMainComponent } from './components/layout-main/layout-main.component';
import { LayoutSideComponent } from './components/layout-side/layout-side.component';
import { RegistrationMakerComponent } from './registration-maker/registration-maker.component';
import { SessionExpiredComponent } from './session-expired/session-expired.component';
import { ReferralSearchResultsComponent } from './components/referral-search-results/referral-search-results.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationSummaryComponent } from './components/pagination-summary/pagination-summary.component';
import { UsefulInformationPageComponent } from './volunteer/pages/useful-information-page/useful-information-page.component';
import { LocalAuthorityRegistrationsPageComponent } from './local-authority/pages/local-authority-registrations-page/local-authority-registrations-page.component';
import { ProvincialAdminRegistrationsPageComponent } from './provincial-admin/pages/registrations/provincial-admin-registrations-page.component';
import { LocalAuthorityVolunteersPageComponent } from './local-authority/pages/local-authority-volunteers-page/local-authority-volunteers-page.component';
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
import { LodgingRatesComponent } from './shared/modals/lodging-rates/lodging-rates.component';
import { ClothingRatesComponent } from './shared/modals/clothing-rates/clothing-rates.component';
import { ConfirmModalComponent } from './shared/modals/confirm/confirm.component';
import { FoodRatesComponent } from './shared/modals/food-rates/food-rates.component';
import { IncidentalsRatesComponent } from './shared/modals/incidentals-rates/incidentals-rates.component';
import { SessionExpiringModalComponent } from './shared/modals/session-expiring/session-expiring.component';
import { TransportationRatesComponent } from './shared/modals/transportation-rates/transportation-rates.component';

// TODO: move these to volunteer/local-authority/provincial-admin modules
import { ReferralMakerComponent } from './volunteer/pages/referral-maker/referral-maker.component';
import { ReferralViewComponent } from './volunteer/pages/referral-view/referral-view.component';
import { LodgingReferralComponent } from './volunteer/components/lodging-referral/lodging-referral.component';
import { ClothingReferralComponent } from './volunteer/components/clothing-referral/clothing-referral.component';
import { FoodReferralComponent } from './volunteer/components/food-referral/food-referral.component';
import { IncidentalsReferralComponent } from './volunteer/components/incidentals-referral/incidentals-referral.component';
import { TransportationReferralComponent } from './volunteer/components/transportation-referral/transportation-referral.component';
import { ReferralTableComponent } from './volunteer/components/referral-table/referral-table.component';
import { ValidFromToComponent } from './volunteer/components/valid-from-to/valid-from-to.component';
import { SupplierComponent } from './volunteer/components/supplier/supplier.component';
import { VolunteerRegistrationsPageComponent } from './volunteer/pages/volunteer-registrations-page/volunteer-registrations-page.component';
import { ReferralListItemComponent } from './volunteer/components/referral-list-item/referral-list-item.component';
import { AbstractReferralComponent } from './volunteer/components/abstract-referral/abstract-referral.component';
import { EvacueePickerComponent } from './volunteer/components/evacuee-picker/evacuee-picker.component';
import { EvacueeListComponent } from './volunteer/components/evacuee-list/evacuee-list.component';
import { ScrollToModule } from '@nicky-lenaers/ngx-scroll-to';

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
    ProvincialAdminRegistrationsPageComponent,
    ProvincialAdminVolunteersOrganizationPageComponent,
    ProvincialAdminOrganizationsPageComponent,
    ProvincialAdminTaskNumbersPageComponent,
    SideboxAddRegistrationComponent,
    SideboxAddOrganizationComponent,
    SideboxAddTaskNumberComponent,
    SideboxAddVolunteerComponent,
    SideboxCollectionNoticeComponent,
    ReferralSearchResultsComponent,
    LayoutMainComponent,
    LayoutSideComponent,
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
  ]
})
export class AppModule { }
