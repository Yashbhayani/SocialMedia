import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FriendServicesService } from 'src/app/MainServices/friend-services.service';
import { LoderService } from 'src/app/Services/loder.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
})
export class SearchComponent {
  SearchhForm!: FormGroup;
  SearchList: any;
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private loadingService: LoderService,
    private follow: FriendServicesService
  ) {}

  ngOnInit(): void {
    this.SearchForm();
  }

  SearchForm() {
    this.SearchhForm = this.formBuilder.group({
      Search: ['', Validators.required],
    });
  }

  SearchData() {
    this.loadingService.setLoading(true);
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      var uname = {
        uname: this.SearchhForm.value.Search,
      };
      this.follow.SearchList(uname, localStorage.getItem('token')).subscribe({
        next: (res) => {
          this.SearchList = res.data;
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
        .RemoveFriend(id, localStorage.getItem('token'))
        .subscribe({
          next: (res) => {
            this.SearchData();
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
            this.SearchData();
          },
          error: (er) => {
            this.loadingService.setLoading(false);
          },
        });
    } else {
      this.router.navigate(['/login']);
    }
  }

  Cancle(id:number){
    this.loadingService.setLoading(true);
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      this.follow
        .CancleFriendRequest(id, localStorage.getItem('token'))
        .subscribe({
          next: (res) => {
            this.SearchData();
          },
          error: (er) => {
            this.loadingService.setLoading(false);
          },
        });
    } else {
      this.router.navigate(['/login']);
    }
  }

  CancleFriendRequest(id: any) {
    this.loadingService.setLoading(true);
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      this.follow
        .CancleFriendRequest(id, localStorage.getItem('token'))
        .subscribe({
          next: (res) => {
            this.SearchData();
          },
          error: (er) => {
            this.loadingService.setLoading(false);
          },
        });
    } else {
      this.router.navigate(['/login']);
    }
  }


  SendFriendRequest(id: any) {
    this.loadingService.setLoading(true);
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      this.follow
        .SendandcancleFriendRequest(id, localStorage.getItem('token'))
        .subscribe({
          next: (res) => {
            this.SearchData();
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
