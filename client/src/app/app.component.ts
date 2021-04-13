import { HttpClient } from '@angular/common/http';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
  //Fetch the data and display
  //implements OnInit - to use the life cycle event after the constructor
export class AppComponent implements OnInit{
  title = 'client';
  //TypeScript gives us type safety unless we use the keyword 'any'
  users: any

  //Angular Dependency Injection. Http is naturally asynchronous
  //Inside the constructor is where we decalre what we want to inject
  //Life cycle after the constructor is called initialization
  constructor(private http: HttpClient, private accountService: AccountService) { }


  ngOnInit() {
    this.getUsers();
    this.setCurrentUser();
  }

  //Persist our login
  //Take a look at the browser to see if we have a key
  //Parse cos we stringyfied the user in account.service.ts
  setCurrentUser() {
    //Getting user object from localstorage and setting that in our accountservice
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }

  //Response we get back from our API
  getUsers()
  {
    this.http.get('https://localhost:5001/api/users').subscribe(response => {
      this.users = response;
    }, error => {
      console.log(error);
    })
  }
}
