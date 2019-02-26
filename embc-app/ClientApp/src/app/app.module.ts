import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// ngrx + configuration
import { StoreModule } from "@ngrx/store";
import { rootReducer } from "./store/reducers/root";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { HomeComponent } from './home/home.component';
import { SelfRegistrationModule } from './self-registration';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
  ],
  imports: [
    // angular
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,

    // 3rd party
    StoreModule.forRoot(rootReducer),

    // core & shared
    CoreModule,
    SharedModule,

    // app
    AppRoutingModule,
    SelfRegistrationModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
