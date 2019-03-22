import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// ngrx + configuration
import { StoreModule } from '@ngrx/store';
import { rootReducer, metaReducers } from './store';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { SelfRegistrationModule } from './self-registration';
import { EvacueeRegistrationModule } from './evacuee-registration';
import { HomeComponent } from './home/home.component';
import { VolunteerLoginComponent } from './volunteer-login/volunteer-login.component';
import { VolunteerDashboardComponent } from './volunteer-dashboard/volunteer-dashboard.component';
// import { EvacueeRegistrationComponent } from './evacuee-registration/evacuee-registration.component';
import { TesterPageComponent } from './tester-page/tester-page.component';
import { EssEditorComponent } from './ess-editor/ess-editor.component';
import { EssEditorOneComponent } from './ess-editor/ess-editor-one/ess-editor-one.component';
import { EssEditorConfirmationComponent } from './ess-editor/ess-editor-confirmation/ess-editor-confirmation.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    VolunteerLoginComponent,
    VolunteerDashboardComponent,
    // EvacueeRegistrationComponent,
    TesterPageComponent,
    EssEditorComponent,
    EssEditorOneComponent,
    EssEditorConfirmationComponent,
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
    EvacueeRegistrationModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
