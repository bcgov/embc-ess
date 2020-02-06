import { Action } from '@ngrx/store';
import { Config } from 'src/app/core/models';

export const LOAD_START = '[Config] Load Start';
export const LOAD_FAIL = '[Config] Load Fail';
export const LOAD_SUCCESS = '[Config] Load Success';

export class LoadConfig implements Action {
  readonly type = LOAD_START;
}

export class LoadConfigFail implements Action {
  readonly type = LOAD_FAIL;
  constructor(public payload: { error: any }) { }
}

export class LoadConfigSuccess implements Action {
  readonly type = LOAD_SUCCESS;
  constructor(public payload: { config: Config }) { }
}

// action types
export type Actions = LoadConfig
  | LoadConfigSuccess
  | LoadConfigFail;
