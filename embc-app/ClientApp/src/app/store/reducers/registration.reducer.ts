import { Registration, Address } from 'src/app/core/models';
import { RegistrationAction, UPDATE_REGISTRATION } from '../actions/registration.action';

const initialAddress: Address = {
  addressLine1: null,
  addressLine2: null,
  addressLine3: null,
  community: null,
  province: null,
  postalCode: null,
  country: null,
};

const initialState: Registration = {
  id: null,
  isRestricted: null,
  familyRepresentative: {
    id: null,
    lastName: null,
    firstName: null,
    initials: null,
    nickname: null,
    gender: null,
    dob: null,
    profile: {
      phoneNumber: null,
      phoneNumberAlt: null,
      email: null,
      primaryResidence: { ...initialAddress },
      mailingAddress: { ...initialAddress },
    },
  },
  isRegisteringFamilyMembers: null,
  familyMembers: [],
  interviewer: null,
  interviewerFirstName: null,
  interviewerLastNameInitial: null,
  startDate: null,
  endDate: null,
  specialNeeds: {},
  isSupportRequired: false,
  requestedSupportServices: {}
};

export function registrationReducer(state = initialState, action: RegistrationAction) {
  switch (action.type) {
    case UPDATE_REGISTRATION:
      return { ...state, ...action.payload };
    default:
      return state;
  }
}
