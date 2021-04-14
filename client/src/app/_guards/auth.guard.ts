import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountService: AccountService) { }
  canActivate(
    /*Route that is being activated
    //route: ActivatedRouteSnapshot,
    ////Current state of our router
    //state: RouterStateSnapshot*/): Observable<boolean>{

    //Take a look at our current user observable to check if its populated
    //No need to subscribe cos we're inside a root auth guard and it's gonna handle the subscription
    //currentUser$ returns a user
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (user) {
          return true;
        }
        else {
          console.log('You shall not pass!!');

        }
      })
    )
  }
  
}
