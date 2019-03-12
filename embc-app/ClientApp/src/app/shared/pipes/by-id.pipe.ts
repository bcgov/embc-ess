import { Pipe, PipeTransform } from '@angular/core';

/**
 * Type definition for an object with props; e.g.
 * categories: {
 *   '32o8wafe': {id: '32o8wafe', name: 'abs',  exercises: []},
 *   'oaiwefjo': {id: 'oaiwefjo', name: 'arms', exercises: []},
 *   '3oij2e3c': {id: '3oij2e3c', name: 'legs', exercises: []},
 * }
 */
interface HashTable {
  [key: string]: any;
}

@Pipe({
  name: 'byId'
})
export class ByIdPipe implements PipeTransform {
  /**
   * Finds an object by its Id within a HashTable (normalized state).
   *
   * @param dataStore The normalized state object to query.
   * @param key The value of the primary key we are searching for.
   */
  transform(dataStore: HashTable, key: string): any {
    if (!key) {
      return null;
    }
    return dataStore[key];
  }
}
