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
// import { RegistrationSummaryPageComponent } from './registration-summary-page/registration-summary-page.component';
import { VolunteerEditorContainerComponent } from './volunteer-editor-container/volunteer-editor-container.component';
import { TaskNumberMakerComponent } from './task-number-maker/task-number-maker.component';
import { VolunteerMakerComponent } from './volunteer-maker/volunteer-maker.component';
import { OrganizationMakerComponent } from './organization-maker/organization-maker.component';
import { RegistrationSummaryFullComponent } from './registration-summary-full/registration-summary-full.component';
import { VolunteerOrganizationListComponent } from './volunteer-organization-list/volunteer-organization-list.component';
import { RegistrationMakerComponent } from './registration-maker/registration-maker.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { SessionExpiredComponent } from './session-expired/session-expired.component';
import { RegistrationPageComponent } from './registration-page/registration-page.component';


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
    // RegistrationSummaryPageComponent,
    VolunteerEditorContainerComponent,
    TaskNumberMakerComponent,
    VolunteerMakerComponent,
    OrganizationMakerComponent,
    RegistrationSummaryFullComponent,
    VolunteerOrganizationListComponent,
    RegistrationMakerComponent,
    RegistrationPageComponent,
    AdminDashboardComponent,
    SessionExpiredComponent,
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
