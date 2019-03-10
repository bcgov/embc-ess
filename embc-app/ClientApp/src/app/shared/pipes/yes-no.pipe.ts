import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'yesNo'
})
export class YesNoPipe implements PipeTransform {
  transform(value: boolean, args?: any): string {
    return value === true ? 'Yes' : 'No';
  }
}
