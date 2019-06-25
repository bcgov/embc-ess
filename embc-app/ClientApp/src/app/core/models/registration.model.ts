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
  petCarePlan: string;
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
  // Deprecated: Back-end computes the completedBy according to session data.
  //  The front-end has no places where this information is available.
  //  The view model in the back end doesn't match anymore.
  completedBy?: Partial<Volunteer> | null;

  // is the registration considered finalized?
  isFinalized?: boolean;
}
export interface RegistrationSummary {
  id: string;
  restrictedAccess: boolean;
  essFileNumber: number;
  selfRegisteredDate: string | null;
  registrationCompletionDate: string | null;
  registeringFamilyMembers: string;
  requiresAccommodation: boolean;
  requiresClothing: boolean;
  requiresFood: boolean;
  requiresIncidentals: boolean;
  requiresTransportation: boolean;
  requiresSupport: boolean;
  headOfHousehold: HeadOfHousehold;
  incidentTask: IncidentTask;
  hostCommunity: Community;
  active: boolean;
  hasInternalCaseNotes: boolean;
  facility: string;
}
