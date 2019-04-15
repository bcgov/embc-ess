import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SharedModule } from './../shared/shared.module';

import { EvacueeRegistrationComponent } from './evacuee-registration.component';
// import { EvacueeRegistrationConfirmationComponent } from './evacuee-registration-confirmation/evacuee-registration-confirmation.component';
import { RegistrationMakerComponent } from './registration-maker/registration-maker.component';

@NgModule({
    declarations: [
        EvacueeRegistrationComponent,
        // EvacueeRegistrationConfirmationComponent,
        RegistrationMakerComponent,
    ],
    imports: [
        CommonModule,
        RouterModule,
        SharedModule,
    ],
    exports: [
        EvacueeRegistrationComponent,
    ]
})
export class EvacueeRegistrationModule { }
