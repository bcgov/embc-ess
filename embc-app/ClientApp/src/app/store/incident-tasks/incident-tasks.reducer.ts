import { IncidentTask } from 'app/core/models';
import * as IncidentTaskActions from './incident-tasks.actions';
import { Loadable } from '..';

// const initalIncidentTask: IncidentTask = {
//   id: null,
//   taskNumber: null,
//   details: null,
//   region: null,
//   community: null
// };

export interface State extends Loadable {
  incidentTasks: IncidentTask[];
  currentIncidentTask: IncidentTask | null;
}

const initialState: State = {
  incidentTasks: [],
  currentIncidentTask: {
    id: null, // as string
    taskNumber: null, // as string
    details: null, // as string
    totalAssociatedEvacuees: 0, // as number
    community: null, // as Community
    region: null, // as Region
    startDate: null // as string (datetime)
  },
  loading: false,
  loaded: false,
  error: null,
};

export function reducer(state = initialState, action: IncidentTaskActions.Actions): State {
  switch (action.type) {
    case IncidentTaskActions.LOAD_ALL_START: {
      return { ...state, loading: true };
    }

    case IncidentTaskActions.LOAD_ALL_SUCCESS: {
      const incidentTasks = action.payload.incidentTasks;
      return {
        ...state,
        incidentTasks,
        loading: false,
        loaded: true,
      };
    }

    case IncidentTaskActions.LOAD_ALL_FAIL: {
      const error = action.payload.error;
      return {
        ...state,
        loading: false,
        loaded: false,
        error,
      };
    }

    case IncidentTaskActions.LOAD_INCIDENT_TASK_START: {
      return { ...state, loading: true };
    }

    case IncidentTaskActions.LOAD_INCIDENT_TASK_SUCCESS: {
      const incidentTask = action.payload.incidentTask;
      return {
        ...state,
        currentIncidentTask: incidentTask,
        loading: false,
        loaded: true,
      };
    }

    case IncidentTaskActions.LOAD_INCIDENT_TASK_FAIL: {
      const error = action.payload.error;
      return {
        ...state,
        error,
        loading: false,
        loaded: false,
      };
    }

    case IncidentTaskActions.CREATE_INCIDENT_TASK: {
      const incidentTask = action.payload.incidentTask;
      return {
        ...state,
        incidentTasks: [...state.incidentTasks, incidentTask],
        currentIncidentTask: incidentTask,
        error: null,
      };
    }

    case IncidentTaskActions.UPDATE_INCIDENT_TASK: {
      const incidentTask = action.payload.incidentTask;
      const updatedIncidentTasks = state.incidentTasks.map(item => item.id === incidentTask.id ? incidentTask : item);
      return {
        ...state,
        incidentTasks: updatedIncidentTasks,
        currentIncidentTask: incidentTask,
        error: null,
      };
    }

    // After a delete, the currentIncidentTask is null.
    case IncidentTaskActions.DELETE_INCIDENT_TASK: {
      const incidentTask = action.payload.incidentTask;
      const updatedIncidentTasks = state.incidentTasks.filter(item => item.id !== incidentTask.id);
      return {
        ...state,
        incidentTasks: updatedIncidentTasks,
        currentIncidentTask: null,
        error: null,
      };
    }

    case IncidentTaskActions.SET_CURRENT_INCIDENT_TASK: {
      return { ...state, currentIncidentTask: action.payload.incidentTask };
    }

    case IncidentTaskActions.CLEAR_CURRENT_INCIDENT_TASK: {
      return { ...state, currentIncidentTask: null };
    }

    case IncidentTaskActions.INITIALIZE_CURRENT_INCIDENT_TASK: {
      const newCopy: IncidentTask = { ...initialState.currentIncidentTask };
      return { ...state, currentIncidentTask: newCopy };
    }

    default:
      return state;
  }
}
