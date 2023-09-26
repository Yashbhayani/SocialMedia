import { TestBed } from '@angular/core/testing';

import { FriendServicesService } from './friend-services.service';

describe('FriendServicesService', () => {
  let service: FriendServicesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FriendServicesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
