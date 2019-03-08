import { HeadOfHousehold, FamilyMember, Community, IncidentTask, Volunteer } from './';

export interface Registration {
  id: string | null;
  essFileNumber: number | null;
  isRestrictedAccess: boolean;
  isRegisteringFamilyMembers: number;
  hasDietaryNeeds: boolean;
  dietaryNeedsDetails: string;
  isTakingMedication: boolean;
  hasThreeDaySupply: boolean;
  hasPets: boolean;
  insuranceCode: string;
  isSupportRequired: boolean;
  requiresFood: boolean;
  requiresClothing: boolean;
  requiresAccommodation: boolean;
  requiresIncidentals: boolean;
  requiresTransportation: boolean;
  facility: string;
  disasterAffectDetails: string;
  externalReferralsDetails: string;
  familyRecoveryPlan: string;
  followUpDetails: string;
  hasInquiryReferral: boolean;
  hasHealthServicesReferral: boolean;
  hasFirstAidReferral: boolean;
  hasPersonalServicesReferral: boolean;
  hasChildCareReferral: boolean;
  hasPetCareReferral: boolean;
  selfRegisteredDate: Date | null; // datetime
  registrationCompletionDate: Date | null; // datetime

  // related entities
  headOfHousehold: HeadOfHousehold;
  familyMembers: FamilyMember[];
  incidentTask: IncidentTask | null;
  hostCommunity: Community | null;
  // TODO: Should we link to the full User record for an interviewer or just capture basic info (name + last name initial)?
  completedBy: Volunteer | null;
}
