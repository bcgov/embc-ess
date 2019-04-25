export interface User {
  id?: string | null;
  name?: string;
  firstname?: string;
  lastname?: string;
  email?: string;
  isNewUser?: boolean;
  contactid?: string;
  userType?: string;
  appRoles: string[];
}
