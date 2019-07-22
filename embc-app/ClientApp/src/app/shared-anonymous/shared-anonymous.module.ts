import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TextMaskModule } from 'angular2-text-mask';
import { NgbTypeaheadModule, NgbPaginationModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FormFieldComponent } from './components/form-field/form-field.component';
import { FontAwesomeIconComponent } from './components/font-awesome-icon/font-awesome-icon.component';


@NgModule({
  declarations: [
    FormFieldComponent,
    FontAwesomeIconComponent,

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

  ]
})
export class SharedAnonymousModule { }
