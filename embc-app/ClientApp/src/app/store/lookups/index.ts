import { combineReducers } from '@ngrx/store';

import * as fromCountries from './country.reducer';
import * as fromRegions from './region.reducer';
import * as fromRegionalDistricts from './regional-district.reducer';
import * as fromCommunities from './community.reducer';
import * as fromRelationshipTypes from './relationship-type.reducer';

// TODO: <-- more lookups here...
export interface State {
  countries: fromCountries.State;
  regions: fromRegions.State;
  regionalDistricts: fromRegionalDistricts.State;
  communities: fromCommunities.State;
  relationshipTypes: fromRelationshipTypes.State;
}

export const initialState: State = {
  countries: fromCountries.initialState,
  regions: fromRegions.initialState,
  regionalDistricts: fromRegionalDistricts.initialState,
  communities: fromCommunities.initialState,
  relationshipTypes: fromRelationshipTypes.initialState,
};

// Loaded once at init time, as they do not change very often, and
// certainly not within the app.
export const reducer = combineReducers<State>({
  countries: fromCountries.reducer,
  regions: fromRegions.reducer,
  regionalDistricts: fromRegionalDistricts.reducer,
  communities: fromCommunities.reducer,
  relationshipTypes: fromRelationshipTypes.reducer,
});
