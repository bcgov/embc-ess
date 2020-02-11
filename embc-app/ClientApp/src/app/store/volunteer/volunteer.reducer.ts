import { Volunteer } from 'src/app/core/models';
import * as VolunteerActions from './volunteer.actions';
import { Loadable } from '..';


export interface State extends Loadable {
  volunteers: Volunteer[];
  currentVolunteer: Volunteer | null;
}

const initialState: State = {
  volunteers: [],
  currentVolunteer: {
    id: null,
    personType: 'VOLN',
    bceidAccountNumber: null,
    isAdministrator: null,
    isPrimaryContact: null,
    canAccessRestrictedFiles: null,
    electronicAccessAgreementAccepted: false,
    firstName: null,
    lastName: null,
    nickname: null,
    initials: null,
    gender: null,
    dob: null,

    // related entities
    organization: null,
  },
  loading: false,
  loaded: false,
  error: null,
};

export function reducer(state = initialState, action: VolunteerActions.Actions): State {
  switch (action.type) {
    case VolunteerActions.LOAD_ALL_START: {
      return { ...state, loading: true };
    }

    case VolunteerActions.LOAD_ALL_SUCCESS: {
      const volunteers = action.payload.volunteers;
      return {
        ...state,
        volunteers,
        loading: false,
        loaded: true,
      };
    }

    case VolunteerActions.LOAD_ALL_FAIL: {
      const error = action.payload.error;
      return {
        ...state,
        loading: false,
        loaded: false,
        error,
      };
    }

    case VolunteerActions.LOAD_VOLUNTEER_START: {
      return { ...state, loading: true };
    }

    case VolunteerActions.LOAD_VOLUNTEER_SUCCESS: {
      const volunteer = action.payload.volunteer;
      return {
        ...state,
        currentVolunteer: volunteer,
        loading: false,
        loaded: true,
      };
    }

    case VolunteerActions.LOAD_VOLUNTEER_FAIL: {
      const error = action.payload.error;
      return {
        ...state,
        error,
        loading: false,
        loaded: false,
      };
    }

    case VolunteerActions.CREATE_VOLUNTEER: {
      const volunteer = action.payload.volunteer;
      return {
        ...state,
        volunteers: [...state.volunteers, volunteer],
        currentVolunteer: volunteer,
        error: null,
      };
    }

    case VolunteerActions.UPDATE_VOLUNTEER: {
      const volunteer = action.payload.volunteer;
      const updatedVolunteers = state.volunteers.map(item => item.id === volunteer.id ? volunteer : item);
      return {
        ...state,
        volunteers: updatedVolunteers,
        currentVolunteer: volunteer,
        error: null,
      };
    }

    // After a delete, the currentVolunteer is null.
    case VolunteerActions.DELETE_VOLUNTEER: {
      const volunteer = action.payload.volunteer;
      const updatedVolunteers = state.volunteers.filter(item => item.id !== volunteer.id);
      return {
        ...state,
        volunteers: updatedVolunteers,
        currentVolunteer: null,
        error: null,
      };
    }

    case VolunteerActions.SET_CURRENT_VOLUNTEER: {
      return { ...state, currentVolunteer: action.payload.volunteer };
    }

    case VolunteerActions.CLEAR_CURRENT_VOLUNTEER: {
      return { ...state, currentVolunteer: null };
    }

    case VolunteerActions.INITIALIZE_CURRENT_VOLUNTEER: {
      const newCopy: Volunteer = { ...initialState.currentVolunteer };
      return { ...state, currentVolunteer: newCopy };
    }

    default:
      return state;
  }
}
