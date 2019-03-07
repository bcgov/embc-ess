import { Action } from '@ngrx/store';
import { RegionalDistrict } from 'src/app/core/models';

export const LOAD_START = '[Regional Districts] Load Start';
export const LOAD_FAIL = '[Regional Districts] Load Fail';
export const LOAD_SUCCESS = '[Regional Districts] Load Success';

export class LoadRegionalDistricts implements Action {
  readonly type = LOAD_START;
}

export class LoadRegionalDistrictsFail implements Action {
  readonly type = LOAD_FAIL;
  constructor(public payload: { error: any }) { }
}

export class LoadRegionalDistrictsSuccess implements Action {
  readonly type = LOAD_SUCCESS;
  constructor(public payload: { regionalDistricts: RegionalDistrict[] }) { }
}

// action types
export type Actions = LoadRegionalDistricts
  | LoadRegionalDistrictsSuccess
  | LoadRegionalDistrictsFail;
