import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PostServicesService {

  constructor(private http: HttpClient) {}

  GetPost(token: any) {
    return this.http.get<any>(
      'https://localhost:44329/GetPostData',
      this.headtok(token)
    );
  }

  AddPostData(addPostClass: any, token: any) {
    return this.http.post<any>(
      'https://localhost:44329/AddPostData',
      addPostClass,
      this.headtook(token)
    );
  }

  ImageChangeData(updatePostClass: any, token: any) {
    return this.http.put<any>(
      'https://localhost:44329/ImageChangeData',
      updatePostClass,
      this.headtook(token)
    );
  }

  NoImageChangeData(updatePostClass: any, token: any) {
    return this.http.put<any>(
      'https://localhost:44329/NoImageChangeData',
      updatePostClass,
      this.headtok(token)
    );
  }

  DeletePostData(id: any, token: any) {
    return this.http.delete<any>(
      `https://localhost:44329/DeletePostData?id=${id}`,
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

  headtook(token: any) {
    let api_key = token;
    const headerDict = {
      token: api_key,
    };

    const requestOptionss = {
      headers: new HttpHeaders(headerDict),
    };

    return requestOptionss;
  }
}
