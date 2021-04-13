import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';

//Services are mainly used for Http Requests
//@Injectable decorator -
//This service can be injected into other services or components
//Angular services are singletons - data stored in the service won't get destroyed until the application is closed
@Injectable({
  providedIn: 'root'
})
  //Used to make requests to our API
export class AccountService {
  //If we want to set the property use '='
  //If we want to make it the type of something use ':'
  baseUrl = 'https://localhost:5001/api/';

  //Creating an observable to store the user in
  /*ReplaySubject - Is kind of like a buffer object. It's going to store the values in and
   anytime a subscriber subscribes to this observable it's going to emmit the last value inside it
   how ever many values we want it to emmit.
   User - type
   (1) - How many previous values we want it to store*/
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  //Inject HttpClient into the account service
  constructor(private http: HttpClient) { }

  /*This login method will receive credentials from the login form
   in the navbar*/
  /*Anything inside the pipe is a RxJS (Reative Extensions for JavaScript) operator.
   Meaning you can change the original observable in some manner*/

  /*When we logging in since we're subscribing in our nav component
   this function is going to run and populate our user in the localstorage*/
  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      /*map() uses the observable, processes it, return the processed observable
       back to the pipe().
       User - from user interface*/
      map((response: User) => {
        //first step is get the user from the response
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  //Register Method
  register(model: any) {
    //use pipe cos when we use register we consider them logged in to our system
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }
  //Helper method
  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
