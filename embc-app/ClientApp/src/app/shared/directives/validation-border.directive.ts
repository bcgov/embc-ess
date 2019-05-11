import { Directive, Self, HostBinding, Input } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appValidationBorder]'
})
export class ValidationBorderDirective {

  // tslint:disable-next-line: no-input-rename
  @Input('appValidationBorderForce') force = false;

  @HostBinding('class.is-invalid')
  get showBorder(): boolean {
    if (!this.formControl) { return false; }
    const { dirty, touched } = this.formControl;
    return this.invalid ? (this.force || dirty || touched) : false;
  }

  get invalid(): boolean {
    return this.formControl ? this.formControl.invalid : false;
  }

  constructor(@Self() private formControl: NgControl) { }
}
