import { User, Registration } from '../core/models';

export interface AppState {
  ui: any;
  user: User;
  models: any;
  registration: Registration;
}
