import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { SkilledWrokersService } from '../services/skilled-wrokers/skilled-wrokers.service';
import { UserProfileInfo } from '../models/UserProfileInfo';
import {Appearance, Location} from '@angular-material-extensions/google-maps-autocomplete';
import {MatBottomSheet, MatBottomSheetRef} from '@angular/material/bottom-sheet';
import PlaceResult = google.maps.places.PlaceResult;

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})

export class ProfileComponent implements OnInit {
  token = '';
  users: string;
  userId = '';
  userProfile: UserProfileInfo = {} as UserProfileInfo;
  userExists = false;
  dataSaved = false;
  dataUpdated = false;
  dataError = false;


  public appearance = Appearance;
  public zoom: number;
  public latitude: number;
  public longitude: number;
  public selectedAddress: PlaceResult;

  constructor(public auth: AuthService, public skilledWorkersService: SkilledWrokersService, private _bottomSheet: MatBottomSheet) {
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

  public CreateUser()
  {
    this.userProfile.userId = this.userId;
    console.log('userCreated ', JSON.stringify(this.userProfile));
    this.skilledWorkersService.createUser$(this.userProfile).subscribe(
      (skilledWorkerRespone) => {
          if (skilledWorkerRespone !== null) {
            this.userProfile = skilledWorkerRespone;
            this.userExists = true;
            this.dataSaved = true;
            setTimeout(function() {
              console.log('hide');
              this.dataSaved = false;
            }.bind(this), 3000);
          }
      }, error => {
        console.log('here is the error' + error);
        this.userExists = false;
        this.dataError = true;
        setTimeout(function() {
          console.log('hide');
          this.dataError = false;
        }.bind(this), 5000);
        console.error(error);
    });
  }

  public UpdateUser()
  {
      console.log('user updated');
      this.dataError = true;
      setTimeout(function() {
        console.log('hide');
        this.dataError = false;
      }.bind(this), 3000);
      return;
  }

  onKey(event) { this.userProfile.lastName = event.target.value;}

  ngOnInit() {
    this.auth.getUser$().subscribe(
      (data) => {
        this.token = data;
        this.userId = data['sub'].replace('auth0|', '');

        if (this.userId !== '') {
          this.skilledWorkersService.getUser$(this.userId).subscribe(
            (skilledWorkerRespone) => {
                if (skilledWorkerRespone !== null) {
                  this.userExists = true;
                  this.userProfile = skilledWorkerRespone;
                }
            }, error => {
              console.log('here is the error' + error);
              this.userExists = false;
              console.error(error);
          });
        }
      });

  }

  public LogCurrentUser()
  {
    console.log(this.userProfile);
  }


  private setCurrentPosition() {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition((position) => {
        this.latitude = position.coords.latitude;
        this.longitude = position.coords.longitude;
        this.zoom = 12;
      });
    }
  }

  onAutocompleteSelected(result: PlaceResult) {
    console.log('this is address object onAutocompleteSelected: ', JSON.stringify(result));
    this.userProfile.streetName = result.address_components.find(x => x.types[0] === 'route').long_name;
    this.userProfile.city = result.address_components.find(x => x.types[0] === 'locality').long_name;
    this.userProfile.state = result.address_components.find(x => x.types[0] === 'administrative_area_level_1').long_name;
    this.userProfile.country = result.address_components.find(x => x.types[0] === 'country').long_name;
    this.userProfile.zipCode = result.address_components.find(x => x.types[0] === 'postal_code').long_name;
  }

  onLocationSelected(location: Location) {
    console.log('this is location object : ', JSON.stringify(location));
    this.latitude = location.latitude;
    this.longitude = location.longitude;
  }

  openBottomSheet(): void {
    this._bottomSheet.open(BottomSheetAlertComponent);
  }
}

@Component({
  selector: 'bottom-sheet-alert',
  templateUrl: 'bottom-sheet-alert.html',
})
export class BottomSheetAlertComponent {
  constructor(private _bottomSheetRef: MatBottomSheetRef<BottomSheetAlertComponent>) {}

  openLink(event: MouseEvent): void {
    this._bottomSheetRef.dismiss();
    event.preventDefault();
  }
}
