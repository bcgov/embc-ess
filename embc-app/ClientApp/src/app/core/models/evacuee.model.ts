import { Person, Address, RelationshipType } from './';

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
  familyMembers: FamilyMember[];
}

export interface FamilyMember extends Evacuee {
  personType: 'FMBR';
  sameLastNameAsEvacuee: boolean;
  relationshipToEvacuee: RelationshipType;
}

export interface EvacueeListItem {
  id: string;
  restrictedAccess: boolean;
  isHeadOfHousehold: boolean;
  firstName: string;
  lastName: string;
  nickname: string;
  initials: string;
  registrationId: string;
  incidentTaskNumber: string;
  evacuatedFrom: string;
  evacuatedTo: string;
  registrationCompletionDate?: string;
  isFinalized: boolean;
  hasReferrals: boolean;
}
