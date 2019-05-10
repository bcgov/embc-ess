import { Component, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import * as moment from 'moment';

// tslint:disable: no-use-before-declare
const DATE_TIME_PICKER_PROVIDER = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DateTimePickerComponent),
  multi: true
};

@Component({
  selector: 'app-date-time-picker',
  templateUrl: './date-time-picker.component.html',
  styleUrls: ['./date-time-picker.component.scss'],
  providers: [DATE_TIME_PICKER_PROVIDER]
})
export class DateTimePickerComponent implements ControlValueAccessor {

  @Input() disabled = false;
  @Input() value: Date = null;
  @Output() valueChange = new EventEmitter<Date>();

  readonly dateMask = [/\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/]; // yyyy-mm-dd
  readonly timeMask = [/\d/, /\d/, ':', /\d/, /\d/, ' ', /[a|A|p|P]/, /[m|M]/]; // hh:mm xx

  dateString: string = null;
  timeString: string = null;

  constructor() { }

  //
  // #region "ControlValueAccessor methods"
  //

  // This is a placeholder method that we use to emit changes back to the form.
  onChange = (_: any) => { };
  onTouched = () => { };

  // This is a basic setter that the Angular forms API is going to use.
  writeValue(value: any): void {
    if (value) {
      const m = moment(value);
      this.dateString = m.format('YYYY-MM-DD');
      this.timeString = m.format('hh:mm a');
    }
  }

  // This allows Angular to register a function to call when the model changes.
  // This is how we emit the changes back to the form.
  registerOnChange(fn: any): void { this.onChange = fn; }

  registerOnTouched(fn: any): void { this.onTouched = fn; }

  // This allows Angular to disable this component.
  setDisabledState(isDisabled: boolean): void { this.disabled = isDisabled; }

  //
  // #endregion "ControlValueAccessor methods"
  //

  updateDate(newValue: string): void {
    this.dateString = newValue;
    this.propagateModelChange();
  }

  updateTime(newValue: string): void {
    this.timeString = newValue;
    this.propagateModelChange();
  }

  private propagateModelChange(touched = true): void {
    if (touched) {
      this.onTouched();
    }

    const dateTime = this.dateString + ' ' + this.timeString;
    const m = moment(dateTime, 'YYYY-MM-DD hh:mm a', true);

    // emit change events
    if (m.isValid()) {
      const jsDate = m.toDate();
      this.valueChange.emit(jsDate);
      this.onChange(jsDate);
    } else {
      this.valueChange.emit(null);
      this.onChange(null);
    }
  }

}
