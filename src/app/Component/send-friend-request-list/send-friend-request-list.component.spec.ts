import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendFriendRequestListComponent } from './send-friend-request-list.component';

describe('SendFriendRequestListComponent', () => {
  let component: SendFriendRequestListComponent;
  let fixture: ComponentFixture<SendFriendRequestListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SendFriendRequestListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SendFriendRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
