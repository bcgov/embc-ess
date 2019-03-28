import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SharedModule } from './../shared/shared.module';
import { AdminDashboardComponent } from './admin-dashboard.component';
import { AdminEvacueesComponent } from './admin-evacuees/admin-evacuees.component';
import { AdminOrganizationsComponent } from './admin-organizations/admin-organizations.component';
import { AdminTaskNumbersComponent } from './admin-task-numbers/admin-task-numbers.component';

@NgModule({
    declarations: [
        AdminDashboardComponent,
        AdminEvacueesComponent,
        AdminOrganizationsComponent,
        AdminTaskNumbersComponent,
    ],
    imports: [
        CommonModule,
        RouterModule,
        SharedModule,
    ],
    exports: [
        AdminDashboardComponent,
    ]
})
export class AdminDashboardModule { }