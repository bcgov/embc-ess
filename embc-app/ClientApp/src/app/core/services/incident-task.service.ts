import { Injectable } from '@angular/core';
import { of, Observable } from 'rxjs';
import { IncidentTask } from '../models';
import { CoreModule } from '../core.module';
import { RestService } from './rest.service';

const INCIDENTTASKS: IncidentTask[] = [
  {
    id: 'wert',
    taskNumber: '56789',
    details: 'House fire',
    region: null,
    regionalDistrict: null,
    community: null,
  },
  {
    id: 'qwer',
    taskNumber: '56790',
    details: 'Forest fire',
    region: null,
    regionalDistrict: null,
    community: null,
  },
  {
    id: 'erty',
    taskNumber: '56791',
    details: 'Province fire',
    region: null,
    regionalDistrict: null,
    community: {
      id: 'ID ELEMENT',
      name: 'Victoria',
      active: true,
      regionalDistrict: {
        id: '',
        name: '',
        region: {
          name: 'asdasd',
          id: 'adasdasd',
          active: true,
        },
      },
    },
  }
];

@Injectable({
  providedIn: CoreModule
})
export class IncidentTaskService extends RestService {

  getIncidentTasks(): Observable<IncidentTask[]> {
    // return a list of all incident task numbers for use in the application
    return of(INCIDENTTASKS);
  }

  getIncident(id: string): Observable<IncidentTask> {
    // return a single matching incident
    return of(INCIDENTTASKS[0]);
  }
}
