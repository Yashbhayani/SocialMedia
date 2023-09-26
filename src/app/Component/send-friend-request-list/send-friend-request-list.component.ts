import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FriendServicesService } from 'src/app/MainServices/friend-services.service';
import { LoderService } from 'src/app/Services/loder.service';

@Component({
  selector: 'app-send-friend-request-list',
  templateUrl: './send-friend-request-list.component.html',
  styleUrls: ['./send-friend-request-list.component.css']
})
export class SendFriendRequestListComponent {
  SendRequestFriendList: any;
  constructor(
    private router: Router,
    private loadingService: LoderService,
    private follow: FriendServicesService
  ) {}

  ngOnInit(): void {
    this.GetList();
  }

  GetList() {
    this.loadingService.setLoading(true);
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      this.follow.GetSendFriendList(localStorage.getItem('token')).subscribe({
        next: (res) => {
          this.SendRequestFriendList = res.data;
          this.loadingService.setLoading(false);
        },
        error: (er) => {
          this.loadingService.setLoading(false);
        },
      });
    } else {
      this.router.navigate(['/login']);
    }
  }

  Cancle(id: any) {
    this.loadingService.setLoading(true);
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      this.follow
        .CancleFriendRequest(id, localStorage.getItem('token'))
        .subscribe({
          next: (res) => {
            this.GetList();
            this.loadingService.setLoading(false);
          },
          error: (er) => {
            this.loadingService.setLoading(false);
          },
        });
    } else {
      this.router.navigate(['/login']);
    }
  }
}
