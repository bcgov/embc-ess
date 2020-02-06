import { Action } from '@ngrx/store';
import { Community } from 'app/core/models';

export const LOAD_START = '[Communities] Load Start';
export const LOAD_FAIL = '[Communities] Load Fail';
export const LOAD_SUCCESS = '[Communities] Load Success';

export class LoadCommunities implements Action {
  readonly type = LOAD_START;
}

export class LoadCommunitiesFail implements Action {
  readonly type = LOAD_FAIL;
  constructor(public payload: { error: any }) { }
}

export class LoadCommunitiesSuccess implements Action {
  readonly type = LOAD_SUCCESS;
  constructor(public payload: { communities: Community[] }) { }
}

// action types
export type Actions = LoadCommunities
  | LoadCommunitiesSuccess
  | LoadCommunitiesFail;
