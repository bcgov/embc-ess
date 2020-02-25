import { Volunteer } from 'src/app/core/models';
import * as VolunteerTaskActions from './volunteer-task.actions';
import { Loadable } from '..';


export interface State {
  taskNumber: string;
}

const initialState: State = {
  taskNumber: null,
};

export function reducer(state = initialState, action: VolunteerTaskActions.Actions): State {
  switch (action.type) {
    // case VolunteerActions.LOAD_ALL_START: {
    //   return { ...state, loading: true };
    // }

    // case VolunteerActions.LOAD_ALL_SUCCESS: {
    //   const volunteers = action.payload.volunteers;
    //   return {
    //     ...state,
    //     volunteers,
    //     loading: false,
    //     loaded: true,
    //   };
    // }

    // case VolunteerActions.LOAD_ALL_FAIL: {
    //   const error = action.payload.error;
    //   return {
    //     ...state,
    //     loading: false,
    //     loaded: false,
    //     error,
    //   };
    // }

    // case VolunteerActions.LOAD_VOLUNTEER_START: {
    //   return { ...state, loading: true };
    // }

    // case VolunteerActions.LOAD_VOLUNTEER_SUCCESS: {
    //   const volunteer = action.payload.volunteer;
    //   return {
    //     ...state,
    //     currentVolunteer: volunteer,
    //     loading: false,
    //     loaded: true,
    //   };
    // }

    // case VolunteerActions.LOAD_VOLUNTEER_FAIL: {
    //   const error = action.payload.error;
    //   return {
    //     ...state,
    //     error,
    //     loading: false,
    //     loaded: false,
    //   };
    // }

    // case VolunteerActions.CREATE_VOLUNTEER: {
    //   const volunteer = action.payload.volunteer;
    //   return {
    //     ...state,
    //     volunteers: [...state.volunteers, volunteer],
    //     currentVolunteer: volunteer,
    //     error: null,
    //   };
    // }

    // case VolunteerActions.UPDATE_VOLUNTEER: {
    //   const volunteer = action.payload.volunteer;
    //   const updatedVolunteers = state.volunteers.map(item => item.id === volunteer.id ? volunteer : item);
    //   return {
    //     ...state,
    //     volunteers: updatedVolunteers,
    //     currentVolunteer: volunteer,
    //     error: null,
    //   };
    // }

    // // After a delete, the currentVolunteer is null.
    // case VolunteerActions.DELETE_VOLUNTEER: {
    //   const volunteer = action.payload.volunteer;
    //   const updatedVolunteers = state.volunteers.filter(item => item.id !== volunteer.id);
    //   return {
    //     ...state,
    //     volunteers: updatedVolunteers,
    //     currentVolunteer: null,
    //     error: null,
    //   };
    // }

    case VolunteerTaskActions.SET_CURRENT_VOLUNTEER_TASK: {
      return { ...state,   taskNumber: action.payload.task };
    }

    case VolunteerTaskActions.CLEAR_CURRENT_VOLUNTEER_TASK: {
      return { ...state, taskNumber: null };
    }

    case VolunteerTaskActions.INITIALIZE_CURRENT_VOLUNTEER_TASK: {
      return { ...state, ...initialState };
    }

    default:
      return state;
  }
}
