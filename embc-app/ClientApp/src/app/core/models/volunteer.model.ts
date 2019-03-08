import { Person, Organization } from './';

// Volunteer information
export interface Volunteer extends Person {
  personType: 'HOH';
  bceidAccountNumber: string;
  isAdministrator: boolean;
  isPrimaryContact: boolean;
  canAccessRestrictedFiles: boolean;

  // related entities
  organization: Organization;
}
