import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './AuthComponent/login/login.component';
import { SignUpComponent } from './AuthComponent/sign-up/sign-up.component';
import { FriendListComponent } from './Component/friend-list/friend-list.component';
import { FriendRequestComponent } from './Component/friend-request/friend-request.component';
import { HomeComponent } from './Component/home/home.component';
import { SearchComponent } from './Component/search/search.component';
import { SendFriendRequestListComponent } from './Component/send-friend-request-list/send-friend-request-list.component';
import { AuthguardGuard } from './guard/authguard.guard';
import { AddpostComponent } from './PostComponent/addpost/addpost.component';

const routes: Routes = [
  { path: 'signup', component: SignUpComponent, canActivate: [AuthguardGuard] },
  { path: 'login', component: LoginComponent, canActivate: [AuthguardGuard] },
  { path: '', component: HomeComponent },
  { path: 'addpost', component: AddpostComponent },
  { path: 'friendRequestList', component: FriendRequestComponent },
  { path: 'sendFriendRequestList', component: SendFriendRequestListComponent },
  { path: 'friendList', component: FriendListComponent },
  { path: 'search', component: SearchComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
