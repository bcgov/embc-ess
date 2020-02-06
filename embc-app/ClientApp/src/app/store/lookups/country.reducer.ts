import { Loadable } from '..';
import { Country } from 'app/core/models';
import * as CountryActions from './country.actions';

export interface State extends Loadable {
  countries: Country[];
}

export const initialState: State = {
  countries: [],
  loading: false,
  loaded: false,
  error: null,
};

export function reducer(state = initialState, action: CountryActions.Actions): State {
  switch (action.type) {
    case CountryActions.LOAD_START:
      return { ...state, loading: true };

    case CountryActions.LOAD_SUCCESS:
      const countries = action.payload.countries;
      return { ...state, countries, loading: false, loaded: true, };

    case CountryActions.LOAD_FAIL:
      return { ...state, loading: false, loaded: false };

    default:
      return state;
  }
}
