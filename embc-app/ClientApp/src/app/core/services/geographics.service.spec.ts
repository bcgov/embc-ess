import { TestBed } from '@angular/core/testing';

import { GeographicsService } from './geographics.service';

describe('GeographicsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GeographicsService = TestBed.get(GeographicsService);
    expect(service).toBeTruthy();
  });
});
