/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { CookieService } from './cookie.service';

describe('Service: Cookie', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CookieService]
    });
  });

  it('should ...', inject([CookieService], (service: CookieService) => {
    expect(service).toBeTruthy();
  }));
});
