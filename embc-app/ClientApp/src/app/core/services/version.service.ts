import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, retry, map } from 'rxjs/operators';

import { CoreModule } from '../core.module';
import { RestService } from './rest.service';
import { AppVersion } from '../models/app-version.model';

@Injectable({
  providedIn: CoreModule
})
export class VersionService extends RestService {
  apiRoute = '/api/applicationversioninfo';

  // Returns the application version info
  getVersion(): Observable<AppVersion> {
    return this.http.get<AppVersion>(this.apiRoute, { headers: this.headers })
      .pipe(
        catchError(this.handleError)
      );
  }
}
