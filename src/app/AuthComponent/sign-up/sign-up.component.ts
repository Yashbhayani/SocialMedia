import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthServicesService } from 'src/app/MainServices/auth-services.service';
import { LoderService } from 'src/app/Services/loder.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css'],
})
export class SignUpComponent {
  SignUpForm!: FormGroup;
  hide: boolean = true;
  Color: string = '';
  AlertVal: boolean = true;
  Message: string = '';
  imageArray: any;
  image: any;
  imagedata_1: any = './assets/image_placeholder.png';
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
      this.SignUpForm = this.formBuilder.group({
        FirstName: ['', Validators.required],
        LastName: ['', Validators.required],
        Emial: ['', Validators.required],
        Image: ['', Validators.required],
        Password: ['', Validators.required],
      });
    } else {
      this.router.navigate(['/signup']);
    }
  }

  onFileSelected(event: any) {
    this.imageArray = event.target.files[0];
    this.image = URL.createObjectURL(event.target.files[0]);
  }

  Shoandhide() {
    if (this.hide) {
      this.hide = false;
    } else {
      this.hide = true;
    }
  }

  SignUp() {
    const formData: any = new FormData();
    formData.append('image', this.imageArray);
    formData.append('firstName', this.SignUpForm.value.FirstName);
    formData.append('lastName', this.SignUpForm.value.LastName);
    formData.append('emial', this.SignUpForm.value.Emial);
    formData.append('password', this.SignUpForm.value.Password);
    this.loadingService.setLoading(true);
    this.signup.SignUp(formData).subscribe({
      next: (res) => {
        if (res.success) {
          this.AlertVal = false;
          this.Color = 'success';
          this.Message = res.message;
          this.SignUpForm.reset();
          this.image = undefined;
          this.loadingService.setLoading(false);
          setTimeout(() => {
            this.AlertVal = true;
            this.Color = '';
            this.Message = '';
          }, 3500);
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
