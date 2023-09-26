import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthServicesService } from 'src/app/MainServices/auth-services.service';
import { LoderService } from 'src/app/Services/loder.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  LoginForm!: FormGroup;
  hide: boolean = true;
  Color: string = '';
  AlertVal: boolean = true;
  Message: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private loadingService: LoderService,
    public signup: AuthServicesService
  ) {}

  ngOnInit(): void {
    if (
      localStorage.getItem('token') == null ||
      localStorage.getItem('token') == undefined
    ) {
      this.LoginForm = this.formBuilder.group({
        Email: ['', Validators.required],
        Password: ['', Validators.required],
      });
    } else {
      this.router.navigate(['/']);
    }
  }

  Shoandhide() {
    if (this.hide) {
      this.hide = false;
    } else {
      this.hide = true;
    }
  }

  Signin() {
    let Data = {
      emialId: this.LoginForm.value.Email,
      password: this.LoginForm.value.Password,
    };

    this.loadingService.setLoading(true);
    this.signup.Login(Data).subscribe({
      next: (res) => {
        if (res.success) {
          localStorage.setItem('token', res.token);
          localStorage.setItem('email', res.email);
          this.AlertVal = false;
          this.Color = 'success';
          this.Message = res.message;
          this.loadingService.setLoading(false);
          setTimeout(() => {
            this.AlertVal = true;
            this.Color = '';
            this.Message = '';
          }, 3500);
          this.LoginForm.reset();
          this.router.navigate(['']);
        } else {
          this.AlertVal = false;
          this.Color = 'warning';
          this.Message = res.message;
          this.loadingService.setLoading(false);
          setTimeout(() => {
            this.AlertVal = true;
            this.Color = '';
            this.Message = '';
          }, 3500);
        }
      },
      error: (er) => {
        this.AlertVal = false;
        this.Color = 'warning';
        this.Message = er.message;
        this.loadingService.setLoading(false);
        setTimeout(() => {
          this.AlertVal = true;
          this.Color = '';
          this.Message = '';
        }, 3500);
      },
    });
  }
}
