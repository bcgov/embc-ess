import { Action } from '@ngrx/store';
// import { Volunteer } from 'src/app/core/models';

// CRUD
// export const LOAD_ALL_START = '[Volunteers] Load All Volunteers Start';
// export const LOAD_ALL_SUCCESS = '[Volunteers] Load All Volunteers Success';
// export const LOAD_ALL_FAIL = '[Volunteers] Load All Volunteers Fail';
// export const LOAD_VOLUNTEER_START = '[Volunteers] Load Volunteer Start';
// export const LOAD_VOLUNTEER_SUCCESS = '[Volunteers] Load Volunteer Success';
// export const LOAD_VOLUNTEER_FAIL = '[Volunteers] Load Volunteer Fail';
// export const CREATE_VOLUNTEER = '[Volunteers] Create Volunteer';
// export const UPDATE_VOLUNTEER = '[Volunteers] Update Volunteer';
// export const DELETE_VOLUNTEER = '[Volunteers] Delete Volunteer';
// Set/Clear Current
export const SET_CURRENT_VOLUNTEER_TASK = '[Volunteers] Set Current Task for logged-in Volunteer';
export const CLEAR_CURRENT_VOLUNTEER_TASK = '[Volunteers] Clear Current Task for logged-in Volunteer';
export const INITIALIZE_CURRENT_VOLUNTEER_TASK = '[Volunteers] Initialize Current Task for logged-in Volunteer';


export class SetCurrentVolunteerTask implements Action {
  readonly type = SET_CURRENT_VOLUNTEER_TASK;
  constructor(public payload: { task: string }) { }
}

export class ClearCurrentVolunteerTask implements Action {
  readonly type = CLEAR_CURRENT_VOLUNTEER_TASK;
}

export class InitializeCurrentVolunteerTask implements Action {
  readonly type = INITIALIZE_CURRENT_VOLUNTEER_TASK;
}

// action types
export type Actions = SetCurrentVolunteerTask
  | ClearCurrentVolunteerTask
  | InitializeCurrentVolunteerTask;
