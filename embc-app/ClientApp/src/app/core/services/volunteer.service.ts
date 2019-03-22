import { Injectable } from '@angular/core';
import { CoreModule } from '../core.module';
import { Observable, of } from 'rxjs';
import { Volunteer } from '../models';
import { RestService } from './rest.service';

@Injectable({
  providedIn: CoreModule
})
export class VolunteerService extends RestService {

  getVolunteers(): Observable<Volunteer[]> {
    return of([]);
  }
}
