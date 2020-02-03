import { Action } from '@ngrx/store';
import { IncidentTask } from 'app/core/models';

// CRUD
export const LOAD_ALL_START = '[IncidentTasks] Load All IncidentTasks Start';
export const LOAD_ALL_SUCCESS = '[IncidentTasks] Load All IncidentTasks Success';
export const LOAD_ALL_FAIL = '[IncidentTasks] Load All IncidentTasks Fail';
export const LOAD_INCIDENT_TASK_START = '[IncidentTasks] Load IncidentTask Start';
export const LOAD_INCIDENT_TASK_SUCCESS = '[IncidentTasks] Load IncidentTask Success';
export const LOAD_INCIDENT_TASK_FAIL = '[IncidentTasks] Load IncidentTask Fail';
export const CREATE_INCIDENT_TASK = '[IncidentTasks] Create IncidentTask';
export const UPDATE_INCIDENT_TASK = '[IncidentTasks] Update IncidentTask';
export const DELETE_INCIDENT_TASK = '[IncidentTasks] Delete IncidentTask';
// Set/Clear Current
export const SET_CURRENT_INCIDENT_TASK = '[IncidentTasks] Set Current IncidentTask';
export const CLEAR_CURRENT_INCIDENT_TASK = '[IncidentTasks] Clear Current IncidentTask';
export const INITIALIZE_CURRENT_INCIDENT_TASK = '[IncidentTasks] Initialize Current IncidentTask';

export class LoadAllIncidentTasks implements Action {
  readonly type = LOAD_ALL_START;
}

export class LoadAllIncidentTasksFail implements Action {
  readonly type = LOAD_ALL_FAIL;
  constructor(public payload: { error: string }) { }
}

export class LoadAllIncidentTasksSuccess implements Action {
  readonly type = LOAD_ALL_SUCCESS;
  constructor(public payload: { incidentTasks: IncidentTask[] }) { }
}

export class LoadIncidentTask implements Action {
  readonly type = LOAD_INCIDENT_TASK_START;
}

export class LoadIncidentTaskFail implements Action {
  readonly type = LOAD_INCIDENT_TASK_FAIL;
  constructor(public payload: { error: string }) { }
}

export class LoadIncidentTaskSuccess implements Action {
  readonly type = LOAD_INCIDENT_TASK_SUCCESS;
  constructor(public payload: { incidentTask: IncidentTask }) { }
}

export class CreateIncidentTask implements Action {
  readonly type = CREATE_INCIDENT_TASK;
  constructor(public payload: { incidentTask: IncidentTask }) { }
}

export class UpdateIncidentTask implements Action {
  readonly type = UPDATE_INCIDENT_TASK;
  constructor(public payload: { incidentTask: IncidentTask }) { }
}

export class DeleteIncidentTask implements Action {
  readonly type = DELETE_INCIDENT_TASK;
  constructor(public payload: { incidentTask: IncidentTask }) { }
}

export class SetCurrentIncidentTask implements Action {
  readonly type = SET_CURRENT_INCIDENT_TASK;
  constructor(public payload: { incidentTask: IncidentTask }) { }
}

export class ClearCurrentIncidentTask implements Action {
  readonly type = CLEAR_CURRENT_INCIDENT_TASK;
}

export class InitializeCurrentIncidentTask implements Action {
  readonly type = INITIALIZE_CURRENT_INCIDENT_TASK;
}

// action types
export type Actions = LoadAllIncidentTasks
  | LoadAllIncidentTasksFail
  | LoadAllIncidentTasksSuccess
  | LoadIncidentTask
  | LoadIncidentTaskFail
  | LoadIncidentTaskSuccess
  | CreateIncidentTask
  | UpdateIncidentTask
  | DeleteIncidentTask
  | SetCurrentIncidentTask
  | ClearCurrentIncidentTask
  | InitializeCurrentIncidentTask;
