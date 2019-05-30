import { HeadOfHousehold, Community, IncidentTask, Volunteer } from './';

export interface Registration {
  // Important
  id: string | null;
  active: boolean;
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
  hasInternalCaseNotes?: boolean; // from BE
  internalCaseNotes: string | null;
  insuranceCode: string;  // one of ['yes', 'yes-unsure', 'no', 'unsure']
  medicationNeeds: boolean;
  registrationCompletionDate: string | null; // datetime
  registeringFamilyMembers: string;  // one of ['yes', 'yes-later', 'no']
  selfRegisteredDate: string | null; // datetime

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

  // related entities
  headOfHousehold: HeadOfHousehold;
  incidentTask: IncidentTask | null;
  hostCommunity: Community | null;
  // Make this as Partial so we can don't have to send the whole volunteer object; i.e. { id: 'guid' }
  completedBy: Partial<Volunteer> | null;
}
