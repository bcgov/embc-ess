import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SharedModule } from './../shared/shared.module';

import { VolunteerEditorComponent } from './volunteer-editor.component';
import { VolunteerEditorOneComponent } from './volunteer-editor-one/volunteer-editor-one.component';
import { VolunteerEditorConfirmationComponent } from './volunteer-editor-confirmation/volunteer-editor-confirmation.component';


@NgModule({
    declarations: [
        VolunteerEditorComponent,
        VolunteerEditorOneComponent,
        VolunteerEditorConfirmationComponent
    ],
    imports: [
        CommonModule,
        RouterModule,
        SharedModule,
    ],
    exports: [
        VolunteerEditorComponent
    ]
})
export class VolunteerEditorModule { }
