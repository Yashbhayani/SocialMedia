import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommentServicesService {

  constructor(private http: HttpClient) {}

  GetComment(postid: number, token: any) {
    return this.http.patch<any>(
      'https://localhost:44329/GetComment',
      postid,
      this.headtok(token)
    );
  }

  AddComment(Data: any, token: any) {
    return this.http.patch<any>(
      'https://localhost:44329/AddComment',
      Data,
      this.headtok(token)
    );
  }

  DeleteComment(id: number, token: any) {
    return this.http.delete<any>(
      `https://localhost:44329/DeleteComment?commentid=${id}`,
      this.headtok(token)
    );
  }
  headtok(token: any) {
    let api_key = token;
    const headerDict = {
      'Content-Type': 'application/json',
      token: api_key,
    };

    const requestOptionss = {
      headers: new HttpHeaders(headerDict),
    };

    return requestOptionss;
  }
}
