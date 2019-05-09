import { TestBed } from '@angular/core/testing';

import { UniqueKeyService } from './unique-key.service';

describe('UniqueKeyService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UniqueKeyService = TestBed.get(UniqueKeyService);
    expect(service).toBeTruthy();
  });
});
