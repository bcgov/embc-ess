import { Action } from '@ngrx/store';
import { RelationshipType } from 'app/core/models';

export const LOAD_START = '[Relationship Types] Load Start';
export const LOAD_FAIL = '[Relationship Types] Load Fail';
export const LOAD_SUCCESS = '[Relationship Types] Load Success';

export class LoadRelationshipTypes implements Action {
  readonly type = LOAD_START;
}

export class LoadRelationshipTypesFail implements Action {
  readonly type = LOAD_FAIL;
  constructor(public payload: { error: any }) { }
}

export class LoadRelationshipTypesSuccess implements Action {
  readonly type = LOAD_SUCCESS;
  constructor(public payload: { relationshipTypes: RelationshipType[] }) { }
}

// action types
export type Actions = LoadRelationshipTypes
  | LoadRelationshipTypesSuccess
  | LoadRelationshipTypesFail;
