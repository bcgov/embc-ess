import { Loadable } from '..';
import { RegionalDistrict } from 'src/app/core/models';
import * as RegionalDistrictActions from './regional-district.actions';

export interface State extends Loadable {
  regionalDistricts: RegionalDistrict[];
}

export const initialState: State = {
  regionalDistricts: [],
  loading: false,
  loaded: false,
  error: null,
};

export function reducer(state = initialState, action: RegionalDistrictActions.Actions): State {
  switch (action.type) {
    case RegionalDistrictActions.LOAD_START:
      return { ...state, loading: true };

    case RegionalDistrictActions.LOAD_SUCCESS:
      const regionalDistricts = action.payload.regionalDistricts;
      return { ...state, regionalDistricts, loading: false, loaded: true, };

    case RegionalDistrictActions.LOAD_FAIL:
      return { ...state, loading: false, loaded: false };

    default:
      return state;
  }
}
