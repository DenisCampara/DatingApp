import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private alertfy: AlertifyService){}

  canActivate(): boolean  {
    if (this.authService.logedIn()) {
      return true;
    }
    this.alertfy.error('You shall not pass!!! :D');
    this.router.navigate(['/home']);
    return false;
  }
}
