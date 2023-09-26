import { TestBed } from '@angular/core/testing';

import { LikeServicesService } from './like-services.service';

describe('LikeServicesService', () => {
  let service: LikeServicesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LikeServicesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
