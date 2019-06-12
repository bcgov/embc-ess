import { combineReducers, Action } from '@ngrx/store';

import * as fromConfig from './config.reducer';
import * as fromCountries from './country.reducer';
import * as fromRegions from './region.reducer';
import * as fromCommunities from './community.reducer';
import * as fromRelationshipTypes from './relationship-type.reducer';

// TODO: <-- more lookups here...
export interface State {
  config: fromConfig.State;
  countries: fromCountries.State;
  regions: fromRegions.State;
  communities: fromCommunities.State;
  relationshipTypes: fromRelationshipTypes.State;
}

export const initialState: State = {
  config: fromConfig.initialState,
  countries: fromCountries.initialState,
  regions: fromRegions.initialState,
  communities: fromCommunities.initialState,
  relationshipTypes: fromRelationshipTypes.initialState,
};

const combined = combineReducers<State>({
  config: fromConfig.reducer,
  countries: fromCountries.reducer,
  regions: fromRegions.reducer,
  communities: fromCommunities.reducer,
  relationshipTypes: fromRelationshipTypes.reducer,
});

// Loaded once at init time, as they do not change very often, and
// certainly not within the app.
export function reducer(state: State, action: Action): State {
  return combined(state, action);
}
