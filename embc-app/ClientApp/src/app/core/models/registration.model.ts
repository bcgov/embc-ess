import { HeadOfHousehold, FamilyMember, Community, IncidentTask, Volunteer } from './';

export interface Registration {
  // primary identifier
  id: string | null;
  //misc
  dietaryNeedsDetails: string;
  disasterAffectDetails: string;
  essFileNumber: number | null;
  externalReferralsDetails: string;
  facility: string;
  familyRecoveryPlan: string;
  followUpDetails: string;
  insuranceCode: string;
  selfRegisteredDate: Date | null; // datetime
  supportRequired: boolean;
  registrationCompletionDate: Date | null; // datetime

  //Registration state
  isRegisteringFamilyMembers: number;
  isRestrictedAccess: boolean;
  isTakingMedication: boolean;

  //Inventory
  hasThreeDaySupply: boolean;
  hasDietaryNeeds: boolean;
  hasInquiryReferral: boolean;
  hasHealthServicesReferral: boolean;
  hasFirstAidReferral: boolean;
  hasChildCareReferral: boolean;
  hasPersonalServicesReferral: boolean;
  hasPetCareReferral: boolean;
  hasPets: boolean;
  
  //requirements
  requiresAccommodation: boolean;
  requiresClothing: boolean;
  requiresFood: boolean;
  requiresIncidentals: boolean;
  requiresTransportation: boolean;

  // related entities
  headOfHousehold: HeadOfHousehold;
  familyMembers: FamilyMember[];
  incidentTask: IncidentTask | null;
  hostCommunity: Community | null;
  // TODO: Should we link to the full User record for an interviewer or just capture basic info (name + last name initial)?
  completedBy: Volunteer | null;
}
