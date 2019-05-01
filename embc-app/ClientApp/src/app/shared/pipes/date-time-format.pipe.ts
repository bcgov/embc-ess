import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

@Pipe({
  name: 'dateTimeFormatPipe'
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {
  transform(value: any, args?: any): string {
    return super.transform(value, 'yyyy/MM/dd \'at\' HH:mm:ss a');
  }
}
