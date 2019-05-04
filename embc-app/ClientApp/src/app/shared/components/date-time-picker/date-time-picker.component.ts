import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';

// tslint:disable: no-use-before-declare
export const DATETIMEPICKER_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DateTimePickerComponent),
  multi: true
};

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

  localDate: string = null;
  localTime: NgbTimeStruct = { hour: 0, minute: 0, second: 0 };

  constructor() { }

  ngOnInit() {
  }

  // ControlValueAccessor methods follow...

  // This is a placeholder method that we use to emit changes back to the form
  onChange = (_: any) => { };
  onTouched = () => { };

  // This is a basic setter that the Angular forms API is going to use
  writeValue(value: any): void {
    const model = this.fromDateObject(value);
    this.localDate = model.localDate;
    this.localTime = model.localTime;
  }

  // Allows Angular to register a function to call when the model changes.
  // This is how we emit the changes back to the form
  registerOnChange(fn: any): void { this.onChange = fn; }

  registerOnTouched(fn: any): void { this.onTouched = fn; }

  // Allows Angular to disable this component.
  setDisabledState(isDisabled: boolean): void { this.disabled = isDisabled; }

  // end of ControlValueAccessor

  updateDate(newValue: string): void {
    this.localDate = newValue;
    this.propagateModelChange();
  }

  updateTime(newValue: NgbTimeStruct): void {
    this.localTime = newValue;
    this.propagateModelChange();
  }

  private propagateModelChange(): void {
    // TODO: combine date, time components
    // mocked for now
    const datetime = this.toDateObject({
      localDate: this.localDate,
      localTime: this.localTime
    });

    // emit change events
    this.valueChange.emit(datetime);
    this.onChange(datetime);
  }

  private formatDate(date: Date): string {
    if (!date) {
      return null;
    }
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    // return moment(date).format('YYYY-MM-DD');
    return `${this.pad(year)}-${this.pad(month)}-${this.pad(day)}`;
  }

  private formatTime(date: Date): NgbTimeStruct {
    if (!date) {
      return null;
    }
    const hour = date.getHours();
    const minute = date.getMinutes();
    const second = date.getSeconds();
    return { hour, minute, second };
  }

  private pad(i: number): string {
    return i < 10 ? `0${i}` : `${i}`;
  }

  private fromDateObject(date: Date): { localDate: string, localTime: NgbTimeStruct } {
    let localDate = null;
    let localTime = { hour: 0, minute: 0, second: 0 };
    if (date !== null) {
      localDate = this.formatDate(date);
      localTime = this.formatTime(date);
    }
    return { localDate, localTime };
  }

  private toDateObject(model: { localDate: string, localTime: NgbTimeStruct }): Date {
    const d = moment(model.localDate, 'YYYY-MM-DD', true);
    d.hour(model.localTime.hour);
    d.minute(model.localTime.minute);
    d.second(model.localTime.second);
    return d.toDate();
  }
}
