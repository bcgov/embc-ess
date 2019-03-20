import { Injectable } from '@angular/core';

import { CoreModule } from '../core.module';
import { catchError, retry } from 'rxjs/operators';

import { User } from '../models/user.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RestService } from './rest.service';

@Injectable({
  providedIn: CoreModule
})
export class UserDataService extends RestService {

  getCurrentUser() {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');

    return this.http.get<User>('api/users/current', {
      headers: headers
    }).pipe(catchError(this.handleError));
  }

}
