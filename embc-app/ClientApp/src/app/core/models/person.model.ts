import { Address } from './';

export interface Person {
  id: string | null;
  active?: boolean;
  personType: string;  // --> 'VOLN' (volunteer), 'HOH' (head of household), 'FMBR' (family member)
  firstName: string;
  lastName: string;
  nickname: string;
  initials: string;
  gender: string;
  dob: Date | null;
}
