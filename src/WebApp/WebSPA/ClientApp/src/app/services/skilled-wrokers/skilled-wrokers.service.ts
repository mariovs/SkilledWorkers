import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { UserProfileInfo } from '../../models/UserProfileInfo';

@Injectable({
  providedIn: 'root'
})
export class SkilledWrokersService {

  constructor(private http: HttpClient) { }

  getUsers$(): Observable<UserProfileInfo> {
    return this.http.get<UserProfileInfo>('https://localhost:5001/api/v1/profiles/test5');
      // .pipe(
      //   catchError(this.handleError<UserProfileInfo[]>('getHeroes', []))
      // );
  }

  // private handleError<T> (operation = 'operation', result?: T) {
  //   return (error: any): Observable<T> => {
  
  //     // TODO: send the error to remote logging infrastructure
  //     console.error(error); // log to console instead
  
  //     // TODO: better job of transforming error for user consumption
  //     this.log(`${operation} failed: ${error.message}`);
  
  //     // Let the app keep running by returning an empty result.
  //     return of(result as T);
  //   };
  // }
}
