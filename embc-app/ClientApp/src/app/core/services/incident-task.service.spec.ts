import { TestBed } from '@angular/core/testing';

import { IncidentTaskService } from './incident-task.service';

describe('IncidentTaskService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: IncidentTaskService = TestBed.get(IncidentTaskService);
    expect(service).toBeTruthy();
  });
});
