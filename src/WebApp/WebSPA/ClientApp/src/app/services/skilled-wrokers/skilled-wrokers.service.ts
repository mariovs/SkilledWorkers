import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { UserProfileInfo } from '../../models/UserProfileInfo';

@Injectable({
  providedIn: 'root'
})
export class SkilledWrokersService {
  baseServiceAddress = "https://localhost:5001";
  constructor(private http: HttpClient) { }

  getUser$(userId): Observable<UserProfileInfo> {
    console.log('this is the url' + this.baseServiceAddress + '/api/v1/profiles/' + userId);
    return this.http.get<UserProfileInfo>(this.baseServiceAddress + '/api/v1/profiles/' + userId);
  }

  createUser$(user): Observable<UserProfileInfo> {
    return this.http.post<UserProfileInfo>(this.baseServiceAddress + '/api/v1/profiles/', user);
  }

  getUsers$(): Observable<UserProfileInfo> {
    return this.http.get<UserProfileInfo>('https://localhost:5001/api/v1/profiles/test5');
  }

  
}
