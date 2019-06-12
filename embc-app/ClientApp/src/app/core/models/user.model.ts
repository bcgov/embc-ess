export interface User {
  id?: string | null;
  name?: string;
  firstname?: string;
  lastname?: string;
  email?: string;
  isNewUser?: boolean;
  contactid?: string; // is a number
  userType?: string;
  appRoles: string[];
  accountid?: string; // this is actually the org id for non-idir users.
}
