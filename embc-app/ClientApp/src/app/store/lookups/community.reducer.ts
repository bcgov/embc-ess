import { Loadable } from '..';
import { Community } from 'app/core/models';
import * as CommunityActions from './community.actions';

export interface State extends Loadable {
  communities: Community[];
}

export const initialState: State = {
  communities: [],
  loading: false,
  loaded: false,
  error: null,
};

export function reducer(state = initialState, action: CommunityActions.Actions): State {
  switch (action.type) {
    case CommunityActions.LOAD_START:
      return { ...state, loading: true };

    case CommunityActions.LOAD_SUCCESS:
      const communities = action.payload.communities;
      return { ...state, communities, loading: false, loaded: true, };

    case CommunityActions.LOAD_FAIL:
      return { ...state, loading: false, loaded: false };

    default:
      return state;
  }
}
