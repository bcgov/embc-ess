import { ControlValueAccessor } from '@angular/forms';

export abstract class ValueAccessorBase<T> implements ControlValueAccessor {
  disabled = false;

  private innerValue: T;

  private onChange = (_: any) => { };
  private onTouched = () => { };

  get value(): T {
    return this.innerValue;
  }

  set value(value: T) {
    if (this.innerValue !== value) {
      this.innerValue = value;
      this.onChange(value);
    }
  }

  writeValue(value: T) { this.innerValue = value; }
  registerOnChange(fn: (value: T) => void) { this.onChange = fn; }
  registerOnTouched(fn: () => void) { this.onTouched = fn; }
  setDisabledState(isDisabled: boolean): void { this.disabled = isDisabled; }

  touch() { this.onTouched(); }
}
