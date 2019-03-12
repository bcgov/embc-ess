import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'pluck'
})
export class PluckPipe implements PipeTransform {
  /**
   * Maps each source value (an object) to its specified nested property.
   *
   * @param source The source object.
   * @param prop The nested property to pluck from the source object.
   */
  transform(source: { [key: string]: any; }, prop: string = 'name'): any {
    return source && source.hasOwnProperty(prop) ? source[prop] : null;
  }
}
