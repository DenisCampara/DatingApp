import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { UserService } from '../_services/user.service';
import { catchError } from 'rxjs/operators';
import { Message } from '../_models/message';
import { AuthService } from '../_services/auth.service';

@Injectable()

export class MessagesResolver implements Resolve<Message[]> {
    
    currentPage = 1;
    pageSize = 6;
    messageContainer = 'Unread';
    constructor(private userService: UserService, private router: Router,
         private alertify: AlertifyService, private authService: AuthService){}
    
    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        return this.userService.getMessages(this.authService.decodedToken.nameid, this.currentPage, this.pageSize, this.messageContainer)
        .pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data!!!');
                this.router.navigate(['']);
                return of(null);
            })
        );
    }
}