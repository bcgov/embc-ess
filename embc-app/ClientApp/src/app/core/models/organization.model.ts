import { Community, Region } from './';

export interface Organization {
  active?: boolean;
  adminBCeID: string; // in view model only
  adminFirstName: string; // in view model only
  adminLastName: string; // in view model only
  bCeIDBusinessGuid?: string;
  community: Community | null;
  id?: string | null;
  legalName?: string;
  name: string;
  region: Region | null;
}
