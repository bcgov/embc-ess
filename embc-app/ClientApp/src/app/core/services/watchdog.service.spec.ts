import { TestBed } from '@angular/core/testing';

import { WatchdogService } from './watchdog.service';

describe('WatchdogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WatchdogService = TestBed.get(WatchdogService);
    expect(service).toBeTruthy();
  });
});
