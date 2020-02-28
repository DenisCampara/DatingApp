import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  
  jwtHelper = new JwtHelperService();

  constructor(private authService: AuthService){}

  ngOnInit(){
    const token = localStorage.getItem('token');
    const userPhoto: User = JSON.parse(localStorage.getItem('userPhoto'));
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    if (userPhoto) {
      this.authService.currentUserPhoto = userPhoto;
      this.authService.changeMemberPhoto(userPhoto.photoUrl);
    }
  }
}
