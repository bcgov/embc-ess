import { TestBed } from '@angular/core/testing';

import { VolunteerService } from './volunteer.service';

describe('VolunteerService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: VolunteerService = TestBed.get(VolunteerService);
    expect(service).toBeTruthy();
  });
});
