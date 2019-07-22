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
import { DateTimePickerComponent } from './components/date-time-picker/date-time-picker.component';
import { PaginatorComponent } from './components/paginator/paginator.component';
import { FullRegistrationSummaryBlockComponent } from './components/full-registration-summary-block/full-registration-summary-block.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { SideboxAddRegistrationComponent } from './components/sideboxes/sidebox-add-registration/sidebox-add-registration.component';
import { SideboxAddOrganizationComponent } from './components/sideboxes/sidebox-add-organization/sidebox-add-organization.component';
import { SideboxAddTaskNumberComponent } from './components/sideboxes/sidebox-add-task-number/sidebox-add-task-number.component';
import { SideboxAddVolunteerComponent } from './components/sideboxes/sidebox-add-volunteer/sidebox-add-volunteer.component';
import { SideboxCollectionNoticeComponent } from './components/sideboxes/sidebox-collection-notice/sidebox-collection-notice.component';

import { ByIdPipe } from './pipes/by-id.pipe';
import { DateTimeFormatPipe } from './pipes/date-time-format.pipe';
import { NewlinesPipe } from './pipes/newlines.pipe';
import { PluckPipe } from './pipes/pluck.pipe';

// directives
import { DisableControlDirective } from './directives/disable-control.directive';
import { UpperCaseDirective } from './directives/upper-case.directive';
import { ValidationBorderDirective } from './directives/validation-border.directive';
import { RegistrationSummaryFullComponent } from './components/registration-summary-full/registration-summary-full.component';
import { TopNavbarComponent } from './components/top-navbar/top-navbar.component';
import { PaginationSummaryComponent } from './components/pagination-summary/pagination-summary.component';
import { ReferralSearchResultsComponent } from './components/referral-search-results/referral-search-results.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { SessionExpiredComponent } from './components/session-expired/session-expired.component';
import { SharedAnonymousModule } from '../shared-anonymous/shared-anonymous.module';

@NgModule({
  declarations: [
    EnvironmentBannerComponent,
    WarningBannerComponent,
    NotificationBannerComponent,
    HeaderComponent,
    FooterComponent,
    SideInfoboxComponent,
    SideBoxComponent,
    FullRegistrationSummaryBlockComponent,
    RegistrationSummaryFullComponent,
    TopNavbarComponent,
    SideboxAddRegistrationComponent,
    SideboxAddOrganizationComponent,
    SideboxAddTaskNumberComponent,
    SideboxAddVolunteerComponent,
    SideboxCollectionNoticeComponent,
    PaginationSummaryComponent,
    ReferralSearchResultsComponent,
    SearchBarComponent,
    SessionExpiredComponent,
    PageNotFoundComponent,
    DateTimePickerComponent,
    PaginatorComponent,

    ByIdPipe,
    DateTimeFormatPipe,
    NewlinesPipe,
    PluckPipe,

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
    SharedAnonymousModule, // this gets the anonymous components when needed.

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
    FullRegistrationSummaryBlockComponent,
    RegistrationSummaryFullComponent,
    TopNavbarComponent,
    PaginationSummaryComponent,
    SideboxAddRegistrationComponent,
    SideboxAddOrganizationComponent,
    SideboxAddTaskNumberComponent,
    SideboxAddVolunteerComponent,
    SideboxCollectionNoticeComponent,
    ReferralSearchResultsComponent,
    SearchBarComponent,
    SessionExpiredComponent,
    DateTimePickerComponent,
    PaginatorComponent,

    ByIdPipe,
    DateTimeFormatPipe,
    NewlinesPipe,
    PluckPipe,

    DisableControlDirective,
    UpperCaseDirective,
    ValidationBorderDirective,
  ]
})
export class SharedModule { }
