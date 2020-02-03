import { Loadable } from '..';
import { RelationshipType } from 'app/core/models';
import * as RelationshipTypeActions from './relationship-type.actions';

export interface State extends Loadable {
  relationshipTypes: RelationshipType[];
}

export const initialState: State = {
  relationshipTypes: [],
  loading: false,
  loaded: false,
  error: null,
};

export function reducer(state = initialState, action: RelationshipTypeActions.Actions): State {
  switch (action.type) {
    case RelationshipTypeActions.LOAD_START:
      return { ...state, loading: true };

    case RelationshipTypeActions.LOAD_SUCCESS:
      const relationshipTypes = action.payload.relationshipTypes;
      return { ...state, relationshipTypes, loading: false, loaded: true, };

    case RelationshipTypeActions.LOAD_FAIL:
      return { ...state, loading: false, loaded: false };

    default:
      return state;
  }
}
