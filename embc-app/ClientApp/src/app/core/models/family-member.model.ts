import { Evacuee } from './';

export interface FamilyMember extends Evacuee {
  personType: 'FMBR';
  relationshipToEvacuee: string;
  sameLastNameAsEvacuee: boolean;
}
