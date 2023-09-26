import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthServicesService {
  constructor(private http: HttpClient) {}
  SignUp(Data: any) {
    return this.http.post<any>('https://localhost:44329/Signup', Data);
  }

  Login(Data: any) {
    return this.http.post<any>('https://localhost:44329/Signin', Data);
  }
}
