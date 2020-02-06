/* tslint:disable:no-unused-variable */

import { TestBed, async } from '@angular/core/testing';
import { DateTimeFormatPipe } from './date-time-format.pipe';

describe('Pipe: DateTimeFormatPipe', () => {
  it('create an instance', () => {
    let pipe = new DateTimeFormatPipe('01/01/1998');
    expect(pipe).toBeTruthy();
  });
});
