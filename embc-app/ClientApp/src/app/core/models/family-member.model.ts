import { Person } from '.';

export interface FamilyMember extends Person {
  relationshipToEvacuee: string;
  sameLastNameAsEvacuee: boolean;
}
