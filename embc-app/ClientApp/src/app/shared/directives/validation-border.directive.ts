import { Directive, Self, HostBinding, Input } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appValidationBorder]'
})
export class ValidationBorderDirective {

  // tslint:disable-next-line: no-input-rename
  @Input('appValidationBorderShowOnError') showOnError = true;

  @HostBinding('class.is-invalid')
  get showBorder(): boolean {
    if (!this.formControl || !this.showOnError) { return false; }
    const { dirty, touched } = this.formControl;
    return this.invalid ? (dirty || touched) : false;
  }

  get invalid(): boolean {
    return this.formControl ? this.formControl.invalid : false;
  }

  constructor(@Self() private formControl: NgControl) { }
}
