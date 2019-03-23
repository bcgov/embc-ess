import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SharedModule } from './../shared/shared.module';

import { EssEditorComponent } from './ess-editor.component';
import { EssEditorOneComponent } from './ess-editor-one/ess-editor-one.component';
import { EssEditorConfirmationComponent } from './ess-editor-confirmation/ess-editor-confirmation.component';


@NgModule({
    declarations: [
        EssEditorComponent,
        EssEditorOneComponent,
        EssEditorConfirmationComponent
    ],
    imports: [
        CommonModule,
        RouterModule,
        SharedModule,
    ],
    exports: [
        EssEditorComponent
    ]
})
export class EssEditorModule { }
