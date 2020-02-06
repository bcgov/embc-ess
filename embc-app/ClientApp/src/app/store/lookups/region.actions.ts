import { Action } from '@ngrx/store';
import { Region } from 'app/core/models';

export const LOAD_START = '[Regions] Load Start';
export const LOAD_FAIL = '[Regions] Load Fail';
export const LOAD_SUCCESS = '[Regions] Load Success';

export class LoadRegions implements Action {
  readonly type = LOAD_START;
}

export class LoadRegionsFail implements Action {
  readonly type = LOAD_FAIL;
  constructor(public payload: { error: any }) { }
}

export class LoadRegionsSuccess implements Action {
  readonly type = LOAD_SUCCESS;
  constructor(public payload: { regions: Region[] }) { }
}

// action types
export type Actions = LoadRegions
  | LoadRegionsSuccess
  | LoadRegionsFail;
