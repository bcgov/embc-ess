import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';

// tslint:disable: no-use-before-declare
const DATETIMEPICKER_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DateTimePickerComponent),
  multi: true
};

interface DateTimeStruct {
  dateString: string;
  hour: number;
  minute: number;
  second: number;
}

@Component({
  selector: 'app-date-time-picker',
  templateUrl: './date-time-picker.component.html',
  styleUrls: ['./date-time-picker.component.scss'],
  providers: [DATETIMEPICKER_VALUE_ACCESSOR]
})
export class DateTimePickerComponent implements OnInit, ControlValueAccessor {

  @Input() disabled = false;
  @Input() value: Date = null;
  @Output() valueChange = new EventEmitter<Date>();

  model: DateTimeStruct = {
    dateString: null,
    hour: 0,
    minute: 0,
    second: 0
  };

  dateString: string = null;
  time: NgbTimeStruct = null;

  constructor() { }

  ngOnInit() {
  }

  // ControlValueAccessor methods follow...

  // This is a placeholder method that we use to emit changes back to the form
  onChange = (_: any) => { };
  onTouched = () => { };

  // This is a basic setter that the Angular forms API is going to use
  writeValue(value: any): void {
    const structValue = this.fromJSDate(value);
    this.setModel(structValue);
  }

  // Allows Angular to register a function to call when the model changes.
  // This is how we emit the changes back to the form
  registerOnChange(fn: any): void { this.onChange = fn; }

  registerOnTouched(fn: any): void { this.onTouched = fn; }

  // Allows Angular to disable this component.
  setDisabledState(isDisabled: boolean): void { this.disabled = isDisabled; }

  // end of ControlValueAccessor

  isValid(model: DateTimeStruct): boolean {
    if (model.dateString) {
      const d = moment(model.dateString, 'YYYY-MM-DD', true);
      return d.isValid();
    }
    return false;
  }

  updateDate(newValue: string): void {
    this.setModel({ ...this.model, dateString: newValue });
    this.propagateModelChange();
  }

  updateTime(newValue: NgbTimeStruct): void {
    this.setModel({ ...this.model, ...newValue });
    this.propagateModelChange();
  }

  formatDate(date: Date): string {
    if (!date) {
      return null;
    }
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    return `${this.pad(year)}-${this.pad(month)}-${this.pad(day)}`;
  }

  dateToNgbTimeStruct(date: Date): NgbTimeStruct {
    if (!date) {
      return null;
    }
    const hour = date.getHours();
    const minute = date.getMinutes();
    const second = date.getSeconds();
    return { hour, minute, second };
  }

  private setModel(newValue: DateTimeStruct): void {
    this.model = newValue;
    this.dateString = this.model.dateString;
    this.time = {
      hour: this.model.hour,
      minute: this.model.minute,
      second: this.model.second,
    };
  }

  private pad(i: number): string {
    return i < 10 ? `0${i}` : `${i}`;
  }

  private fromJSDate(jsDate: Date): DateTimeStruct {
    let dateAndTime: DateTimeStruct = { dateString: null, hour: 0, minute: 0, second: 0 };
    if (jsDate) {
      const timeStruct = this.dateToNgbTimeStruct(jsDate);
      const dateString = this.formatDate(jsDate);
      dateAndTime = { ...dateAndTime, ...timeStruct, dateString };
    }
    return dateAndTime;
  }

  private toJSDate(model: DateTimeStruct): Date {
    // use moment 'strict' mode so it fails for any format other than 'YYYY-MM-DD' when parsing the date string
    const d = moment(model.dateString, 'YYYY-MM-DD', true);
    if (d.isValid()) {
      d.hour(model.hour);
      d.minute(model.minute);
      d.second(model.second);
      return d.toDate();
    }
    return null;
  }

  private propagateModelChange(touched = true): void {
    if (touched) {
      this.onTouched();
    }

    // emit change events
    if (this.isValid(this.model)) {
      const jsDate = this.toJSDate(this.model);
      this.valueChange.emit(jsDate);
      this.onChange(jsDate);
    } else {
      this.valueChange.emit(null);
      this.onChange(null);
    }
  }
}
