import { TestBed } from '@angular/core/testing';

import { EvacueeService } from './evacuee.service';

describe('EvacueeService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EvacueeService = TestBed.get(EvacueeService);
    expect(service).toBeTruthy();
  });
});
