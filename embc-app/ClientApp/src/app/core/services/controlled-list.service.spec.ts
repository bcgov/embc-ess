import { TestBed } from '@angular/core/testing';

import { ControlledListService } from './controlled-list.service';

describe('ControlledListService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ControlledListService = TestBed.get(ControlledListService);
    expect(service).toBeTruthy();
  });
});
