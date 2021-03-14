import { HttpClient } from '@angular/common/http';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'client';
  //TypeScript gives us type safety unless we use the keyword 'any'
  users: any

  //Angular Dependency Injection. Http is naturally asynchronous
  //Life cycle after the constructor is called initialization
  constructor(private http: HttpClient) { }


  ngOnInit() {
    this.getUsers();
  }

  getUsers()
  {
    this.http.get('https://localhost:5001/api/users').subscribe(response => {
      this.users = response;
    }, error => {
      console.log(error);
    })
  }
}
