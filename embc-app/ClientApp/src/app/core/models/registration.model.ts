import { HeadOfHousehold, FamilyMember, Community, IncidentTask, Volunteer } from './';

export interface Registration {
  // Important
  id: string | null;
  restrictedAccess: boolean;
  declarationAndConsent: boolean;
  essFileNumber: number | null;

  // Registration Record
  dietaryNeeds: boolean;
  dietaryNeedsDetails: string;
  disasterAffectDetails: string;
  externalReferralsDetails: string;
  facility: string;
  familyRecoveryPlan: string;
  followUpDetails: string;
  insuranceCode: string;  // one of ['yes', 'yes-unsure', 'no', 'unsure']
  medicationNeeds: boolean;
  selfRegisteredDate: Date | null; // datetime
  registrationCompletionDate: Date | null; // datetime
  registeringFamilyMembers: string;  // one of ['yes', 'yes-later', 'no']

  // Family state flags
  hasThreeDayMedicationSupply: boolean;
  hasInquiryReferral: boolean;
  hasHealthServicesReferral: boolean;
  hasFirstAidReferral: boolean;
  hasChildCareReferral: boolean;
  hasPersonalServicesReferral: boolean;
  hasPetCareReferral: boolean;
  hasPets: boolean;

  // requirements
  requiresAccommodation: boolean;
  requiresClothing: boolean;
  requiresFood: boolean;
  requiresIncidentals: boolean;
  requiresTransportation: boolean;
  requiresSupport: boolean;

  // related entities
  headOfHousehold: HeadOfHousehold;
  familyMembers: FamilyMember[];
  incidentTask: IncidentTask | null;
  hostCommunity: Community | null;
  // TODO: Should we link to the full User record for an interviewer or just capture basic info (name + last name initial)?
  completedBy: Volunteer | null;
}
