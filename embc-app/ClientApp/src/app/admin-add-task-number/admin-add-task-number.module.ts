import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SharedModule } from './../shared/shared.module';
import { AdminAddTaskNumberComponent } from './admin-add-task-number.component';
import { AdminAddTaskNumberOneComponent } from './admin-add-task-number-one/admin-add-task-number-one.component';
import { AdminAddTaskNumberConfirmationComponent } from './admin-add-task-number-confirmation/admin-add-task-number-confirmation.component';

@NgModule({
    declarations: [
        AdminAddTaskNumberComponent,
        AdminAddTaskNumberOneComponent,
        AdminAddTaskNumberConfirmationComponent,
    ],
    imports: [
        CommonModule,
        RouterModule,
        SharedModule,
    ],
    exports: [
        AdminAddTaskNumberComponent
    ]
})
export class VolunteerEditorModule { }
