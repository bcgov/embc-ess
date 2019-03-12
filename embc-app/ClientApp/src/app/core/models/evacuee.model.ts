import { Person, Address } from './';

export interface Evacuee extends Person {
  bcServicesNumber?: string;
}

export interface HeadOfHousehold extends Evacuee {
  // evacuee information (HOH and family members)
  personType: 'HOH';
  phoneNumber: string;
  phoneNumberAlt: string;
  email: string;
  // related entities
  primaryResidence: Address;
  mailingAddress: Address | null;
}

export interface FamilyMember extends Evacuee {
  personType: 'FMBR';
  relationshipToEvacuee: string;
  sameLastNameAsEvacuee: boolean;
}
