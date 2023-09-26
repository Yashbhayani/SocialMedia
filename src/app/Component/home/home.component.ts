import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommentServicesService } from 'src/app/MainServices/comment-services.service';
import { LikeServicesService } from 'src/app/MainServices/like-services.service';
import { PostServicesService } from 'src/app/MainServices/post-services.service';
import { LoderService } from 'src/app/Services/loder.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  Postdata: any;
  Commentdata: any;
  NewPostdata: any;

  CommentForm!: FormGroup;
  EDITPOSTForm!: FormGroup;

  imageArray: any;
  image: any;
  imagedata_1: any = './assets/image_placeholder.png';
  AlertVal: any;
  Color: any;
  Message: any;
  LikeList: any;
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private loadingService: LoderService,
    private post: PostServicesService,
    private like: LikeServicesService,
    private Comment: CommentServicesService
  ) {}

  ngOnInit(): void {
    this.EditPostForm();
    this.GetPost();
  }

  GetPost() {
    this.loadingService.setLoading(true);
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      this.post.GetPost(localStorage.getItem('token')).subscribe({
        next: (res) => {
          this.Postdata = res.data;
          this.loadingService.setLoading(false);
        },
        error: (er) => {
          this.loadingService.setLoading(false);
        },
      });
    } else {
      this.router.navigate(['/login']);
      this.loadingService.setLoading(false);
    }
  }

  ToggleLike(id: any) {
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      this.like.LikePost(id, localStorage.getItem('token')).subscribe({
        next: (res) => {
          this.GetPost();
        },
        error: (er) => {
          console.log(er);
        },
      });
    } else {
      this.router.navigate(['/login']);
    }
  }

  ToggleComment(id: any) {
    const EditModel = document.getElementById('editModal');
    if (EditModel != null) {
      this.CommnetForm();
      EditModel.style.display = 'block';
      this.loadingService.setLoading(true);
      let TOKEN = localStorage.getItem('token');
      if (TOKEN !== null && TOKEN !== undefined) {
        this.CommentForm.controls['PostId'].setValue(id);
        this.GetCommentdata(id);
      }
    } else {
      this.GetPost();
    }
  }

  Closemodel() {
    const EditModel = document.getElementById('editModal');
    if (EditModel != null) {
      EditModel.style.display = 'none';
      this.GetPost();
    }
  }

  NewToggleLike(id: any) {
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      this.like.LikePost(id, localStorage.getItem('token')).subscribe({
        next: (res) => {
          if (res.success) {
            this.GetCommentdata(id);
          }
        },
        error: (er) => {
          console.log(er);
        },
      });
    } else {
      this.router.navigate(['/login']);
    }
  }

  GetCommentdata(id: number) {
    this.CommnetForm();
    this.Comment.GetComment(id, localStorage.getItem('token')).subscribe({
      next: (res) => {
        this.Commentdata = res.data;
        this.NewPostdata = res.postdata;
        this.CommentForm.controls['PostId'].setValue(id);
        this.loadingService.setLoading(false);
      },
      error: (er) => {
        console.log(er);
        this.loadingService.setLoading(false);
      },
    });
  }

  CommnetForm() {
    this.CommentForm = this.formBuilder.group({
      PostId: ['', Validators.required],
      Commnets: ['', Validators.required],
    });
  }

  AddCommnet() {
    this.loadingService.setLoading(true);
    let id = this.CommentForm.value.PostId;
    let data = {
      postId: this.CommentForm.value.PostId,
      comment_Content: this.CommentForm.value.Commnets,
    };
    this.Comment.AddComment(data, localStorage.getItem('token')).subscribe({
      next: (res) => {
        this.GetCommentdata(id);
        this.CommentForm.reset();
        this.loadingService.setLoading(false);
      },
      error: (er) => {
        console.log(er);
        this.loadingService.setLoading(false);
      },
    });
  }

  DeleteComment(id: number, PostId: number) {
    this.loadingService.setLoading(true);
    this.Comment.DeleteComment(id, localStorage.getItem('token')).subscribe({
      next: (res) => {
        this.GetCommentdata(PostId);
        this.loadingService.setLoading(false);
      },
      error: (er) => {
        console.log(er);
        this.loadingService.setLoading(false);
      },
    });
  }

  EditPostForm() {
    this.EDITPOSTForm = this.formBuilder.group({
      id: [0, Validators.required],
      Description: ['', Validators.required],
      Image: ['', Validators.required],
    });
  }

  EditPost(id: any, image: any, content: any) {
    const EditPostModel = document.getElementById('editPostModal');
    if (EditPostModel != null) {
      EditPostModel.style.display = 'block';
      this.loadingService.setLoading(true);
      let TOKEN = localStorage.getItem('token');
      if (TOKEN !== null && TOKEN !== undefined) {
        this.loadingService.setLoading(false);
        this.imagedata_1 = `https://localhost:44329/resources/${image}`;
        this.EDITPOSTForm.controls['Description'].setValue(content);
        this.EDITPOSTForm.controls['id'].setValue(id);
      }
    } else {
      this.GetPost();
    }
  }

  onFileSelected(event: any) {
    this.imageArray = event.target.files[0];
    this.image = URL.createObjectURL(event.target.files[0]);
  }

  EDITADDPost() {
    if (this.imageArray) {
      if (
        localStorage.getItem('token') != null &&
        localStorage.getItem('token') != undefined
      ) {
        this.loadingService.setLoading(true);
        let gh: any = [];
        const formData: any = new FormData();
        formData.append('id', this.EDITPOSTForm.value.id);
        formData.append('image', this.imageArray);
        formData.append('content', this.EDITPOSTForm.value.Description);
        this.post
          .ImageChangeData(formData, localStorage.getItem('token'))
          .subscribe({
            next: (res) => {
              this.GetPost();
              this.AlertVal = false;
              this.Color = 'success';
              this.Message = res.message;
              this.EDITPOSTForm.reset();
              this.image = undefined;
              this.loadingService.setLoading(false);
              setTimeout(() => {
                this.AlertVal = true;
                this.Color = '';
                this.Message = '';
              }, 3500);
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
    } else {
      if (
        localStorage.getItem('token') != null &&
        localStorage.getItem('token') != undefined
      ) {
        this.loadingService.setLoading(true);
        const updatePostClass = {
          id: this.EDITPOSTForm.value.id,
          content: this.EDITPOSTForm.value.Description,
        };
        this.post
          .NoImageChangeData(updatePostClass, localStorage.getItem('token'))
          .subscribe({
            next: (res) => {
              this.GetPost();
              this.AlertVal = false;
              this.Color = 'success';
              this.Message = res.message;
              this.EDITPOSTForm.reset();
              this.image = undefined;
              this.loadingService.setLoading(false);
              setTimeout(() => {
                this.AlertVal = true;
                this.Color = '';
                this.Message = '';
              }, 3500);
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
    this.CloseEditmodel();
  }

  CloseEditmodel() {
    const EditPOstModel = document.getElementById('editPostModal');
    if (EditPOstModel != null) {
      EditPOstModel.style.display = 'none';
      this.GetPost();
    }
  }

  DeletePost(id: number) {
    this.loadingService.setLoading(true);
    let TOKEN = localStorage.getItem('token');
    if (TOKEN !== null && TOKEN !== undefined) {
      this.post.DeletePostData(id, localStorage.getItem('token')).subscribe({
        next: (res) => {
          this.GetPost();
          this.loadingService.setLoading(false);
        },
        error: (er) => {
          console.log(er);
          this.loadingService.setLoading(false);
        },
      });
    } else {
      this.router.navigate(['/login']);
      this.loadingService.setLoading(false);
    }
  }
  PostLikeUserList(id: number) {
    const likeModel = document.getElementById('likeModel');
    if (likeModel != null) {
      likeModel.style.display = 'block';
      this.loadingService.setLoading(true);
      let TOKEN = localStorage.getItem('token');
      if (TOKEN !== null && TOKEN !== undefined) {
        this.like.LikeList(id, localStorage.getItem('token')).subscribe({
          next: (res) => {
            if (res.success) {
              this.LikeList = res.data;
              this.loadingService.setLoading(false);
            }
          },
          error: (er) => {
            console.log(er);
          },
        });
      } else {
        this.router.navigate(['/login']);
      }
    } else {
      this.GetPost();
    }

  }


  CloseLikemodel() {
    const likeModel = document.getElementById('likeModel');
    if (likeModel != null) {
      likeModel.style.display = 'none';
      this.GetPost();
    }
  }
}
