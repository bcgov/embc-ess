import { Action } from '@ngrx/store';
import { Registration } from 'src/app/core/models';

export const UPDATE_REGISTRATION = 'UPDATE_REGISTRATION';

export class UpdateRegistration implements Action {
  readonly type = UPDATE_REGISTRATION;
  constructor(public payload: Registration) {}
}

export type RegistrationAction = UpdateRegistration;
