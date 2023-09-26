import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PostServicesService } from 'src/app/MainServices/post-services.service';
import { LoderService } from 'src/app/Services/loder.service';

@Component({
  selector: 'app-addpost',
  templateUrl: './addpost.component.html',
  styleUrls: ['./addpost.component.css'],
})
export class AddpostComponent {
  PostForm!: FormGroup;
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
    public post: PostServicesService
  ) {}

  ngOnInit(): void {
    if (
      localStorage.getItem('token') != null &&
      localStorage.getItem('token') != undefined
    ) {
      this.PostForm = this.formBuilder.group({
        Description: ['', Validators.required],
        Image: ['', Validators.required],
      });
    } else {
      this.router.navigate(['/signup']);
    }
  }

  onFileSelected(event: any) {
    this.imageArray = event.target.files[0];
    this.image = URL.createObjectURL(event.target.files[0]);
  }

  AddPost() {
    if (
      localStorage.getItem('token') != null &&
      localStorage.getItem('token') != undefined
    ) {
      this.loadingService.setLoading(true);
      let gh: any = [];
      const formData: any = new FormData();
      formData.append('image', this.imageArray);
      formData.append('content', this.PostForm.value.Description);
      this.post.AddPostData(formData, localStorage.getItem('token')).subscribe({
        next: (res) => {
          if (res.success) {
            this.AlertVal = false;
            this.Color = 'success';
            this.Message = res.message;
            this.PostForm.reset();
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
          console.log(er);
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
}
