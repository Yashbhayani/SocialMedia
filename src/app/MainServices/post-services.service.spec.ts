import { TestBed } from '@angular/core/testing';

import { PostServicesService } from './post-services.service';

describe('PostServicesService', () => {
  let service: PostServicesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PostServicesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
