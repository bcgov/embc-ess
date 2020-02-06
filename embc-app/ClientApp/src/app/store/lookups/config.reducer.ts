import { Loadable } from '..';
import { Config } from 'src/app/core/models';
import * as ConfigActions from './config.actions';

export interface State extends Loadable {
  config: Config;
}

export const initialState: State = {
  config: null,
  loading: false,
  loaded: false,
  error: null,
};

export function reducer(state = initialState, action: ConfigActions.Actions): State {
  switch (action.type) {
    case ConfigActions.LOAD_START:
      return { ...state, loading: true };

    case ConfigActions.LOAD_SUCCESS:
      const config = action.payload.config;
      return { ...state, config, loading: false, loaded: true, };

    case ConfigActions.LOAD_FAIL:
      return { ...state, loading: false, loaded: false };

    default:
      return state;
  }
}
