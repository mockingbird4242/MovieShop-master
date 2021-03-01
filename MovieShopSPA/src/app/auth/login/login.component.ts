import { Component, OnInit } from '@angular/core';
import { Login } from 'src/app/shared/models/login';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  userLogin: Login = {
    email: '', password: ''
  }
  constructor() { }
  ngOnInit(): void {
    console.log('Inside ng Oninit UN/PW');
    console.log(this.userLogin.email);
    console.log(this.userLogin.password);
  }
  login() {
    console.log('Inside On Click Oninit UN/PW');
    console.log(this.userLogin.email);
    console.log(this.userLogin.password);
    console.log('form is submitted');
  }
}