import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MaterialModule } from './material.module';
import { WarningBannerComponent } from './components/warning-banner/warning-banner.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { SideBoxComponent } from './components/side-box/side-box.component';
import { SideBoxInfoComponent } from './components/side-box-info/side-box-info.component';
import { FormFieldComponent } from './components/form-field/form-field.component';
import { AddressSelectorComponent } from './components/address-form/address-selector.component';
import { BcAddressComponent } from './components/address-form/bc-address/bc-address.component';
import { OtherAddressComponent } from './components/address-form/other-address/other-address.component';
import { DisableControlDirective } from './directives/disable-control.directive';
import { FontAwesomeIconComponent } from './components/font-awesome-icon/font-awesome-icon.component';
import { FontAwesomeLinkComponent } from './components/fa-link/fa-link.component';
import { AttentionIconComponent } from './components/attention-icon/attention-icon.component';
import { SearchBarComponent, EvacueeSearchResultsComponent } from './components/search/';

import { YesNoPipe } from './pipes/yes-no.pipe';
import { ByIdPipe } from './pipes/by-id.pipe';
import { PluckPipe } from './pipes/pluck.pipe';
import { AddEvacueeComponent, AddOrganizationComponent, AddTaskNumberComponent, AddUserComponent } from './components/side-boxes';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { PaginationSummaryComponent } from './components/pagination-summary/pagination-summary.component';

@NgModule({
  declarations: [
    WarningBannerComponent,
    HeaderComponent,
    FooterComponent,
    SideBoxComponent,
    SideBoxInfoComponent,
    FormFieldComponent,
    AddressSelectorComponent,
    BcAddressComponent,
    OtherAddressComponent,
    DisableControlDirective,
    FontAwesomeIconComponent,
    FontAwesomeLinkComponent,
    AttentionIconComponent,
    SearchBarComponent,
    EvacueeSearchResultsComponent,
    YesNoPipe,
    ByIdPipe,
    PluckPipe,
    AddEvacueeComponent,
    AddOrganizationComponent,
    AddTaskNumberComponent,
    AddUserComponent,
    PageNotFoundComponent,
    PaginationSummaryComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
  ],
  exports: [
    // modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,

    // components, pipes, etc
    WarningBannerComponent,
    HeaderComponent,
    FooterComponent,
    SideBoxComponent,
    SideBoxInfoComponent,
    FormFieldComponent,
    AddressSelectorComponent,
    BcAddressComponent,
    OtherAddressComponent,
    DisableControlDirective,
    FontAwesomeIconComponent,
    FontAwesomeLinkComponent,
    AttentionIconComponent,
    SearchBarComponent,
    EvacueeSearchResultsComponent,
    YesNoPipe,
    ByIdPipe,
    PluckPipe,
    AddEvacueeComponent,
    AddOrganizationComponent,
    AddTaskNumberComponent,
    AddUserComponent,
    PaginationSummaryComponent,
  ]
})
export class SharedModule { }
