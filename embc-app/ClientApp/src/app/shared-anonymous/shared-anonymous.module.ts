import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TextMaskModule } from 'angular2-text-mask';
import { NgbTypeaheadModule, NgbPaginationModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FormFieldComponent } from './components/form-field/form-field.component';
import { FontAwesomeIconComponent } from './components/font-awesome-icon/font-awesome-icon.component';
import { FontAwesomeLinkComponent } from './components/fa-link/fa-link.component';
import { AddressSelectorComponent } from './components/address-form/address-selector.component';
import { BcAddressComponent } from './components/address-form/bc-address/bc-address.component';
import { OtherAddressComponent } from './components/address-form/other-address/other-address.component';
import { AttentionIconComponent } from './components/attention-icon/attention-icon.component';
import { CommunitiesSelectComponent } from './components/communities-select/communities-select.component';
import { YesNoPipe } from './pipes/yes-no.pipe';
import { CaptchaComponent } from './components/captcha/captcha.component';

@NgModule({
  declarations: [
    FormFieldComponent,
    FontAwesomeIconComponent,
    FontAwesomeLinkComponent,
    AddressSelectorComponent,
    BcAddressComponent,
    OtherAddressComponent,
    AttentionIconComponent,
    CommunitiesSelectComponent,
    CaptchaComponent,

    YesNoPipe
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

    // components, pipes, etc
    FormFieldComponent,
    FontAwesomeIconComponent,
    FontAwesomeLinkComponent,
    AddressSelectorComponent,
    BcAddressComponent,
    OtherAddressComponent,
    AttentionIconComponent,
    CommunitiesSelectComponent,
    CaptchaComponent,

    YesNoPipe,
  ]
})
export class SharedAnonymousModule { }
