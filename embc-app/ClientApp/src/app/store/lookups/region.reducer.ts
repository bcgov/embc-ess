import { Loadable } from '..';
import { Region } from 'app/core/models';
import * as RegionActions from './region.actions';

export interface State extends Loadable {
  regions: Region[];
}

export const initialState: State = {
  regions: [],
  loading: false,
  loaded: false,
  error: null,
};

export function reducer(state = initialState, action: RegionActions.Actions): State {
  switch (action.type) {
    case RegionActions.LOAD_START:
      return { ...state, loading: true };

    case RegionActions.LOAD_SUCCESS:
      const regions = action.payload.regions;
      return { ...state, regions, loading: false, loaded: true, };

    case RegionActions.LOAD_FAIL:
      return { ...state, loading: false, loaded: false };

    default:
      return state;
  }
}
