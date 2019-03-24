import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// ngrx + configuration
import { AppComponent } from './app.component';
import { StoreModule } from '@ngrx/store';
import { rootReducer, metaReducers } from './store';

import { HomeComponent } from './home/home.component';
import { VolunteerLoginComponent } from './volunteer-login/volunteer-login.component';
import { VolunteerDashboardComponent } from './volunteer-dashboard/volunteer-dashboard.component';
import { TesterPageComponent } from './tester-page/tester-page.component';

import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';
import { EssEditorModule } from './ess-editor';
import { EvacueeRegistrationModule } from './evacuee-registration';
import { SelfRegistrationModule } from './self-registration';
import { SharedModule } from './shared/shared.module';
import { VolunteerUsefulInformationComponent } from './volunteer-useful-information/volunteer-useful-information.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    VolunteerLoginComponent,
    VolunteerDashboardComponent,
    TesterPageComponent,
    VolunteerUsefulInformationComponent,
  ],
  imports: [
    // angular
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,

    // 3rd party
    StoreModule.forRoot(rootReducer, { metaReducers }),

    // core & shared
    CoreModule,
    SharedModule,

    // app
    AppRoutingModule,
    SelfRegistrationModule,
    EvacueeRegistrationModule,
    EssEditorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
