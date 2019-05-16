import { Directive, HostListener, Input } from '@angular/core';

//
// Ref: https://github.com/KingMario/packages
//

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: 'input[appUpperCase],textarea[appUpperCase]',
})
export class UpperCaseDirective {

  @Input() appUpperCase: string;

  private getCaret(el: HTMLInputElement) {
    return {
      start: el.selectionStart,
      end: el.selectionEnd,
    };
  }

  private setCaret(el: HTMLInputElement, start: number, end: number) {
    el.selectionStart = start;
    el.selectionEnd = end;
    el.focus();
  }

  private dispatchEvent(el: HTMLInputElement, eventType: string) {
    const event = document.createEvent('Event');
    event.initEvent(eventType, false, false);
    el.dispatchEvent(event);
  }

  private convertValue(el: HTMLInputElement, value: string) {
    el.value = value.toUpperCase();
    this.dispatchEvent(el, 'input');
  }

  @HostListener('blur', ['$event.target', '$event.target.value'])
  onBlur(el: HTMLInputElement, value: string): void {
    if ((!this.appUpperCase || 'blur' === this.appUpperCase) && 'function' === typeof value.toUpperCase && value.toUpperCase() !== value) {
      this.convertValue(el, value);
      this.dispatchEvent(el, 'blur'); // in case updateOn is set to blur
    }
  }

  @HostListener('input', ['$event.target', '$event.target.value'])
  onInput(el: HTMLInputElement, value: string): void {
    if (!this.appUpperCase && 'function' === typeof value.toUpperCase && value.toUpperCase() !== value) {
      let { start, end } = this.getCaret(el);
      if (value[0] === ' ' && start === 1 && end === 1) {
        start = 0;
        end = 0;
      }
      this.convertValue(el, value);
      this.setCaret(el, start, end);
    }
  }

}
