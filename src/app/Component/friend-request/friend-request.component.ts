import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FriendServicesService } from 'src/app/MainServices/friend-services.service';
import { LoderService } from 'src/app/Services/loder.service';

@Component({
  selector: 'app-friend-request',
  templateUrl: './friend-request.component.html',
  styleUrls: ['./friend-request.component.css']
})
export class FriendRequestComponent {
  FriendRequestList: any;
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
      this.follow.GetReciveFriendList(localStorage.getItem('token')).subscribe({
        next: (res) => {
          this.FriendRequestList = res.data;
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

  Accept(id:number){
    this.loadingService.setLoading(true);
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      this.follow
        .AcceptRequest(id, localStorage.getItem('token'))
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

  Remove(id:number){
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
