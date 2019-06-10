import { ActionReducerMap, ActionReducer, MetaReducer } from '@ngrx/store';

import * as fromRegistration from './registration/registration.reducer';
import * as fromLookups from './lookups';
import * as fromIncidentTasks from './incident-tasks/incident-tasks.reducer';
import * as fromVolunteers from './volunteer/volunteer.reducer';

export interface Loadable {
  loading: boolean;
  loaded: boolean;
  error: string;
}

export interface NormalizedObjects<T = any> {
  [id: string]: T;
}

export interface AppState {
  registrations: fromRegistration.State;
  lookups: fromLookups.State;
  incidentTasks: fromIncidentTasks.State;
  volunteers: fromVolunteers.State;
}

/**
 * Our state is composed of a map of action reducer functions.
 * These reducer functions are called with each dispatched action
 * and the current or initial state and return a new immutable state.
 */
export const rootReducer: ActionReducerMap<AppState> = {
  registrations: fromRegistration.reducer,
  lookups: fromLookups.reducer,
  incidentTasks: fromIncidentTasks.reducer,
  volunteers: fromVolunteers.reducer,
};

// TODO: Discard this when done developing/debugging
// console.log all actions
export function logger(reducer: ActionReducer<AppState>): ActionReducer<AppState> {
  return (state, action) => {
    const result = reducer(state, action);
    console.groupCollapsed(action.type);
    console.log('prev state', state);
    console.log('action', action);
    console.log('next state', result);
    console.groupEnd();

    return result;
  };
}

export const metaReducers: MetaReducer<AppState>[] = [logger];
