import { Action } from '@ngrx/store';
import { Volunteer } from 'src/app/core/models';

// CRUD
export const LOAD_ALL_START = '[Volunteers] Load All Volunteers Start';
export const LOAD_ALL_SUCCESS = '[Volunteers] Load All Volunteers Success';
export const LOAD_ALL_FAIL = '[Volunteers] Load All Volunteers Fail';
export const LOAD_VOLUNTEER_START = '[Volunteers] Load Volunteer Start';
export const LOAD_VOLUNTEER_SUCCESS = '[Volunteers] Load Volunteer Success';
export const LOAD_VOLUNTEER_FAIL = '[Volunteers] Load Volunteer Fail';
export const CREATE_VOLUNTEER = '[Volunteers] Create Volunteer';
export const UPDATE_VOLUNTEER = '[Volunteers] Update Volunteer';
export const DELETE_VOLUNTEER = '[Volunteers] Delete Volunteer';
// Set/Clear Current
export const SET_CURRENT_VOLUNTEER = '[Volunteers] Set Current Volunteer';
export const CLEAR_CURRENT_VOLUNTEER = '[Volunteers] Clear Current Volunteer';
export const INITIALIZE_CURRENT_VOLUNTEER = '[Volunteers] Initialize Current Volunteer';

export class LoadAllVolunteers implements Action {
  readonly type = LOAD_ALL_START;
}

export class LoadAllVolunteersFail implements Action {
  readonly type = LOAD_ALL_FAIL;
  constructor(public payload: { error: string }) { }
}

export class LoadAllVolunteersSuccess implements Action {
  readonly type = LOAD_ALL_SUCCESS;
  constructor(public payload: { volunteers: Volunteer[] }) { }
}

export class LoadVolunteer implements Action {
  readonly type = LOAD_VOLUNTEER_START;
}

export class LoadVolunteerFail implements Action {
  readonly type = LOAD_VOLUNTEER_FAIL;
  constructor(public payload: { error: string }) { }
}

export class LoadVolunteerSuccess implements Action {
  readonly type = LOAD_VOLUNTEER_SUCCESS;
  constructor(public payload: { volunteer: Volunteer }) { }
}

export class CreateVolunteer implements Action {
  readonly type = CREATE_VOLUNTEER;
  constructor(public payload: { volunteer: Volunteer }) { }
}

export class UpdateVolunteer implements Action {
  readonly type = UPDATE_VOLUNTEER;
  constructor(public payload: { volunteer: Volunteer }) { }
}

export class DeleteVolunteer implements Action {
  readonly type = DELETE_VOLUNTEER;
  constructor(public payload: { volunteer: Volunteer }) { }
}

export class SetCurrentVolunteer implements Action {
  readonly type = SET_CURRENT_VOLUNTEER;
  constructor(public payload: { volunteer: Volunteer }) { }
}

export class ClearCurrentVolunteer implements Action {
  readonly type = CLEAR_CURRENT_VOLUNTEER;
}

export class InitializeCurrentVolunteer implements Action {
  readonly type = INITIALIZE_CURRENT_VOLUNTEER;
}

// action types
export type Actions = LoadAllVolunteers
  | LoadAllVolunteersFail
  | LoadAllVolunteersSuccess
  | LoadVolunteer
  | LoadVolunteerFail
  | LoadVolunteerSuccess
  | CreateVolunteer
  | UpdateVolunteer
  | DeleteVolunteer
  | SetCurrentVolunteer
  | ClearCurrentVolunteer
  | InitializeCurrentVolunteer;
