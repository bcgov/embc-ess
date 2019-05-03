import { TestBed } from '@angular/core/testing';

import { ReferralService } from './referral.service';

describe('ReferralService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReferralService = TestBed.get(ReferralService);
    expect(service).toBeTruthy();
  });
});
