import { Component, OnInit} from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { EventemiterService } from '../_services/eventemiter.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  photoUrl: string;
  username: string;
  chatStatus = false;
  constructor(public authService: AuthService, private alertify: AlertifyService, public router: Router,
              private eventEmitterService: EventemiterService) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in successfuly');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  logedIn() {
    return this.authService.logedIn();
  }

  logOut() {
    localStorage.removeItem('token');
    localStorage.removeItem('userPhoto');
    localStorage.removeItem('userDetails');
    this.authService.decodedToken = null;
    this.authService.currentUserPhoto = null;
    this.alertify.message('Logged out');
    this.router.navigate(['/home']);
  }

  loadUsers() {
    this.eventEmitterService.onSearchButtonClick(this.username);
  }
}
