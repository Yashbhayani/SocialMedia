import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LikeServicesService {

  constructor(private http: HttpClient) {}

  LikePost(POSTID: number, token: any) {
    return this.http.patch<any>(
      'https://localhost:44329/AddAndDeleteLike',
      POSTID,
      this.headtok(token)
    );
  }

  LikeList(POSTID: number, token: any) {
    return this.http.patch<any>(
      'https://localhost:44329/LikeList',
      POSTID,
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
