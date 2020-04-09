import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { SkilledWrokersService } from '../services/skilled-wrokers/skilled-wrokers.service';
import { UserProfileInfo } from '../models/UserProfileInfo';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})

export class ProfileComponent implements OnInit {
  token = '';
  users: string;
  userId = '';
  userProfile: UserProfileInfo;
  userExists = false;

  constructor(public auth: AuthService, public skilledWorkersService: SkilledWrokersService) {
  }

  public GetToken()
  {
      this.auth.getUser$().subscribe(
        (data) => {
          console.log(JSON.stringify(data));
          this.token = data;
          this.userId = data['sub'].replace('auth0|', '');
        });
  }

  ngOnInit() {
    console.log('hello from here');
    this.auth.getUser$().subscribe(
      (data) => {
        console.log(JSON.stringify(data));
        this.token = data;
        this.userId = data['sub'].replace('auth0|', '');

        this.skilledWorkersService.getUsers$().subscribe(
          (skilledWorkerRespone) => {
              if (skilledWorkerRespone !== null) {
                this.userProfile = skilledWorkerRespone;
                this.userExists = true;
              }
          });
      });
  }

}
