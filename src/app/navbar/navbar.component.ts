import { Component } from '@angular/core';
import { LoderService } from '../Services/loder.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent {
  login_Active: boolean = false;
  login_Not_Active: boolean = true;
  Email: any;

  constructor(
    private loadingService: LoderService,
    private route: Router,
    public AR: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.events.subscribe((val: any) => {
      this.checkfiunction();
    });
    this.checkfiunction();
  }

  checkfiunction() {
    if (
      localStorage.getItem('token') != null &&
      localStorage.getItem('token') != undefined
    ) {
      this.login_Active = true;
      this.login_Not_Active = false;
      this.Email = localStorage.getItem('email');
    } else {
      this.login_Not_Active = true;
      this.login_Active = false;
    }
  }

  Logout() {
    localStorage.clear();
    this.route.navigate(['/login']);
  }
}
