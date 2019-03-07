import { Action } from '@ngrx/store';
import { Country } from 'src/app/core/models';

export const LOAD_START = '[Countries] Load Start';
export const LOAD_FAIL = '[Countries] Load Fail';
export const LOAD_SUCCESS = '[Countries] Load Success';

export class LoadCountries implements Action {
  readonly type = LOAD_START;
}

export class LoadCountriesFail implements Action {
  readonly type = LOAD_FAIL;
  constructor(public payload: { error: any }) { }
}

export class LoadCountriesSuccess implements Action {
  readonly type = LOAD_SUCCESS;
  constructor(public payload: { countries: Country[] }) { }
}

// action types
export type Actions = LoadCountries
  | LoadCountriesSuccess
  | LoadCountriesFail;
