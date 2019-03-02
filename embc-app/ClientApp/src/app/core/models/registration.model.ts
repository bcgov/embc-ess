import { Person, User, FamilyMember, Community, IncidentTask } from '.';

export interface Registration {
  id: string | null;
  essFileNumber: number | null;
  isRestrictedAccess: boolean;
  familyRepresentative: Person;
  isRegisteringFamilyMembers: number;
  familyMembers: FamilyMember[];
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
  incidentTask: IncidentTask | null;
  hostCommunity: Community | null;
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

  // TODO: Should we link to the full User record for an interviewer or just capture basic info (name + last name initial)?
  interviewer: User;
  interviewerFirstName: string;
  interviewerLastNameInitial: string;

  selfRegisteredDate: Date | null; // datetime
  registrationCompletionDate: Date | null; // datetime
}
