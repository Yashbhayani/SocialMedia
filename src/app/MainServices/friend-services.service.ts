import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FriendServicesService {

  constructor(private http: HttpClient) {}

  SendandcancleFriendRequest(reciverUserId: number, token: any) {
    return this.http.patch<any>(
      'https://localhost:44329/SendandcancleFriendRequest',
      reciverUserId,
      this.headtok(token)
    );
  }

  CancleFriendRequest(reciverUserId: number, token: any) {
    return this.http.patch<any>(
      'https://localhost:44329/CancleFriendRequest',
      reciverUserId,
      this.headtok(token)
    );
  }

  AcceptRequest(senerUserId: number, token: any) {
    return this.http.patch<any>(
      'https://localhost:44329/AcceptRequest',
      senerUserId,
      this.headtok(token)
    );
  }

  RemoveFriend(userID: number, token: any) {
    return this.http.delete<any>(
      `https://localhost:44329/RemoveFriend?UserID=${userID}`,
      this.headtok(token)
    );
  }

  GetSendFriendList(token: any) {
    return this.http.get<any>(
      'https://localhost:44329/GetSendFriendList',
      this.headtok(token)
    );
  }

  GetReciveFriendList(token: any) {
    return this.http.get<any>(
      'https://localhost:44329/GetReciveFriendList',
      this.headtok(token)
    );
  }

  GetFriendList(token: any) {
    return this.http.get<any>(
      'https://localhost:44329/GetFriendList',
      this.headtok(token)
    );
  }


  SearchList(uname: any, token: any) {
    return this.http.patch<any>(
      'https://localhost:44329/SearchList',
      uname,
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
