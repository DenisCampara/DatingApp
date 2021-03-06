import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { UserService } from '../_services/user.service';
import { catchError } from 'rxjs/operators';
import { User } from '../_models/user';

@Injectable()

export class MemberListResolver implements Resolve<User[]> {
    
    currentPage = 1;
    pageSize = 6;
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService){}
    
    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        return this.userService.getUsers(this.currentPage, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data!!!');
                this.router.navigate(['']);
                return of(null);
            })
        );
    }
}