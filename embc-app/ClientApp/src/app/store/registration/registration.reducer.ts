import { Registration, Address } from 'src/app/core/models';
import * as RegistrationActions from './registration.actions';
import { Loadable } from '..';

export interface State extends Loadable {
  registrations: Registration[];
  currentRegistration: Registration | null;
}

const initialState: State = {
  registrations: [],
  currentRegistration: null,
  loading: false,
  loaded: false,
  error: null,
};

const newRegistration: Registration = {
  id: null,
  active: null,
  essFileNumber: null,
  restrictedAccess: null,
  declarationAndConsent: null,
  registeringFamilyMembers: null,
  dietaryNeeds: null,
  dietaryNeedsDetails: null,
  medicationNeeds: null,
  hasThreeDayMedicationSupply: null,
  hasPets: null,
  insuranceCode: null,
  requiresSupport: null,
  requiresFood: null,
  requiresClothing: null,
  requiresAccommodation: null,
  requiresIncidentals: null,
  requiresTransportation: null,
  facility: null,
  disasterAffectDetails: null,
  externalReferralsDetails: null,
  familyRecoveryPlan: null,
  internalCaseNotes: null,
  hasInquiryReferral: null,
  hasHealthServicesReferral: null,
  hasFirstAidReferral: null,
  hasPersonalServicesReferral: null,
  hasChildCareReferral: null,
  hasPetCareReferral: null,
  selfRegisteredDate: null,
  registrationCompletionDate: null,
  headOfHousehold: {
    id: null,
    personType: 'HOH',
    lastName: null,
    firstName: null,
    initials: null,
    nickname: null,
    gender: null,
    dob: null,
    phoneNumber: null,
    phoneNumberAlt: null,
    email: null,
    familyMembers: [],
    primaryResidence: null,
    mailingAddress: null,
  },
  incidentTask: null,
  hostCommunity: null,
  completedBy: null,
};

export function reducer(state = initialState, action: RegistrationActions.Actions): State {
  switch (action.type) {
    case RegistrationActions.LOAD_ALL_START: {
      return { ...state, loading: true };
    }

    case RegistrationActions.LOAD_ALL_SUCCESS: {
      const registrations = action.payload.registrations;
      return {
        ...state,
        registrations,
        loading: false,
        loaded: true,
      };
    }

    case RegistrationActions.LOAD_ALL_FAIL: {
      const error = action.payload.error;
      return {
        ...state,
        loading: false,
        loaded: false,
        error,
      };
    }

    case RegistrationActions.LOAD_REGISTRATION_START: {
      return { ...state, loading: true };
    }

    case RegistrationActions.LOAD_REGISTRATION_SUCCESS: {
      const registration = action.payload.registration;
      return {
        ...state,
        currentRegistration: registration,
        loading: false,
        loaded: true,
      };
    }

    case RegistrationActions.LOAD_REGISTRATION_FAIL: {
      const error = action.payload.error;
      return {
        ...state,
        error,
        loading: false,
        loaded: false,
      };
    }

    case RegistrationActions.CREATE_REGISTRATION: {
      const registration = action.payload.registration;
      return {
        ...state,
        registrations: [...state.registrations, registration],
        currentRegistration: registration,
        error: null,
      };
    }

    case RegistrationActions.UPDATE_REGISTRATION: {
      const registration = action.payload.registration;
      const updatedRegistrations = state.registrations.map(item => item.id === registration.id ? registration : item);
      return {
        ...state,
        registrations: updatedRegistrations,
        currentRegistration: registration,
        error: null,
      };
    }

    // After a delete, the currentRegistration is null.
    case RegistrationActions.DELETE_REGISTRATION: {
      const registration = action.payload.registration;
      const updatedRegistrations = state.registrations.filter(item => item.id !== registration.id);
      return {
        ...state,
        registrations: updatedRegistrations,
        currentRegistration: null,
        error: null,
      };
    }

    case RegistrationActions.SET_CURRENT_REGISTRATION: {
      return { ...state, currentRegistration: action.payload.registration };
    }

    case RegistrationActions.CLEAR_CURRENT_REGISTRATION: {
      return { ...state, currentRegistration: null };
    }

    case RegistrationActions.INITIALIZE_CURRENT_REGISTRATION: {
      const newCopy: Registration = { ...newRegistration };
      return { ...state, currentRegistration: newCopy };
    }

    default:
      return state;
  }
}
