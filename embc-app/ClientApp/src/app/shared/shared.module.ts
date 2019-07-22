import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TextMaskModule } from 'angular2-text-mask';
import { NgbTypeaheadModule, NgbPaginationModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

import { EnvironmentBannerComponent } from './components/environment-banner/environment-banner.component';
import { WarningBannerComponent } from './components/warning-banner/warning-banner.component';
import { NotificationBannerComponent } from './components/notification-banner/notification-banner.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { SideInfoboxComponent } from './components/side-infobox/side-infobox.component';
import { SideBoxComponent } from './components/side-box/side-box.component';
import { FormFieldComponent } from './components/form-field/form-field.component';
import { AddressSelectorComponent } from './components/address-form/address-selector.component';
import { BcAddressComponent } from './components/address-form/bc-address/bc-address.component';
import { OtherAddressComponent } from './components/address-form/other-address/other-address.component';
import { FontAwesomeIconComponent } from './components/font-awesome-icon/font-awesome-icon.component';
import { FontAwesomeLinkComponent } from './components/fa-link/fa-link.component';
import { AttentionIconComponent } from './components/attention-icon/attention-icon.component';
import { CommunitiesSelectComponent } from './components/communities-select/communities-select.component';
import { DateTimePickerComponent } from './components/date-time-picker/date-time-picker.component';
import { CaptchaComponent } from './components/captcha/captcha.component';
import { PaginatorComponent } from './components/paginator/paginator.component';
import { FullRegistrationSummaryBlockComponent } from './components/full-registration-summary-block/full-registration-summary-block.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

import { ByIdPipe } from './pipes/by-id.pipe';
import { DateTimeFormatPipe } from './pipes/date-time-format.pipe';
import { NewlinesPipe } from './pipes/newlines.pipe';
import { PluckPipe } from './pipes/pluck.pipe';
import { YesNoPipe } from './pipes/yes-no.pipe';

// directives
import { DisableControlDirective } from './directives/disable-control.directive';
import { UpperCaseDirective } from './directives/upper-case.directive';
import { ValidationBorderDirective } from './directives/validation-border.directive';
import { RegistrationSummaryFullComponent } from './components/registration-summary-full/registration-summary-full.component';
import { TopNavbarComponent } from './components/top-navbar/top-navbar.component';

@NgModule({
  declarations: [
    EnvironmentBannerComponent,
    WarningBannerComponent,
    NotificationBannerComponent,
    HeaderComponent,
    FooterComponent,
    SideInfoboxComponent,
    SideBoxComponent,
    FormFieldComponent,
    AddressSelectorComponent,
    BcAddressComponent,
    OtherAddressComponent,
    FontAwesomeIconComponent,
    FontAwesomeLinkComponent,
    AttentionIconComponent,
    CommunitiesSelectComponent,
    FullRegistrationSummaryBlockComponent,
    RegistrationSummaryFullComponent,
    TopNavbarComponent,

    ByIdPipe,
    DateTimeFormatPipe,
    NewlinesPipe,
    PluckPipe,
    YesNoPipe,
    PageNotFoundComponent,
    DateTimePickerComponent,
    CaptchaComponent,
    PaginatorComponent,
    DisableControlDirective,
    UpperCaseDirective,
    ValidationBorderDirective,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    TextMaskModule,

    // ng-bootstrap UI components
    NgbTypeaheadModule,
    NgbPaginationModule,
    NgbModalModule,
  ],
  exports: [
    // modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TextMaskModule,

    // ng-bootstrap components we want to re-export... (they are used outside of this SharedModule)
    NgbPaginationModule,
    NgbModalModule,

    // components, pipes, etc
    EnvironmentBannerComponent,
    WarningBannerComponent,
    NotificationBannerComponent,
    HeaderComponent,
    FooterComponent,
    SideInfoboxComponent,
    SideBoxComponent,
    FormFieldComponent,
    AddressSelectorComponent,
    BcAddressComponent,
    OtherAddressComponent,
    FontAwesomeIconComponent,
    FontAwesomeLinkComponent,
    AttentionIconComponent,
    CommunitiesSelectComponent,
    FullRegistrationSummaryBlockComponent,
    RegistrationSummaryFullComponent,
    TopNavbarComponent,

    ByIdPipe,
    DateTimeFormatPipe,
    NewlinesPipe,
    PluckPipe,
    YesNoPipe,
    DateTimePickerComponent,
    CaptchaComponent,
    PaginatorComponent,
    DisableControlDirective,
    UpperCaseDirective,
    ValidationBorderDirective,
  ]
})
export class SharedModule { }
