import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SpinnerComponent } from './spinner/spinner.component';
import { SignUpComponent } from './AuthComponent/sign-up/sign-up.component';
import { LoginComponent } from './AuthComponent/login/login.component';
import { HomeComponent } from './Component/home/home.component';
import { AddpostComponent } from './PostComponent/addpost/addpost.component';
import { FriendRequestComponent } from './Component/friend-request/friend-request.component';
import { SendFriendRequestListComponent } from './Component/send-friend-request-list/send-friend-request-list.component';
import { FriendListComponent } from './Component/friend-list/friend-list.component';
import { SearchComponent } from './Component/search/search.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SpinnerComponent,
    SignUpComponent,
    LoginComponent,
    HomeComponent,
    AddpostComponent,
    FriendRequestComponent,
    SendFriendRequestListComponent,
    FriendRequestComponent,
    FriendListComponent,
    FriendListComponent,
    SearchComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
