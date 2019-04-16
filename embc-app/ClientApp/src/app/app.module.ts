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
import { EvacueeSummaryComponent } from './evacuee-summary/evacuee-summary.component';
import { TopNavigationComponent } from './top-navigation/top-navigation.component';
import { TaskNumberListComponent } from './task-number-list/task-number-list.component';
import { VolunteerListComponent } from './volunteer-list/volunteer-list.component';
import { EvacueeListComponent } from './evacuee-list/evacuee-list.component';
import { OrganizationListComponent } from './organization-list/organization-list.component';
import { UsefulInformationContentComponent } from './useful-information-content/useful-information-content.component';
import { EvacueeSummaryContainerComponent } from './evacuee-summary-container/evacuee-summary-container.component';
import { VolunteerEditorContainerComponent } from './volunteer-editor-container/volunteer-editor-container.component';
import { TaskNumberMakerComponent } from './task-number-maker/task-number-maker.component';
import { VolunteerMakerComponent } from './volunteer-maker/volunteer-maker.component';
import { OrganizationMakerComponent } from './organization-maker/organization-maker.component';
import { EvacueeSummaryPageComponent } from './evacuee-summary-page/evacuee-summary-page.component';
import { VolunteerOrganizationListComponent } from './volunteer-organization-list/volunteer-organization-list.component';
import { LayoutMainComponent } from './components/layout-main/layout-main.component';
import { LayoutSideComponent } from './components/layout-side/layout-side.component';
import { VolunteerDashboardPageComponent } from './pages/volunteer/volunteer-dashboard-page/volunteer-dashboard-page.component';
import { RegistrationMakerComponent } from './registration-maker/registration-maker.component';
import { EvacueeRegistrationComponent } from './evacuee-registration/evacuee-registration.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { SessionExpiredComponent } from './session-expired/session-expired.component';
import { EvacueeSearchResultsComponent } from './components/evacuee-search-results/evacuee-search-results.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    VolunteerDashboardComponent,
    TesterPageComponent,
    VolunteerTeamDashboardComponent,
    VolunteerUsefulInformationComponent,
    EvacueeSummaryComponent,
    TopNavigationComponent,
    // VolunteerLayoutComponent,
    TaskNumberListComponent,
    VolunteerListComponent,
    EvacueeListComponent,
    OrganizationListComponent,
    UsefulInformationContentComponent,
    EvacueeSummaryContainerComponent,
    VolunteerEditorContainerComponent,
    TaskNumberMakerComponent,
    VolunteerMakerComponent,
    OrganizationMakerComponent,
    EvacueeSummaryPageComponent,
    VolunteerOrganizationListComponent,
    LayoutMainComponent,
    LayoutSideComponent,
    VolunteerDashboardPageComponent,
    RegistrationMakerComponent,
    EvacueeRegistrationComponent,
    AdminDashboardComponent,
    SessionExpiredComponent,
    // SearchBarComponent,
    EvacueeSearchResultsComponent,
    SearchBarComponent,
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
