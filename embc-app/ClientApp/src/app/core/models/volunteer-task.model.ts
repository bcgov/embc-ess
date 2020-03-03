import { Person, Organization } from './';
import { Volunteer } from './volunteer.model';
import { IncidentTask } from './incident-task.model';

// Volunteer information
export interface VolunteerTask  {
  id: number;
  volunteerId: string;
  incidentTaskId: string;

  // related entities
  volunteer: Volunteer;
  incidentTask: IncidentTask;
}
