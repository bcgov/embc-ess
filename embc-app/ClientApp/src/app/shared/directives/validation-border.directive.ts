import { Directive, Self, HostBinding, Input } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appErrorBorder]'
})
export class ValidationBorderDirective {
  @Input() when = true;

  @HostBinding('class.is-invalid') get show() {
    return this.invalid && this.when;
  }

  get invalid() {
    return this.formControl ? this.formControl.invalid : false;
  }

  constructor(@Self() private formControl: NgControl) { }
}
