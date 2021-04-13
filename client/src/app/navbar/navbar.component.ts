import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  //Class property to store what the user enters
  model: any = {}
  /*loggedIn: boolean;*/
  //Don't need this as the AccountService already got it as a property
  //currentUser$ : Observable<User>;

  //Injecting account.service.ts
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    //get current user from account service
    //this.getCurrentUser();
    //this.currentUser$ = this.accountService.currentUser$;
  }

  login() {
    //log the values in the model proprty
    //console.log(this.model);

    //Using accountservice to login the user
    //Have to subscribe because observables are lazy and that's what the login method is returning
    //The response is going to be the UserDto with the username and JWT token when loggin in
    this.accountService.login(this.model).subscribe(response => {
      console.log(response);
      //console.log(this.loggedIn);
      //this.loggedIn = true;
    }, error => {
      console.log(error)
    });
    
  }

  logout() {
    this.accountService.logout();
    //this.loggedIn = false;
  }

  //Reminder - this currentUser$ isn't an HTTP Request which can cause memory leaks
  //DON'T NEED THIS METHOD AS I'M GETTING THE USER FROM ACCOUNT SERVICE
  //getCurrentUser() {
  //  //Take a look at our current user observable
  //  //subscribing to the obserable from app.component.ts
  //  this.accountService.currentUser$.subscribe(user => {
  //    //!! is a boolean
  //    this.loggedIn = !!user;
  //  }, error => {
  //    console.log(error);
  //  })
  //}

}
