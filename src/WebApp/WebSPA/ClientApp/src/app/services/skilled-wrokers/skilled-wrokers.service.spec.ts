import { TestBed } from '@angular/core/testing';

import { SkilledWrokersService } from './skilled-wrokers.service';

describe('SkilledWrokersService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SkilledWrokersService = TestBed.get(SkilledWrokersService);
    expect(service).toBeTruthy();
  });
});
