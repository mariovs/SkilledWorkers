import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from  './login/login.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuard } from './auth.guard';
import { SkilledWrokersService } from './services/skilled-wrokers/skilled-wrokers.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
// import {MatToolbarModule} from '@angular/material/toolbar';
import { CreateProfileComponent } from './create-profile/create-profile.component';
import { UpdateProfileComponent } from './update-profile/update-profile.component';
import { SearchComponent} from './search/search.component';
import { MatGoogleMapsAutocompleteModule } from '@angular-material-extensions/google-maps-autocomplete';
import { AgmCoreModule } from '@agm/core';


import {
  MatCardModule,
  MatInputModule,
  MatButtonModule,
  MatFormFieldModule,
  MatGridListModule,
  MatSliderModule,
  MatToolbarModule,
  MatBottomSheetModule,
  MatDividerModule,
  MatSelectModule,
  MatProgressSpinnerModule
} from '@angular/material';

const materialModules = [
  MatCardModule,
  MatInputModule,
  MatButtonModule,
  MatFormFieldModule,
  MatGridListModule,
  MatSliderModule,
  MatToolbarModule,
  AgmCoreModule,
  MatDividerModule,
  MatBottomSheetModule,
  MatSelectModule,
  MatProgressSpinnerModule
];


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ProfileComponent,
    LoginComponent,
    CreateProfileComponent,
    UpdateProfileComponent,
    SearchComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    materialModules,
    RouterModule.forRoot([
      { path: 'login', component: LoginComponent, pathMatch: 'full' },
      { path: '', component: ProfileComponent, pathMatch: 'full', canActivate: [AuthGuard]},
      { path: 'search', component: SearchComponent, canActivate: [AuthGuard]},
      { path: 'seed', component: SearchComponent, canActivate: [AuthGuard]},
      { path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
      { path: 'counter', component: CounterComponent, canActivate: [AuthGuard]},
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard]}
    ]),
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyB0mdGGBW56PqDxYMQJzycRdH7xvbSWT58  ',
      libraries: ['places']
    }),
    MatGoogleMapsAutocompleteModule,
    BrowserAnimationsModule
  ],
  providers: [SkilledWrokersService],
  bootstrap: [AppComponent]
})
export class AppModule { }
