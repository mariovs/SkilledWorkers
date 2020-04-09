import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

export interface Tile {
  color: string;
  cols: number;
  rows: number;
  text: string;
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {

  constructor(public auth: AuthService, public router: Router){

  }

  isLoggedIn() {
    if (this.auth.loggedIn) {
      return this.router.navigateByUrl('login');
    }
  }


  ngOnInit() {

  }

}
