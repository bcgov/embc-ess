import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { IncidentTask } from '../models';

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
    community: null,
  }
];

@Injectable({
  providedIn: 'root'
})
export class IncidentTaskService {

  constructor() { }
  getIncidentTasks() {
    // return a list of all incident task numbers for use in the application
    return of(INCIDENTTASKS);
  }
}
