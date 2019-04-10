import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

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
import { EvacueeRegistrationModule } from './evacuee-registration';
import { SelfRegistrationModule } from './self-registration';
import { SharedModule } from './shared/shared.module';
import { VolunteerTeamDashboardComponent } from './volunteer-team-dashboard/volunteer-team-dashboard.component';
import { VolunteerUsefulInformationComponent } from './volunteer-useful-information/volunteer-useful-information.component';
import { EvacueeSummaryComponent } from './evacuee-summary/evacuee-summary.component';
import { AdminAddTaskNumberModule } from './admin-add-task-number';
import { AdminDashboardModule } from './admin-dashboard';
import { TopNavigationComponent } from './top-navigation/top-navigation.component';
import { VolunteerLayoutComponent } from './volunteers/containers/volunteer-layout/volunteer-layout.component';
import { AdminTaskNumbersComponent } from './admin-task-numbers/admin-task-numbers.component';
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
    VolunteerLayoutComponent,
    AdminTaskNumbersComponent,
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
  ],
  imports: [
    // angular
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    NgbPaginationModule,
    // 3rd party
    StoreModule.forRoot(rootReducer, { metaReducers }),

    // core & shared
    CoreModule,
    SharedModule,

    // app
    AppRoutingModule,
    SelfRegistrationModule,
    EvacueeRegistrationModule,
    VolunteerEditorModule,
    AdminAddTaskNumberModule,
    AdminDashboardModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
