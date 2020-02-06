import { Action } from '@ngrx/store';
import { Registration } from 'src/app/core/models';

// CRUD
export const LOAD_ALL_START = '[Registrations] Load All Registrations Start';
export const LOAD_ALL_SUCCESS = '[Registrations] Load All Registrations Success';
export const LOAD_ALL_FAIL = '[Registrations] Load All Registrations Fail';
export const LOAD_REGISTRATION_START = '[Registrations] Load Registration Start';
export const LOAD_REGISTRATION_SUCCESS = '[Registrations] Load Registration Success';
export const LOAD_REGISTRATION_FAIL = '[Registrations] Load Registration Fail';
export const CREATE_REGISTRATION = '[Registrations] Create Registration';
export const UPDATE_REGISTRATION = '[Registrations] Update Registration';
export const DELETE_REGISTRATION = '[Registrations] Delete Registration';
// Set/Clear Current
export const SET_CURRENT_REGISTRATION = '[Registrations] Set Current Registration';
export const CLEAR_CURRENT_REGISTRATION = '[Registrations] Clear Current Registration';
export const INITIALIZE_CURRENT_REGISTRATION = '[Registrations] Initialize Current Registration';

export class LoadAllRegistrations implements Action {
  readonly type = LOAD_ALL_START;
}

export class LoadAllRegistrationsFail implements Action {
  readonly type = LOAD_ALL_FAIL;
  constructor(public payload: { error: string }) { }
}

export class LoadAllRegistrationsSuccess implements Action {
  readonly type = LOAD_ALL_SUCCESS;
  constructor(public payload: { registrations: Registration[] }) { }
}

export class LoadRegistration implements Action {
  readonly type = LOAD_REGISTRATION_START;
}

export class LoadRegistrationFail implements Action {
  readonly type = LOAD_REGISTRATION_FAIL;
  constructor(public payload: { error: string }) { }
}

export class LoadRegistrationSuccess implements Action {
  readonly type = LOAD_REGISTRATION_SUCCESS;
  constructor(public payload: { registration: Registration }) { }
}

export class CreateRegistration implements Action {
  readonly type = CREATE_REGISTRATION;
  constructor(public payload: { registration: Registration }) { }
}

export class UpdateRegistration implements Action {
  readonly type = UPDATE_REGISTRATION;
  constructor(public payload: { registration: Registration }) { }
}

export class DeleteRegistration implements Action {
  readonly type = DELETE_REGISTRATION;
  constructor(public payload: { registration: Registration }) { }
}

export class SetCurrentRegistration implements Action {
  readonly type = SET_CURRENT_REGISTRATION;
  constructor(public payload: { registration: Registration }) { }
}

export class ClearCurrentRegistration implements Action {
  readonly type = CLEAR_CURRENT_REGISTRATION;
}

export class InitializeCurrentRegistration implements Action {
  readonly type = INITIALIZE_CURRENT_REGISTRATION;
}

// action types
export type Actions = LoadAllRegistrations
  | LoadAllRegistrationsFail
  | LoadAllRegistrationsSuccess
  | LoadRegistration
  | LoadRegistrationFail
  | LoadRegistrationSuccess
  | CreateRegistration
  | UpdateRegistration
  | DeleteRegistration
  | SetCurrentRegistration
  | ClearCurrentRegistration
  | InitializeCurrentRegistration;
