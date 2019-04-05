import { Community, Region, RegionalDistrict } from './';

export interface Organization {
  active?: boolean;
  adminBCeID: string;
  adminFirstName: string;
  adminLastName: string;
  bCeIDBusinessGuid?: string;
  community: Community;
  id?: string | null;
  legalName?: string;
  name: string;
  region: Region;
  regionalDistrict: RegionalDistrict;
}
