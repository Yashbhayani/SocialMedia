import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class MasterServiceService {
  constructor(private router: Router) {}

  UserFunction() {
    if (
      localStorage.getItem('token') !== null ||
      localStorage.getItem('token') !== undefined
    ) {
      return true;
    } else {
      return false;
    }
  }
}
