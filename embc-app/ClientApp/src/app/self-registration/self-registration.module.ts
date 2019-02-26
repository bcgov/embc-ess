import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SharedModule } from './../shared/shared.module';
import { SelfRegistrationComponent } from './self-registration.component';
import { SelfRegistrationOneComponent } from './self-registration-one/self-registration-one.component';
import { SelfRegistrationTwoComponent } from './self-registration-two/self-registration-two.component';
import { SelfRegistrationThreeComponent } from './self-registration-three/self-registration-three.component';
import { SelfRegistrationFourComponent } from './self-registration-four/self-registration-four.component';

@NgModule({
  declarations: [
    SelfRegistrationComponent,
    SelfRegistrationOneComponent,
    SelfRegistrationTwoComponent,
    SelfRegistrationThreeComponent,
    SelfRegistrationFourComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
  ],
  exports: [
    SelfRegistrationComponent,
  ]
})
export class SelfRegistrationModule { }
